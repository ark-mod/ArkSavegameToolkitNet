using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class HibernationEntry
    {
        public IEnumerable<GameObject> Objects
        {
           get
            {
                return _objects;
            }
        }

        private List<GameObject> _objects = new List<GameObject>();
        private List<string> _zoneVolumes = new List<string>();
        private  List<string> _nameTable = new List<string>();
        public int _classIndex;
        public HibernationEntry(ArkArchive archive)
        {
            readBinary(archive);
        }

        public void readBinary(ArkArchive archive)
        {
            var x = archive.GetFloat();
            var y = archive.GetFloat();
            var z = archive.GetFloat();
            var unkByte = archive.GetByte();
            var unkFloat = archive.GetFloat();
            var nameArchiveSize = archive.GetInt();
            var nameArchive = archive.Slice( archive.Position, nameArchiveSize);
            readBinaryNameTable(nameArchive);
            archive.Position += nameArchiveSize;
            var objectArchiveSize = archive.GetInt();
            var objectArchive = archive.Slice( archive.Position, objectArchiveSize);
            readBinaryObjects(objectArchive);
            archive.Position += objectArchiveSize;
            var unkInt1 = archive.GetInt();
            _classIndex = archive.GetInt();
        }

        private void readBinaryNameTable(ArkArchive archive)
        {
            var version = archive.GetInt();
            if (version != 3)
                throw new NotSupportedException($"hibernation version not supported <>3");
            var nameCount = archive.GetInt();
            for (int i = 0; i <= nameCount - 1; i++)
            {
                _nameTable.Add(archive.GetString());
            }

            var zoneCount = archive.GetInt();
            for (int i = 0; i <= zoneCount - 1; i++)
            {
                _zoneVolumes.Add(archive.GetString());
            }
        }

        private void readBinaryObjects(ArkArchive archive)
        {
            var objectCount = archive.GetInt();
            for (int i = 0; i <= objectCount - 1; i++)
            {
                var go = new GameObject(archive);
                go.ObjectId = i;
                _objects.Add(go);
            }

            archive.SetNameTable(_nameTable, 0, true);
            for (int n = 0; n <= objectCount - 1; n++)
            {
                var go = _objects[n];
                _objects[n].loadProperties(archive, (n < _objects.Count - 1) ? _objects[n + 1] : null, 0);
            }
        }

        public override string ToString()
        {
            return $"HibernationEntry: Objects: {_objects.Count} ClassIndex:{_classIndex}";
        }
    }
}
