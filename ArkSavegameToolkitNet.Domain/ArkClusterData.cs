using ArkSavegameToolkitNet.Domain.Utils.Extensions;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterData : ArkClusterDataBase
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ArkClusterData));

        private readonly Regex r_clusterFiles = new Regex(@"^\d+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
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

                var settings = ArkToolkit.DefaultSettings.Invoke();
                settings.ExclusivePropertyNameTree = exclusivePropertyNameTree;

                var arkcloudInventories = ArkToolkitLoader.Create(settings).LoadClusterSave(savePath);

                var inventoryData = arkcloudInventories.objects.FirstOrDefault(x => x.className.Token.Equals("ArkCloudInventoryData"));

                data = inventoryData?.AsCloudInventory(arkcloudInventories.steamId, arkcloudInventories);
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

        public ArkClusterDataUpdateResult Update(CancellationToken ct, bool deferApplyNewData = false, ArkAnonymizeData anonymize = null)
        {
            var success = false;
            var cancelled = false;
            var st = Stopwatch.StartNew();

            try
            {
                var exclusivePropertyNameTree = _loadOnlyPropertiesInDomain ? ArkClusterDataContainerBase._alldependencies : null;

                // Extract all cluster data
                var arkcloudInventories = Directory.GetFiles(_savePath, "*", SearchOption.TopDirectoryOnly)
                    .Where(x => /*avoid loading duplicate cluster files*/ r_clusterFiles.IsMatch(Path.GetFileName(x)))
                    .Select(x =>
                    {
                        try
                        {
                            var settings = ArkToolkit.DefaultSettings.Invoke();
                            settings.ExclusivePropertyNameTree = exclusivePropertyNameTree;

                            return ArkToolkitLoader.Create(settings).LoadClusterSave(x);
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn($"Failed to load cluster save", ex);
                        }

                        return null;
                    }).Where(x => x != null).ToArray();

                var cloudInventories = arkcloudInventories.Select(x =>
                {
                    var inventoryData = x.objects.FirstOrDefault(x => x.className.Token.Equals("ArkCloudInventoryData"));

                    return new { ci = x, id = inventoryData };
                }).Where(x => x.id != null).Select(x => x.id.AsCloudInventory(x.ci.steamId, x.ci)).ToArray();

                ApplyOrSaveNewData(deferApplyNewData, cloudInventories);

                success = true;
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
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
