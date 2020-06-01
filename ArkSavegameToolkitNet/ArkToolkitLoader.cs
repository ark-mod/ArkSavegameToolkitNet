using ArkSavegameToolkitNet.Cache;
using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DeveloperTools;
using ArkSavegameToolkitNet.SaveGameReaders;
using System;
using System.IO;

namespace ArkSavegameToolkitNet
{
    public class ArkToolkitLoader
    {
        // settings
        public bool EnableStructureLog { get => _enableStructureLog; set => _enableStructureLog = value; }
        internal bool _enableStructureLog;

        public DevFlags DevFlags { get => _devFlags; set => _devFlags = value; }
        internal DevFlags _devFlags;

        public int BufferSize { get => _bufferSize; set => _bufferSize = value; }
        internal int _bufferSize;

        public ArkNameTree ExclusivePropertyNameTree { get => _exclusivePropertyNameTree; set => _exclusivePropertyNameTree = value; }
        internal ArkNameTree _exclusivePropertyNameTree;

        public Func<IDataConsumer> DataConsumerProvider { get => _dataConsumerProvider; set => _dataConsumerProvider = value; }
        internal Func<IDataConsumer> _dataConsumerProvider;

        internal Func<StructureLog> StructureLogProvider { get => _structureLogProvider; set => _structureLogProvider = value; }
        internal Func<StructureLog> _structureLogProvider;

        public Func<IArkStringCache> StringCacheProvider { get => _stringCacheProvider; set => _stringCacheProvider = value; }
        internal Func<IArkStringCache> _stringCacheProvider;

        public Func<IArkUuidCache> UuidCacheProvider { get => _uuidCacheProvider; set => _uuidCacheProvider = value; }
        internal Func<IArkUuidCache> _uuidCacheProvider;

        public ArkToolkitLoader()
        {
            _enableStructureLog = ArkToolkitLoaderSettings.DefaultEnableStructureLog;
            _devFlags = ArkToolkitLoaderSettings.DefaultDevFlags;
            _bufferSize = ArkToolkitLoaderSettings.DefaultBufferSize;
            _exclusivePropertyNameTree = ArkToolkitLoaderSettings.DefaultExclusivePropertyNameTree;
            _dataConsumerProvider = ArkToolkitLoaderSettings.DefaultDataConsumerProvider;
            _structureLogProvider = ArkToolkitLoaderSettings.DefaultStructureLogProvider;
            _stringCacheProvider = ArkToolkitLoaderSettings.DefaultStringCacheProvider;
            _uuidCacheProvider = ArkToolkitLoaderSettings.DefaultUuidCacheProvider;
        }

        public static ArkToolkitLoader CreateDefault()
        {
            var defaultSettings = ArkToolkit.DefaultSettings?.Invoke();
            return Create(defaultSettings);
        }

        public static ArkToolkitLoader Create() => new ArkToolkitLoader();

        public static ArkToolkitLoader Create(ArkToolkitLoaderSettings settings)
        {
            var reader = Create();
            if (settings != null) ApplySettings(reader, settings);

            return reader;
        }

        private static void ApplySettings(ArkToolkitLoader loader, ArkToolkitLoaderSettings settings)
        {
            if (settings._enableStructureLog != null) loader.EnableStructureLog = settings.EnableStructureLog;
            if (settings._devFlags != null) loader.DevFlags = settings.DevFlags;
            if (settings._bufferSize != null) loader.BufferSize = settings.BufferSize;
            if (settings._exclusivePropertyNameTree != null) loader.ExclusivePropertyNameTree = settings.ExclusivePropertyNameTree;
            if (settings._structureLogProvider != null) loader.StructureLogProvider = settings.StructureLogProvider;
            if (settings._dataConsumerProvider != null) loader.DataConsumerProvider = settings.DataConsumerProvider;
            if (settings._stringCacheProvider != null) loader.StringCacheProvider = settings.StringCacheProvider;
            if (settings._uuidCacheProvider != null) loader.UuidCacheProvider = settings.UuidCacheProvider;
        }


        public ArkSaveData LoadArkSave(string fileName)
        {
            return ArkSaveGameReader.Load(this, fileName);
        }

        public ArkSaveData LoadArkSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkSaveGameReader.Load(this, stream, madeUpFileName, savedAt);
        }

        public ArkSaveData LoadArkSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkSaveGameReader.Load(this, data, madeUpFileName, savedAt);
        }


        public ProfileSaveGameData LoadProfileSave(string fileName)
        {
            return ProfileSaveGameReader.Load(this, fileName);
        }

        public ProfileSaveGameData LoadProfileSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ProfileSaveGameReader.Load(this, stream, madeUpFileName, savedAt);
        }

        public ProfileSaveGameData LoadProfileSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ProfileSaveGameReader.Load(this, data, madeUpFileName, savedAt);
        }


        public LocalProfileSaveGameData LoadLocalProfileSave(string fileName)
        {
            return LocalProfileSaveGameReader.Load(this, fileName);
        }

        public LocalProfileSaveGameData LoadLocalProfileSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return LocalProfileSaveGameReader.Load(this, stream, madeUpFileName, savedAt);
        }

        public LocalProfileSaveGameData LoadLocalProfileSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return LocalProfileSaveGameReader.Load(this, data, madeUpFileName, savedAt);
        }


        public TribeSaveGameData LoadTribeSave(string fileName)
        {
            return TribeSaveGameReader.Load(this, fileName);
        }

        public TribeSaveGameData LoadTribeSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return TribeSaveGameReader.Load(this, stream, madeUpFileName, savedAt);
        }

        public TribeSaveGameData LoadTribeSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return TribeSaveGameReader.Load(this, data, madeUpFileName, savedAt);
        }


        public ClusterSaveGameData LoadClusterSave(string fileName)
        {
            return ClusterSaveGameReader.Load(this, fileName);
        }

        public ClusterSaveGameData LoadClusterSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ClusterSaveGameReader.Load(this, stream, madeUpFileName, savedAt);
        }

        public ClusterSaveGameData LoadClusterSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ClusterSaveGameReader.Load(this, data, madeUpFileName, savedAt);
        }


        public ClusterDinoData LoadClusterDino(byte[] data, float version)
        {
            return ClusterDinoReader.Load(this, data, version);
        }


        public FrozenDinoData LoadFrozenDino(byte[] data)
        {
            return FrozenDinoReader.Load(this, data);
        }
    }
}
