using ArkSavegameToolkitNet.DataReaders;
using ArkSavegameToolkitNet.DataReaders.Extras;
using ArkSavegameToolkitNet.DataTypes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArkSavegameToolkitNet.SaveGameReaders
{
    [System.Reflection.Obfuscation(ApplyToMembers =true, StripAfterObfuscation = true, Exclude = false, Feature = "renaming")]
    internal class ArkSaveGameReader : ArchiveReader
    {
        private static ILog _logger = LogManager.GetLogger("toolkit", "arksavegamereader"); //typeof(ArkSaveGameReader));

        private const bool _readDataFiles = true;
        private const bool _readEmbeddedData = true;
        private const bool _readDataFilesObjectMap = true;
        private const bool _readGameObjects = true;
        private const bool _readGameObjectProperties = true;
        private const bool _readHibernationEntries = true;


        internal ArkSaveData _saveData;
        

        internal int _nameTableOffset;
        internal int _propertiesBlockOffset;
        internal int _hibernationOffset;

        internal int[] _objectPropertiesOffsets;

        private bool _unknownDataFound;

        internal ArkSaveGameReader(ArkToolkitLoader loader) : base(loader.BufferSize)
        {
            SaveReaderState = new ArkSaveGameReaderState(loader);

            _structureLog?.PushStack("ARK");
        }

        internal ArkSaveGameReader(ArkToolkitLoader loader, byte[] data) : base(data)
        {
            SaveReaderState = new ArkSaveGameReaderState(loader);

            _structureLog?.PushStack("ARK");
        }

        internal static ArkSaveData Load(ArkToolkitLoader loader, string fileName)
        {
            var fi = new FileInfo(fileName);
            var size = fi.Length;

            // save time is not stored in the savegame - our fallback is to read the last modified date in the filesystem
            // it's far from ideal, but it's all we have
            var savedAt = fi.LastWriteTimeUtc;

            using var sr = new ArkSaveGameReader(loader);
            sr._stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr._saveData = new ArkSaveData { fileName = fileName, size = size, savedAt = savedAt };
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        internal static ArkSaveData Load(ArkToolkitLoader loader, Stream stream, string madeUpFileName, DateTime savedAt)
        {
            using var sr = new ArkSaveGameReader(loader);
            sr._saveData = new ArkSaveData { fileName = madeUpFileName, size = stream.Length, savedAt = savedAt };
            sr._stream = stream;
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        internal static ArkSaveData Load(ArkToolkitLoader loader, byte[] data, string madeUpFileName, DateTime savedAt)
        {
            using var sr = new ArkSaveGameReader(loader, data);
            sr._saveData = new ArkSaveData { fileName = madeUpFileName, size = data.Length, savedAt = savedAt };
            sr.ReadBinarySaveData();

            return sr._saveData;
        }

        private void ReadBinarySaveData()
        {
            ReadSavegame();

            _dataConsumer.Completed();
        }

        /// <summary>
        /// Read savegame
        /// </summary>
        private void ReadSavegame()
        {
            ReadHeader();

            // need name table before reading game objects (seeks to _nameTableOffset)
            if (_saveData.saveVersion > 5) ReadNameTable(); 

            ReadDataFiles();
            ReadEmbeddedData();
            ReadDataFilesObjectMap();

            ReadGameObjectsAndProperties();

            if (_saveData.saveVersion > 6) ReadHibernationEntries();

            //todo: did not implement _oldNameList reading
            if (_unknownDataFound) throw new NotImplementedException("Have not implemented _oldNameList");
        }

        /// <summary>
        /// Read header
        /// </summary>
        internal void ReadHeader()
        {
            try
            {
                _structureLog?.PushStack("Header");

                GetShort(out _saveData.saveVersion, "saveVersion");

                if (_saveData.saveVersion < 5 || _saveData.saveVersion > 9)
                {
                    var msg = $"Found unknown Version {_saveData.saveVersion}";
                    _logger.Error(msg);
                    throw new NotSupportedException(msg);
                }

                if (_saveData.saveVersion > 6)
                {
                    GetInt(out _hibernationOffset, "hibernationOffset");
                    GetInt(out var shouldBeZero, "shouldBeZero");
                    if (shouldBeZero != 0)
                    {
                        var msg = $"Found unexpected Value {shouldBeZero} at {_position - 4:X}";
                        _logger.Error(msg);
                        throw new System.NotSupportedException(msg);
                    }
                }

                if (_saveData.saveVersion > 5)
                {
                    GetInt(out _nameTableOffset, "nameTableOffset");
                    GetInt(out _propertiesBlockOffset, "propertiesBlockOffset");
                }

                GetFloat(out _saveData.gameTime, "gameTime");

                if (_saveData.saveVersion > 8)
                {
                    GetInt(out _saveData.saveCount, "saveCount");
                }
                
                _dataConsumer.Push(_saveData);

            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        /// <summary>
        /// Read name table
        /// </summary>
        internal void ReadNameTable()
        {
            var prevPos = _position;
            SeekTo(_nameTableOffset);

            try
            {
                _structureLog?.PushStack("NameTable");

                GetInt(out var nameCount, "nameCount");
                var nameTable = new string[nameCount];
                for (var i = 0; i < nameCount; i++)
                {
                    nameTable[i] = GetString("nameTable");
                }

                SetNameTable(nameTable, out _);
                
            }
            finally
            {
                _structureLog?.PopStack();
                SeekTo(prevPos);
            }
        }

        /// <summary>
        /// Read data files
        /// </summary>
        private void ReadDataFiles()
        {
            try
            {
                _structureLog?.PushStack("DataFiles");

                GetInt(out var count, "count");

                for (var i = 0; i < count; i++)
                {
                    if (_readDataFiles)
                    {
                        var dataFile = GetString("dataFiles");

                        if (i == 0) _saveData.mapName = dataFile;

                        //todo: data files are not consumed
                        //_dataConsumer.Push(dataFile);
                    }
                    else SkipString();
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }
            
        }

        /// <summary>
        /// Read embedded data
        /// </summary>
        private void ReadEmbeddedData()
        {
            try
            {
                _structureLog?.PushStack("EmbeddedDatas");

                GetInt(out var count, "count");

                for (var i = 0; i < count; i++)
                {
                    if (_readEmbeddedData)
                    {
                        var embeddedData = EmbeddedDataReader.Get(this);
                        _dataConsumer.Push(embeddedData);
                    }
                    else EmbeddedDataReader.Skip(this);
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }
            
        }

        /// <summary>
        /// Read data files object map
        /// </summary>
        private void ReadDataFilesObjectMap()
        {
            try
            {
                _structureLog?.PushStack("DataFilesObjectMap");

                GetInt(out var dataFilesCount, "dataFilesCount");

                var dataFiles = (Dictionary<int, string[]>)null;
                if (_readDataFilesObjectMap) dataFiles = new Dictionary<int, string[]>(dataFilesCount);
                else _unknownDataFound = true;

                for (var i = 0; i < dataFilesCount; i++)
                {
                    try
                    {
                        _structureLog?.PushStack($"DataFile");

                        var level = 0;
                        if (_readDataFilesObjectMap) GetInt(out level, "level");
                        else Advance(4);

                        GetInt(out var count, "count");

                        var names = (string[])null;
                        if (_readDataFilesObjectMap) names = new string[count];
                        for (var j = 0; j < count; j++)
                        {
                            if (_readDataFilesObjectMap) names[j] = GetString("names");
                            else SkipString();
                        }

                        if (!dataFiles.ContainsKey(level)) dataFiles.Add(level, names);
                        else dataFiles[level] = names;
                    }
                    finally
                    {
                        _structureLog?.PopStack();
                    }
                }
                //todo: data files object map are not consumed
                //_dataConsumer.Push(dataFiles);
            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        /// <summary>
        /// Read game objects and properties
        /// </summary>
        private void ReadGameObjectsAndProperties()
        {
            //todo: no filtering supported for game objects or properties

            if (!_readGameObjects || !_readGameObjectProperties) return;

            try
            {
                _structureLog?.PushStack("GameObjectsAndProperties");

                GetInt(out var count, "count");

                var propBuffer = CreateBuffer(_propertiesBlockOffset);
                var objectBuffer = new ArchiveReaderBuffer();

                for (var i = 0; i < count; i++)
                {
                    var obj = GameObjectReader.Get(this);
                    obj.objectId = i;

                    var objPos = _position;
                    var propPos = (long)_propertiesBlockOffset + obj.propertiesOffset;
                    HotSwapBuffer(propBuffer, ref objectBuffer);
                    AdvanceTo(propPos);

                    foreach (var p in PropertyReader.GetMany(this))
                    {
                        obj.properties.Add(p.name, p);
                    }

                    // hot swap back to object buffer, or do a manual seek
                    if(!HotSwapBuffer(objectBuffer, ref propBuffer)) SeekTo(objPos);

                    _dataConsumer.Push(obj);
                }

                ReleaseBuffer(propBuffer);
            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        /// <summary>
        /// Read hibernation entries
        /// </summary>
        private void ReadHibernationEntries()
        {
            if (!_readHibernationEntries)
            {
                _unknownDataFound = true;
                return;
            }

            SeekTo(_hibernationOffset);

            try
            {
                _structureLog?.PushStack("HibernationEntries");

                if (_saveData.saveVersion > 7)
                {
                    GetInt(out var hibernationV8Unknown1, "hibernationV8Unknown1");
                    GetInt(out var hibernationV8Unknown2, "hibernationV8Unknown2");
                    GetInt(out var hibernationV8Unknown3, "hibernationV8Unknown3");
                    GetInt(out var hibernationV8Unknown4, "hibernationV8Unknown4");
                }

                // No hibernate section if we reached the nameTable
                if (_position != _nameTableOffset)
                {
                    GetInt(out var hibernationUnknown1, "hibernationUnknown1");
                    GetInt(out var hibernationUnknown2, "hibernationUnknown2");
                    GetInt(out var ccount, "ccount");

                    var hibernationClasses = new string[ccount];
                    for (var i = 0; i < ccount; i++)
                    {
                        hibernationClasses[i] = GetString("hibernationClasses");
                        //todo: hibernation classes is not consumed
                    }

                    GetInt(out var icount, "icount");
                    if (icount != ccount)
                    {
                        var msg = $"hibernatedClassesCount ({ccount}) does not match hibernatedIndicesCount ({icount}) at {_position - 4:X}";
                        _logger.Error(msg);
                        throw new System.NotSupportedException(msg);
                    }

                    var hibernationIndices = new int[icount];
                    for (var i = 0; i < icount; i++)
                    {
                        GetInt(out var index, "index");
                        hibernationIndices[i] = index;
                        //todo: hibernation indices is not consumed
                    }

                    GetInt(out var ocount, "ocount");
                    for (var i = 0; i < ocount; i++)
                    {
                        var hibernationEntry = HibernationEntryReader.Get(this);
                        _dataConsumer.Push(hibernationEntry);
                    }
                }
            }
            finally
            {
                _structureLog?.PopStack();
                //SeekTo(prevPos); // for hibernation section we do not reset the position
            }
        }
    }
}
