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
    public class ArkLocalProfile : IGameObjectContainer, IPropertyContainer
    {
        [JsonProperty]
        public IList<GameObject> Objects { get; private set; }

        public IDictionary<ArkName, IProperty> Properties
        {
            get { return _localprofile.Properties; }
            set { _localprofile.Properties = value; }
        }

        public int LocalProfileVersion { get; set; }
        public DateTime SaveTime { get; set; }

        public GameObject LocalProfile
        {
            get { return _localprofile; }
            set
            {
                if (_localprofile != null)
                {
                    int oldIndex = Objects.IndexOf(_localprofile);
                    if (oldIndex > -1)
                    {
                        Objects.RemoveAt(oldIndex);
                    }
                }
                _localprofile = value;
                if (value != null && Objects.IndexOf(value) == -1)
                {
                    Objects.Insert(0, value);
                }
            }
        }
        private GameObject _localprofile;
        private sbyte[] unknownData;
        private ArkNameCache _arkNameCache;
        private string _fileName;

        public ArkLocalProfile()
        {
            Objects = new List<GameObject>();
            _arkNameCache = new ArkNameCache();
        }

        public ArkLocalProfile(string fileName, ArkNameCache arkNameCache = null) : this()
        {
            _fileName = fileName;

            if (arkNameCache != null) _arkNameCache = arkNameCache;
            var fi = new FileInfo(fileName);
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
            LocalProfileVersion = archive.GetInt();

            if (LocalProfileVersion != 1)
                throw new NotSupportedException($@"Unknown Local Profile Version {LocalProfileVersion} in ""{_fileName}""" + (LocalProfileVersion == 0 ? " (possibly corrupt)" : ""));

            var unknownDataSize = archive.GetInt();
            unknownData = archive.GetBytes(unknownDataSize);

            var profilesCount = archive.GetInt();
            for (int i = 0; i < profilesCount; i++)
            {
                Objects.Add(new GameObject(archive, _arkNameCache));
            }

            for (int i = 0; i < profilesCount; i++)
            {
                GameObject obj = Objects[i];
                if (obj.ClassName.Token.Equals("PrimalLocalProfile")) _localprofile = obj;
                obj.loadProperties(archive, i < profilesCount - 1 ? Objects[i + 1] : null, 0);
            }
        }
    }
}
