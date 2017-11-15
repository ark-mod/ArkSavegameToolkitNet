using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ArkName = ArkSavegameToolkitNet.Types.ArkName;
using ObjectReference = ArkSavegameToolkitNet.Types.ObjectReference;
using StructPropertyList = ArkSavegameToolkitNet.Structs.StructPropertyList;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterData : ArkClusterDataBase
    {
        private readonly Regex r_clusterFiles = new Regex(@"\d+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private readonly string _savePath;
        private readonly bool _loadOnlyPropertiesInDomain;
        private dynamic[] _newData;

        public ArkClusterData(string savePath, bool loadOnlyPropertiesInDomain = false)
        {
            _savePath = savePath;
            _loadOnlyPropertiesInDomain = loadOnlyPropertiesInDomain;
        }

        public static ArkClusterDataResult<ArkCloudInventory> LoadSingle(string savePath, CancellationToken ct, bool loadOnlyPropertiesInDomain = false, bool decouple = true)
        {
            var success = false;
            var cancelled = false;
            var st = Stopwatch.StartNew();
            ArkCloudInventory data = null;

            try
            {
                var exclusivePropertyNameTree = loadOnlyPropertiesInDomain ? ArkClusterDataContainerBase._alldependencies : null;
                var arkcloudInventories = new ArkSavegameToolkitNet.ArkCloudInventory(savePath, exclusivePropertyNameTree: exclusivePropertyNameTree);

                data = arkcloudInventories.InventoryData?.AsCloudInventory(arkcloudInventories.SteamId, SaveState.FromSaveTime(arkcloudInventories.SaveTime), arkcloudInventories.InventoryDinoData);
                if (decouple) data?.Decouple();
                success = true;
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
            }

            return new ArkClusterDataResult<ArkCloudInventory>
            {
                Success = success,
                Cancelled = cancelled,
                Elapsed = st.Elapsed,
                FilePath = savePath,
                Data = data
            };
        }

        public ArkClusterDataUpdateResult Update(CancellationToken ct, bool deferApplyNewData = false)
        {
            var success = false;
            var cancelled = false;
            ArkSavegame save = null;
            var st = Stopwatch.StartNew();

            try
            {
                var exclusivePropertyNameTree = _loadOnlyPropertiesInDomain ? ArkClusterDataContainerBase._alldependencies : null;

                // Extract all cluster data
                var arkcloudInventories = Directory.GetFiles(_savePath, "*", SearchOption.TopDirectoryOnly).Where(x => /*avoid loading duplicate cluster files*/ r_clusterFiles.IsMatch(Path.GetFileName(x))).Select(x => new ArkSavegameToolkitNet.ArkCloudInventory(x, exclusivePropertyNameTree: exclusivePropertyNameTree)).ToArray();

                var cloudInventories = arkcloudInventories.Where(x => x.InventoryData != null).Select(x => x.InventoryData.AsCloudInventory(x.SteamId, SaveState.FromSaveTime(x.SaveTime), x.InventoryDinoData)).ToArray();

                ApplyOrSaveNewData(deferApplyNewData, cloudInventories);

                success = true;
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
            }
            finally
            {
                save?.Dispose();
            }

            return new ArkClusterDataUpdateResult { Success = success, Cancelled = cancelled, Elapsed = st.Elapsed };
        }
        
        public bool ApplyPreviousUpdate(bool decouple = true)
        {
            if (_newData != null)
            {
                ApplyNewData(_newData[0], decouple);
                _newData = null;
                return true;
            }
            else return false;
        }

        private void ApplyOrSaveNewData(bool deferApplyNewData, ArkCloudInventory[] inventories)
        {
            if (deferApplyNewData)
            {
                _newData = new dynamic[] { inventories };
            }
            else
            {
                ApplyNewData(inventories);
            }
        }

        private void ApplyNewData(ArkCloudInventory[] inventories, bool decouple = true)
        {
            // Setup relations in the domain model between entities
            var newClusterData = new ArkClusterDataBase(inventories);
            newClusterData.Initialize();
            foreach (var i in inventories) i.Initialize(newClusterData);

            if (decouple) //should always be true except for debugging
            {
                // Unset all references to raw extracted game objects and properties to free memory
                foreach (var i in inventories) i.Decouple();
            }

            // Assign updated data to public properties
            newClusterData.CopyTo(this);

            // Force an immediate garbage collection because it seems more effective (extraction process requires a great deal of memory)
            //GC.Collect();
        }
    }
}
