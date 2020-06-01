using ArkSavegameToolkitNet.Cache;
using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DeveloperTools;
using System;

namespace ArkSavegameToolkitNet.SaveGameReaders
{
    class ArkSaveGameReaderState : IDisposable
    {
        internal ArkToolkitLoader _loader;
        internal IDataConsumer _dataConsumer;
        internal ArkNameTree _exclusivePropertyNameTree;
        internal StructureLog _structureLog;
        internal IArkStringCache _stringCache;
        internal IArkUuidCache _uuidCache;

        internal DevFlags _devFlags;

        internal ArkSaveGameReaderState(ArkToolkitLoader loader)
        {
            _loader = loader;
            _dataConsumer = loader.DataConsumerProvider.Invoke();

            _exclusivePropertyNameTree = loader.ExclusivePropertyNameTree;
            _structureLog = loader.EnableStructureLog ? loader.StructureLogProvider.Invoke() : null;
            _stringCache = loader.StringCacheProvider.Invoke();
            _uuidCache = loader.UuidCacheProvider.Invoke();
            _devFlags = loader.DevFlags;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _structureLog?.Dispose();
                    _structureLog = null;

                    _stringCache?.Dispose();
                    _stringCache = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SaveReaderState()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
