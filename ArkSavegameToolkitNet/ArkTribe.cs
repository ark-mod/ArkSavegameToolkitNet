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
    public class ArkTribe : IGameObjectContainer, IPropertyContainer
    {
        [JsonProperty]
        public IList<GameObject> Objects { get; private set; }

        public IDictionary<ArkName, IProperty> Properties
        {
            get { return Tribe.Properties; }
            set { Tribe.Properties = value; }
        }

        public GameObject Tribe { get; set; }

        public int TribeVersion { get; set; }
        public DateTime SaveTime { get; set; }

        private ArkNameCache _arkNameCache;
        private ArkNameTree _exclusivePropertyNameTree;
        private string _fileName;

        public ArkTribe()
        {
            Objects = new List<GameObject>();
            _arkNameCache = new ArkNameCache();
        }

        public ArkTribe(string fileName, ArkNameCache arkNameCache = null, ArkNameTree exclusivePropertyNameTree = null) : this()
        {
            _fileName = fileName;

            if (arkNameCache != null) _arkNameCache = arkNameCache;
            _exclusivePropertyNameTree = exclusivePropertyNameTree;
            var fi = new FileInfo(fileName);
            SaveTime = fi.LastWriteTimeUtc;
            var size = fi.Length;
            if (size == 0) return;
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open, null, 0L, MemoryMappedFileAccess.Read))
            {
                using (MemoryMappedViewAccessor va = mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read))
                {
                    ArkArchive archive = new ArkArchive(va, size, _arkNameCache, exclusivePropertyNameTree: _exclusivePropertyNameTree);
                    readBinary(archive);
                }
            }
        }

        public virtual void readBinary(ArkArchive archive)
        {
            TribeVersion = archive.GetInt();

            if (TribeVersion != 1)
                throw new System.NotSupportedException($@"Unknown Tribe Version {TribeVersion} in ""{_fileName}""" + (TribeVersion == 0 ? " (possibly corrupt)" : ""));

            var tribesCount = archive.GetInt();
            for (int i = 0; i < tribesCount; i++)
            {
                Objects.Add(new GameObject(archive, _arkNameCache));
            }

            for (int i = 0; i < tribesCount; i++)
            {
                GameObject obj = Objects[i];
                if (obj.ClassName.Token.Equals("PrimalTribeData")) Tribe = obj;
                obj.loadProperties(archive, i < tribesCount - 1 ? Objects[i + 1] : null, 0);
            }
        }
    }
}
