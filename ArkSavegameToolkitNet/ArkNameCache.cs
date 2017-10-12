using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class ArkNameCache
    {
        private ConcurrentDictionary<string, ArkName> _instances = new ConcurrentDictionary<string, ArkName>();

        public ArkName Create(string token)
        {
            return _instances.GetOrAdd(token, s => new ArkName(s));
        }

        public ArkName Create(string name, int index)
        {
            var token = index == 0 ? name : $"{name}_{index}";
            return _instances.GetOrAdd(token, s => new ArkName(name, index, token));
        }
    }
}
