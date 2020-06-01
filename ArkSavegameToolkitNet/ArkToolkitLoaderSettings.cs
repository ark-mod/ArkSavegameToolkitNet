using ArkSavegameToolkitNet.Cache;
using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DeveloperTools;
using System;

namespace ArkSavegameToolkitNet
{
    [Flags] public enum DevFlags { None, Testing }

    public class ArkToolkitLoaderSettings
    {
        internal const bool DefaultEnableStructureLog = false;
        internal const DevFlags DefaultDevFlags = DevFlags.None;
        internal const int DefaultBufferSize = 4096 * 100; // larger min buffer results in fewer seeks when reading game objects and properties concurrently from separate sections of the save
        internal const ArkNameTree DefaultExclusivePropertyNameTree = null;
        internal static readonly Func<IDataConsumer> DefaultDataConsumerProvider = () => new NullDataConsumer();
        internal static readonly Func<IArkStringCache> DefaultStringCacheProvider = () => new ArkStringCache();
        internal static readonly Func<IArkUuidCache> DefaultUuidCacheProvider = () => new ArkUuidCache();
        internal static readonly Func<StructureLog> DefaultStructureLogProvider = () => null;

        public bool EnableStructureLog { get => _enableStructureLog ?? DefaultEnableStructureLog; set => _enableStructureLog = value; }
        internal bool? _enableStructureLog;

        public DevFlags DevFlags { get => _devFlags ?? DefaultDevFlags; set => _devFlags = value; }
        internal DevFlags? _devFlags;

        public int BufferSize { get => _bufferSize ?? DefaultBufferSize; set => _bufferSize = value; }
        internal int? _bufferSize;

        public ArkNameTree ExclusivePropertyNameTree { get => _exclusivePropertyNameTree ?? DefaultExclusivePropertyNameTree; set => _exclusivePropertyNameTree = value; }
        internal ArkNameTree _exclusivePropertyNameTree;

        public Func<IDataConsumer> DataConsumerProvider { get => _dataConsumerProvider ?? DefaultDataConsumerProvider; set => _dataConsumerProvider = value; }
        internal Func<IDataConsumer> _dataConsumerProvider;

        public Func<IArkStringCache> StringCacheProvider { get => _stringCacheProvider ?? DefaultStringCacheProvider; set => _stringCacheProvider = value; }
        internal Func<IArkStringCache> _stringCacheProvider;

        public Func<IArkUuidCache> UuidCacheProvider { get => _uuidCacheProvider ?? DefaultUuidCacheProvider; set => _uuidCacheProvider = value; }
        internal Func<IArkUuidCache> _uuidCacheProvider;

        internal Func<StructureLog> StructureLogProvider { get => _structureLogProvider ?? DefaultStructureLogProvider; set => _structureLogProvider = value; }
        internal Func<StructureLog> _structureLogProvider;
    }
}
