using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkSavegameToolkitNet.Property;
using Newtonsoft.Json;
using ArkSavegameToolkitNet.Types;

namespace ArkSavegameToolkitNet
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ArkProfile : IGameObjectContainer, IPropertyContainer
    {
        [JsonProperty]
        public IList<GameObject> Objects { get; private set; }

        public IDictionary<ArkName, IProperty> Properties
        {
            get { return _profile.Properties; }
            set { _profile.Properties = value; }
        }

        public int ProfileVersion { get; set; }
        public DateTime SaveTime { get; set; }

        public GameObject Profile
        {
            get { return _profile; }
            set
            {
                if (_profile != null)
                {
                    var oldIndex = Objects.IndexOf(_profile);
                    if (oldIndex > -1)
                    {
                        Objects.RemoveAt(oldIndex);
                    }
                }
                _profile = value;
                if (value != null && Objects.IndexOf(value) == -1)
                {
                    Objects.Insert(0, value);
                }
            }
        }
        private GameObject _profile;
        private ArkNameCache _arkNameCache;
        private ArkNameTree _exclusivePropertyNameTree;
        private string _fileName;

        public ArkProfile()
        {
            Objects = new List<GameObject>();
            _arkNameCache = new ArkNameCache();
        }

        public ArkProfile(string fileName, ArkNameCache arkNameCache = null, ArkNameTree exclusivePropertyNameTree = null) : this()
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

        public void readBinary(ArkArchive archive)
        {
            ProfileVersion = archive.GetInt();

            if (ProfileVersion != 1)
                throw new NotSupportedException($@"Unknown Profile Version {ProfileVersion} in ""{_fileName}""" + (ProfileVersion == 0 ? " (possibly corrupt)" : ""));

            var profilesCount = archive.GetInt();
            for (var i = 0; i < profilesCount; i++)
            {
                Objects.Add(new GameObject(archive, _arkNameCache));
            }

            for (var i = 0; i < profilesCount; i++)
            {
                var obj = Objects[i];
                if (obj.ClassName.Token.Equals("PrimalPlayerData") || obj.ClassName.Token.Equals("PrimalPlayerDataBP_C")) _profile = obj;
                obj.loadProperties(archive, i < profilesCount - 1 ? Objects[i + 1] : null, 0);
            }
        }
    }
}
