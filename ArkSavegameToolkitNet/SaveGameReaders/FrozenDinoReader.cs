using ArkSavegameToolkitNet.DataReaders;
using ArkSavegameToolkitNet.DataReaders.Extras;
using ArkSavegameToolkitNet.DataTypes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArkSavegameToolkitNet.SaveGameReaders
{
    class FrozenDinoReader : ArchiveReader
    {
        private static ILog _logger = LogManager.GetLogger(typeof(FrozenDinoReader));

        internal FrozenDinoData _saveData;

        internal FrozenDinoReader(ArkToolkitLoader loader, byte[] data) : base(data)
        {
            SaveReaderState = new ArkSaveGameReaderState(loader);

            _structureLog?.PushStack("FrozenDino");
        }

        internal static FrozenDinoData Load(ArkToolkitLoader loader, byte[] data)
        {
            using var sr = new FrozenDinoReader(loader, data);
            sr._saveData = new FrozenDinoData {};
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        /// <summary>
        /// Read frozen dino data
        /// </summary>
        private void ReadBinarySaveData()
        {
            try
            {
                _structureLog?.PushStack("GameObjects");

                GetInt(out var count, "count");

                _saveData.objects = new GameObject[count];
                for (var i = 0; i < count; i++)
                {
                    var obj = GameObjectReader.Get(this);
                    obj.objectId = i;
                    _saveData.objects[i] = obj;
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }

            try
            {
                _structureLog?.PushStack("ObjectProperties");

                for (var i = 0; i < _saveData.objects.Length; i++)
                {
                    var obj = _saveData.objects[i];
                    AdvanceTo(obj.propertiesOffset);

                    foreach (var p in PropertyReader.GetMany(this))
                    {
                        obj.properties.Add(p.name, p);
                    }
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }

            _dataConsumer.Completed();
        }
    }
}
