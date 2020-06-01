using System.Collections.Generic;
using System.Linq;

namespace ArkSavegameToolkitNet.DataTypes
{
    public class ArkNameTree : Dictionary<ArkName, ArkNameTree>
    {
        public static ArkNameTree Merge(params ArkNameTree[] items)
        {
            if (items.All(x => x == null)) return null;

            var result = new ArkNameTree();

            foreach (var l in items.Where(x => x != null).SelectMany(x => x).ToLookup(pair => pair.Key, pair => pair.Value))
            {
                result.Add(l.Key, Merge(l.ToArray()));
            }

            return result;
        }
    }
}
