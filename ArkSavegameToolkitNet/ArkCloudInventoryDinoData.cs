using ArkSavegameToolkitNet.Arrays;
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
    public class ArkCloudInventoryDinoData : IGameObjectContainer, ICloudInventoryDinoData
    {
        [JsonProperty]
        public IList<GameObject> Objects { get; private set; }
        public IGameObject Creature { get; private set; }
        public IGameObject Status { get; private set; }
        public IGameObject Inventory { get; private set; }

        public float DinoDataVersion { get; set; }

        private ArkNameCache _arkNameCache;
        private ArkNameTree _exclusivePropertyNameTree;

        public ArkCloudInventoryDinoData()
        {
            Objects = new List<GameObject>();
            _arkNameCache = new ArkNameCache();
        }

        public ArkCloudInventoryDinoData(float version, ArkArrayByte dinoData, ArkNameCache arkNameCache = null, ArkNameTree exclusivePropertyNameTree = null) : this()
        {
            //if (version != 3.0) throw new NotSupportedException("Unknown Cloud Inventory Dino Data Version " + version);
            DinoDataVersion = version;

            if (dinoData == null || dinoData.Any(x => !x.HasValue)) throw new NotSupportedException("DinoData was null or contained null-values.");

            if (arkNameCache != null) _arkNameCache = arkNameCache;
            _exclusivePropertyNameTree = exclusivePropertyNameTree;


            //since the ArkArchive implementation we have takes a MemoryMappedViewAccessor for reading operations lets do the lazy thing and write this managed byte[] data to a new memory mapped file
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("ArkCloudInventoryDinoData", dinoData.Count, MemoryMappedFileAccess.ReadWrite))
            {
                using (MemoryMappedViewAccessor va = mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.ReadWrite))
                {
                    va.WriteArray(0, dinoData.Select(x => x.Value).ToArray(), 0, dinoData.Count);
                    ArkArchive archive = new ArkArchive(va, dinoData.Count, _arkNameCache, exclusivePropertyNameTree: _exclusivePropertyNameTree);
                    readBinary(archive);
                }
            }
        }

        public void readBinary(ArkArchive archive)
        {
            var objectCount = archive.GetInt();
            for (var i = 0; i < objectCount; i++)
            {
                Objects.Add(new GameObject(archive, _arkNameCache));
            }

            for (var i = 0; i < objectCount; i++)
            {
                var obj = Objects[i];
                obj.loadProperties(archive, i < objectCount - 1 ? Objects[i + 1] : null, 0);

                if (obj.IsCreature) Creature = obj;
                else if (obj.IsDinoStatusComponent) Status = obj;
                else if (obj.IsTamedCreatureInventory) Inventory = obj;
            }
        }
    }
}
