using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Extras;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Extras
{
    static class HibernationEntryReader
    {
        private const bool _readHibernationObjectProperties = true;

        public static HibernationEntry Get(ArchiveReader ar)
        {
            var result = new HibernationEntry();

            try
            {
                ar._structureLog?.PushStack("HibernationEntry");

                ar.GetFloat(out result.x, "x");
                ar.GetFloat(out result.y, "y");
                ar.GetFloat(out result.z, "z");
                ar.GetByte(out var unkByte, "unkByte");
                ar.GetFloat(out var unkFloat, "unkFloat");

                ar.GetInt(out var nameTableSize, "nameTableSize");
                var pos = ar._position;
                if (_readHibernationObjectProperties)
                {
                    ar.SetSection(nameTableSize, out var prevSection1);
                    ar.SetNameTable(null, out var prevState1, 0, false);
                    readBinaryNameTable(ar, out result.nameTable, out result.zoneVolumes);
                    ar.SetNameTable(prevState1);
                    ar.SetSection(prevSection1);
                }
                else
                {
                    ar.Advance(nameTableSize);

                    // Unknown data since the missed names are unrelated to the main nameTable
                    //todo: not implemented
                }

                if (ar._position != pos + nameTableSize)
                {
                    //todo: log unknown data and skip forward
                    ar.Advance((int)(pos + nameTableSize - ar._position));
                }

                ar.GetInt(out var objectsSize, "objectsSize");
                pos = ar._position;

                ar.SetSection(objectsSize, out var prevSection2);
                ar.SetNameTable(null, out var prevState2, 0, false);
                readBinaryObjects(ar, result.nameTable, out result.objects);
                ar.SetNameTable(prevState2);
                ar.SetSection(prevSection2);

                if (ar._position != pos + objectsSize)
                {
                    //todo: log unknown data and skip forward
                    ar.Advance((int)(pos + objectsSize - ar._position));
                }

                ar.GetInt(out var unkInt1, "unkInt1");
                ar.GetInt(out result.classIndex, "classIndex");

                return result;
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }

        private static void readBinaryNameTable(ArchiveReader ar, out string[] nameTable, out ArkName[] zoneVolumes)
        {
            try
            {
                ar._structureLog?.PushStack("NameTable");

                ar.GetInt(out var version, "version");
                if (version != 3)
                {
                    var msg = $@"Unknown HibernationEntry Version {version} at {ar._position - 4:X}";
                    throw new System.NotSupportedException(msg);
                }

                ar.GetInt(out var ncount, "ncount");
                nameTable = new string[ncount];
                for (var i = 0; i < ncount; i++) nameTable[i] = ar.GetString("nameTable");

                ar.GetInt(out var zcount, "zcount");
                zoneVolumes = new ArkName[zcount];
                for (var i = 0; i < zcount; i++) zoneVolumes[i] = ar.GetName("zoneVolumes");
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }

        private static void readBinaryObjects(ArchiveReader ar, string[] nameTable, out List<GameObject> objects)
        {
            try
            {
                ar._structureLog?.PushStack("GameObjects");

                ar.GetInt(out var count, "count");
                objects = new List<GameObject>(count);
                for (var index = 0; index < count; index++)
                {
                    var obj = GameObjectReader.Get(ar);
                    objects.Add(obj);
                }

                if (nameTable == null) return;

                ar.SetNameTable(nameTable, out var prevState, 0, true);

                for (var index = 0; index < count; index++)
                {
                    var obj = objects[index];

                    var offset = ar._sectionState.position + obj.propertiesOffset;
                    ar.AdvanceTo(offset);

                    foreach (var p in PropertyReader.GetMany(ar))
                    {
                        ar._dataConsumer.Push(p);
                    }
                }

                ar.SetNameTable(prevState);
                ar.AdvanceTo(ar._sectionState.position + ar._sectionState.size);
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }
    }
}