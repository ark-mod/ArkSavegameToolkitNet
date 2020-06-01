using System;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.Cache
{
    public interface IArkUuidCache
    {
        Guid Create((long uuidMostSig, long uuidLeastSig) key, Func<Guid> uuid);
    }

    class ArkUuidCache : IArkUuidCache
    {
        private IDictionary<(long, long), Guid> _instances;

        internal int _numAdded;
        internal int _numDuplicates;

        public ArkUuidCache()
        {
            _instances = new Dictionary<(long uuidMostSig, long uuidLeastSig), Guid>();
        }

        public Guid Create((long, long) key, Func<Guid> uuidProvider)
        {
            if (_instances.TryGetValue(key, out var result))
            {
                _numDuplicates++;
                return result;
            }

            var uuid = uuidProvider.Invoke();
            _instances.Add(key, uuid);
            _numAdded++;
            return uuid;
        }
    }
}
