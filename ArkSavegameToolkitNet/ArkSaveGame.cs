using ArkSavegameToolkitNet.Types;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ArkSavegame : IGameObjectContainer, IDisposable
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ArkSavegame));

        [JsonProperty]
        public IList<GameObject> Objects
        {
            get { return _objects; }
            private set
            {
                if (value == null) throw new NullReferenceException("Value cannot be null");
                _objects = value;
            }
        }
        private IList<GameObject> _objects = new List<GameObject>();

        [JsonProperty]
        public IList<string> DataFiles
        {
            get { return _dataFiles; }
            set
            {
                if (value == null) throw new NullReferenceException("Value cannot be null");
                _dataFiles = value;
            }
        }
        private IList<string> _dataFiles = new List<string>();

        [JsonProperty]
        public IList<EmbeddedData> EmbeddedData
        {
            get { return _embeddedData; }
            set
            {
                if (value == null) throw new NullReferenceException("Value cannot be null");
                _embeddedData = value;
            }
        }
        private IList<EmbeddedData> _embeddedData = new List<EmbeddedData>();

        [JsonProperty]
        public short SaveVersion { get; set; }
        [JsonProperty]
        public float GameTime { get; set; }
        //the only way to get this is by looking at the last modified date of the savegame file. this may not be correct if not read from an active save on the server
        [JsonProperty]
        public DateTime SaveTime { get; set; }
        public SaveState SaveState { get; set; }

        protected internal int binaryDataOffset;
        protected internal int nameTableOffset;
        protected internal int propertiesBlockOffset;
        private string _fileName;
        private bool _baseRead;
        private long _gameObjectsOffset;
        private ArkNameCache _arkNameCache;

        private MemoryMappedFile _mmf;
        private MemoryMappedViewAccessor _va;
        private ArkArchive _archive;

        public ArkSavegame()
        {
            Objects = new List<GameObject>();
            _arkNameCache = new ArkNameCache();
    }

        public ArkSavegame(string fileName, ArkNameCache arkNameCache = null)
        {
            _fileName = fileName;
            _arkNameCache = arkNameCache ?? new ArkNameCache();

            var fi = new FileInfo(_fileName);
            var size = fi.Length;
            SaveTime = fi.LastWriteTimeUtc;
            //if (size == 0) return false;

            _mmf = MemoryMappedFile.CreateFromFile(_fileName, FileMode.Open, null, 0L, MemoryMappedFileAccess.Read);
            _va = _mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read);
            _archive = new ArkArchive(_va, size, _arkNameCache);
        }

        /// <summary>
        /// Load all gameobjects, properties and other data in this savegame.
        /// </summary>
        public bool LoadEverything()
        {
            //var fi = new FileInfo(_fileName);
            //var size = fi.Length;
            //SaveTime = fi.LastWriteTimeUtc;
            //if (size == 0) return false;

            //using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(_fileName, FileMode.Open))
            //{
            //    using (MemoryMappedViewAccessor va = mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read))
            //    {
            //        ArkArchive archive = new ArkArchive(va, size, _arkNameCache);
            //        readBinary(archive, mmf);
            //    }
            //}

            readBinary(_archive, _mmf);

            return true;
        }

        public GameObject GetObjectAtOffset(long offset, int nextPropertiesOffset)
        {
            var oldposition = _archive.Position;
            _archive.Position = offset;
            var gameObject = new GameObject(_archive, _arkNameCache);
            gameObject.loadProperties(_archive, null, propertiesBlockOffset, nextPropertiesOffset);
            _archive.Position = oldposition;

            return gameObject;
        }

        public IEnumerable<Tuple<GameObjectReader, GameObjectReader>> GetObjectsEnumerable()
        {
            var fi = new FileInfo(_fileName);
            var size = fi.Length;
            SaveTime = fi.LastWriteTimeUtc;
            if (size == 0) yield break;

            //using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(_fileName, FileMode.Open, null, 0L, MemoryMappedFileAccess.Read))
            //{
            //    using (MemoryMappedViewAccessor va = mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read))
            //    {
            //        ArkArchive archive = new ArkArchive(va, size, _arkNameCache);
            //        if (!_baseRead) readBinaryBase(archive);
            //        else archive.Position = _gameObjectsOffset;

            //        var count = archive.GetInt();
            //        for (var n = 0; n < count; n++)
            //        {
            //            var gameObject = new GameObjectReader(archive, _arkNameCache) { Index = n };
            //            archive.Position += gameObject.Size;
            //            yield return gameObject;
            //        }
            //    }
            //}

            if (!_baseRead) readBinaryBase(_archive);
            else _archive.Position = _gameObjectsOffset;

            var count = _archive.GetInt();
            GameObjectReader prev = null;
            for (var n = 0; n <= count; n++)
            {
                GameObjectReader gameObject = null;
                if (n < count)
                {
                    gameObject = new GameObjectReader(_archive, _arkNameCache) { Index = n };
                    _archive.Position += gameObject.Size;
                }

                if (n > 0) yield return Tuple.Create(prev, gameObject);
                prev = gameObject;
            }
        }

        public void readBinary(ArkArchive archive, MemoryMappedFile mmf)
        {
            if (!_baseRead) readBinaryBase(archive);
            else archive.Position = _gameObjectsOffset;
            readBinaryObjects(archive);
            readBinaryObjectProperties(archive, mmf);
        }

        private void readBinaryBase(ArkArchive archive)
        {
            readBinaryHeader(archive);

            if (SaveVersion > 5)
            {
                // Name table is located after the objects block, but will be needed to read the objects block
                readBinaryNameTable(archive);
            }

            readBinaryDataFiles(archive);

            SaveState = new SaveState { GameTime = GameTime, SaveTime = SaveTime, MapName = DataFiles.FirstOrDefault() };

            readBinaryEmbeddedData(archive);

            var unknownValue = archive.GetInt();
            if (unknownValue != 0)
            {
                //if (unknownValue > 2)
                //{
                //    var msg = $"Found unexpected Value {unknownValue} at {archive.Position - 4:X}";
                //    _logger.Error(msg);
                //    throw new System.NotSupportedException(msg);
                //}

                for (int n = 0; n < unknownValue; n++)
                {
                    var unknownFlags = archive.GetInt();
                    var objectCount = archive.GetInt();
                    var name = archive.GetString();
                }
            }
            _baseRead = true;
            _gameObjectsOffset = archive.Position;
        }

        protected void readBinaryHeader(ArkArchive archive)
        {
            SaveVersion = archive.GetShort();

            if (SaveVersion == 5)
            {
                GameTime = archive.GetFloat();

                propertiesBlockOffset = 0;
            }
            else if (SaveVersion == 6)
            {
                nameTableOffset = archive.GetInt();
                propertiesBlockOffset = archive.GetInt();
                GameTime = archive.GetFloat();
            }
            else if (SaveVersion == 7 || SaveVersion == 8)
            {
                binaryDataOffset = archive.GetInt();
                var unknownValue = archive.GetInt();
                if (unknownValue != 0)
                {
                    var msg = $"Found unexpected Value {unknownValue} at {archive.Position - 4:X}";
                    _logger.Error(msg);
                    throw new System.NotSupportedException(msg);
                }

                nameTableOffset = archive.GetInt();
                propertiesBlockOffset = archive.GetInt();
                GameTime = archive.GetFloat();
            }
            else if (SaveVersion == 9)
            {
                binaryDataOffset = archive.GetInt();
                var unknownValue = archive.GetInt();
                if (unknownValue != 0)
                {
                    var msg = $"Found unexpected Value {unknownValue} at {archive.Position - 4:X}";
                    _logger.Error(msg);
                    throw new System.NotSupportedException(msg);
                }

                nameTableOffset = archive.GetInt();
                propertiesBlockOffset = archive.GetInt();
                GameTime = archive.GetFloat();

                //note: unknown integer value was added in v268.22 with SaveVersion=9 [0x25 (37) on The Island, 0x26 (38) on ragnarok/center/scorched]
                var unknownValue2 = archive.GetInt();
            }
            else
            {
                var msg = $"Found unknown Version {SaveVersion}";
                _logger.Error(msg);
                throw new System.NotSupportedException(msg);
            }
        }

        protected void readBinaryNameTable(ArkArchive archive)
        {
            var position = archive.Position;

            archive.Position = nameTableOffset;

            var nameCount = archive.GetInt();
            var nameTable = new List<string>(nameCount);
            for (var n = 0; n < nameCount; n++)
            {
                nameTable.Add(archive.GetString());
            }

            archive.NameTable = nameTable;

            archive.Position = position;
        }

        protected void readBinaryDataFiles(ArkArchive archive, bool dataFiles = true)
        {
            var count = archive.GetInt();
            if (dataFiles)
            {
                DataFiles.Clear();
                for (var n = 0; n < count; n++)
                {
                    DataFiles.Add(archive.GetString());
                }
            }
            else
            {
                for (var n = 0; n < count; n++)
                {
                    archive.SkipString();
                }
            }
        }

        protected void readBinaryEmbeddedData(ArkArchive archive, bool embeddedData = true)
        {
            var count = archive.GetInt();
            if (embeddedData)
            {
                EmbeddedData.Clear();
                for (var n = 0; n < count; n++)
                {
                    EmbeddedData.Add(new EmbeddedData(archive));
                }
            }
            else
            {
                for (int n = 0; n < count; n++)
                {
                    Types.EmbeddedData.Skip(archive);
                }
            }
        }

        protected void readBinaryObjects(ArkArchive archive, bool gameObjects = true)
        {
            if (gameObjects)
            {
                var count = archive.GetInt();

                Objects.Clear();
                for (var n = 0; n < count; n++)
                {
                    var gameObject = new GameObject(archive, _arkNameCache);
                    gameObject.ObjectId = n;
                    Objects.Add(gameObject);
                }
            }
        }

        protected void readBinaryObjectProperties(ArkArchive archive, MemoryMappedFile mmf, Func<GameObject, bool> objectFilter = null, bool gameObjects = true, bool gameObjectProperties = true)
        {
            if (gameObjects && gameObjectProperties)
            {
                var success = true;
                try
                {
                    var cb = new ConcurrentBag<ArkArchive>();
                    cb.Add(archive);

                    var indices = Enumerable.Range(0, Objects.Count);
                    if (objectFilter != null) indices = indices.Where(x => objectFilter(Objects[x]));
                    Parallel.ForEach(indices, new ParallelOptions { MaxDegreeOfParallelism = 6 },
                        () => { ArkArchive arch = null; var va = cb.TryTake(out arch) ? null : mmf.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read); return new { va = va, a = arch ?? new ArkArchive(archive, va) }; },
                        (item, loopState, taskLocals) =>
                        {
                            readBinaryObjectPropertiesImpl(item, taskLocals.a);
                            return taskLocals;
                        },
                        (taskLocals) => { if (taskLocals?.va != null) taskLocals.va.Dispose(); }
                        );
                }
                catch (AggregateException ae)
                {
                    success = false;
                    ae.Handle(ex => {
                        if (ex is IOException 
                            && ex.Message.IndexOf("Not enough storage is available to process this command.", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            _logger.Error($"Not enough memory available to load properties with this degree of parallelism.");
                            return true;
                        }

                        return false;
                    });
                }

                if (!success) throw new ApplicationException("Failed to load properties for all gameobjects.");
            }
        }

        protected void readBinaryObjectPropertiesImpl(int n, ArkArchive archive)
        {
            Objects[n].loadProperties(archive, (n < Objects.Count - 1) ? Objects[n + 1] : null, propertiesBlockOffset);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _va?.Dispose();
                    _mmf?.Dispose();
                    _va = null;
                    _mmf = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
