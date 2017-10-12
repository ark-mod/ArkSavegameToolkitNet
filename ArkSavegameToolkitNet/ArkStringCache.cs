using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class ArkStringCache
    {
        private ConcurrentDictionary<string, string> _items;

        public ArkStringCache()
        {
            _items = new ConcurrentDictionary<string, string>();
        }

        public string Add(string value)
        {
            if (value == null) return null;

            try
            {
                string result;
                if (!_items.TryGetValue(value, out result))
                {
                    if (!_items.TryAdd(value, value))
                    {
                        _items.TryGetValue(value, out result);
                        return result;
                    }
                    return value;
                }
                return result;
            }
            catch { }

            return value;
        }
    }
}
