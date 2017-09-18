using ArkSavegameToolkitNet;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ArkCloudInventory : IGameObjectContainer, IPropertyContainer
    {
        [JsonProperty]
        public IList<GameObject> Objects { get; private set; }

        public IDictionary<ArkName, IProperty> Properties
        {
            get { return _inventoryData.Properties; }
            set { _inventoryData.Properties = value; }
        }

        public int InventoryVersion { get; set; }
        public string SteamId { get; set; }
        public DateTime SaveTime { get; set; }

        public ArkCloudInventoryDinoData[] InventoryDinoData { get; set; }
        public virtual GameObject InventoryData
        {
            get { return _inventoryData; }
            set
            {
                if (_inventoryData != null)
                {
                    var oldIndex = Objects.IndexOf(_inventoryData);
                    if (oldIndex > -1)
                    {
                        Objects.RemoveAt(oldIndex);
                    }
                }
                _inventoryData = value;
                if (value != null && Objects.IndexOf(value) == -1)
                {
                    Objects.Insert(0, value);
                }
            }
        }
        private GameObject _inventoryData;
        private ArkNameCache _arkNameCache;

        public ArkCloudInventory()
        {
            Objects = new List<GameObject>();
            InventoryDinoData = new ArkCloudInventoryDinoData[] { };
            _arkNameCache = new ArkNameCache();
        }

        public ArkCloudInventory(string fileName, ArkNameCache arkNameCache = null) : this()
        {
            if (arkNameCache != null) _arkNameCache = arkNameCache;
            var fi = new FileInfo(fileName);
            SteamId = Path.GetFileNameWithoutExtension(fileName);
            SaveTime = fi.LastWriteTimeUtc;
            var size = fi.Length;
            if (size == 0) return;

            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open, null, 0L, MemoryMappedFileAccess.Read))
            {
                using (MemoryMappedViewAccessor va = mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read))
                {
                    ArkArchive archive = new ArkArchive(va, size, _arkNameCache);
                    readBinary(archive);
                }
            }
        }

        public void readBinary(ArkArchive archive)
        {
            InventoryVersion = archive.GetInt();

            //if (InventoryVersion != 1)
            //    throw new NotSupportedException("Unknown Cloud Inventory Version " + InventoryVersion);

            var objectCount = archive.GetInt();
            for (var i = 0; i < objectCount; i++)
            {
                Objects.Add(new GameObject(archive, _arkNameCache));
            }

            for (var i = 0; i < objectCount; i++)
            {
                var obj = Objects[i];
                if (obj.ClassName.Token.Equals("ArkCloudInventoryData")) _inventoryData = obj;
                obj.loadProperties(archive, i < objectCount - 1 ? Objects[i + 1] : null, 0);
            }

            var mydata = InventoryData?.GetPropertyValue<Structs.StructPropertyList>(_myArkData);
            var dinos = mydata?.GetPropertyValue<Arrays.ArkArrayStruct>(_arkTamedDinosData);
            if (dinos != null) InventoryDinoData = dinos.OfType<Structs.StructPropertyList>().Select(x =>
            {
                var version = x.GetPropertyValue<float?>(_version);
                if (!version.HasValue) return null;

                var data = x.GetPropertyValue<Arrays.ArkArrayByte>(_dinoData);

                return new ArkCloudInventoryDinoData(version.Value, data);
            }).ToArray();
        }

        private static readonly ArkName _myArkData = ArkName.Create("MyArkData");
        private static readonly ArkName _arkTamedDinosData = ArkName.Create("ArkTamedDinosData");
        private static readonly ArkName _dinoData = ArkName.Create("DinoData");
        private static readonly ArkName _version = ArkName.Create("Version");
    }
}
