﻿using ArkSavegameToolkitNet.DataReaders;
using ArkSavegameToolkitNet.DataReaders.Extras;
using ArkSavegameToolkitNet.DataTypes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArkSavegameToolkitNet.SaveGameReaders
{
    class ProfileSaveGameReader : ArchiveReader
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ProfileSaveGameReader));

        internal ProfileSaveGameData _saveData;

        internal ProfileSaveGameReader(ArkToolkitLoader loader) : base(loader.BufferSize)
        {
            SaveReaderState = new ArkSaveGameReaderState(loader);

            _structureLog?.PushStack("Profile");
        }

        internal ProfileSaveGameReader(ArkToolkitLoader loader, byte[] data) : base(data)
        {
            SaveReaderState = new ArkSaveGameReaderState(loader);

            _structureLog?.PushStack("Profile");
        }

        internal static ProfileSaveGameData Load(ArkToolkitLoader loader, string fileName)
        {
            var fi = new FileInfo(fileName);
            var size = fi.Length;

            // save time is not stored in the savegame - our fallback is to read the last modified date in the filesystem
            // it's far from ideal, but it's all we have
            var savedAt = fi.LastWriteTimeUtc;

            using var sr = new ProfileSaveGameReader(loader);
            sr._stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr._saveData = new ProfileSaveGameData { fileName = fileName, size = size, savedAt = savedAt };
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        internal static ProfileSaveGameData Load(ArkToolkitLoader loader, Stream stream, string madeUpFileName, DateTime savedAt)
        {
            using var sr = new ProfileSaveGameReader(loader);
            sr._saveData = new ProfileSaveGameData { fileName = madeUpFileName, size = stream.Length, savedAt = savedAt };
            sr._stream = stream;
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        internal static ProfileSaveGameData Load(ArkToolkitLoader loader, byte[] data, string madeUpFileName, DateTime savedAt)
        {
            using var sr = new ProfileSaveGameReader(loader, data);
            sr._saveData = new ProfileSaveGameData { fileName = madeUpFileName, size = data.Length, savedAt = savedAt };
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        /// <summary>
        /// Read profile save
        /// </summary>
        private void ReadBinarySaveData()
        {
            try
            {
                _structureLog?.PushStack("Header");

                GetInt(out _saveData.saveVersion, "saveVersion");

                if (_saveData.saveVersion != 1)
                {
                    var msg = $"Found unknown profile save version {_saveData.saveVersion} in '{_saveData.fileName}'";
                    _logger.Error(msg);
                    throw new NotSupportedException(msg);
                }

            }
            finally
            {
                _structureLog?.PopStack();
            }

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
