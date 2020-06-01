using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class PropertyListStructReader
    {
        public static PropertyListStructProperty Get(ArchiveReader ar, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            var result = new PropertyListStructProperty { properties = new Dictionary<ArkName, PropertyBase>() };


#if DEBUG
            var prevOffset = ar._position;
#endif
            foreach (var p in PropertyReader.GetMany(ar, exclusivePropertyNameTreeChildNode))
            {
                result.properties.Add(p.name, p);
#if DEBUG
                prevOffset = ar._position;
#endif
            }

            return result;
        }
    }
}
