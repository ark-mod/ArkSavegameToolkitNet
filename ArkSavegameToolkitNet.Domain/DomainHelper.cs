using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public static class DomainHelper
    {
        /// <summary>
        /// Write property data structure information for a collection of GameObjects to specified file path (use default grouping on classname).
        /// </summary>
        public static void WriteDataStructure(IEnumerable<IGameObject> gameObjects, string filePath)
        {
            WriteDataStructure(gameObjects.GroupBy(x => x.ClassName.Token), filePath);
        }

        /// <summary>
        /// Write property data structure information for a collection of GameObjects to specified file path.
        /// </summary>
        public static void WriteDataStructure(IEnumerable<IGrouping<string, IGameObject>> gameObjectGroups, string filePath)
        {
            //todo: refactor this code to make it less hackish
            Func<Type, string> getTypeName = null;
            getTypeName = new Func<Type, string>(t =>
            {
                var nt = Nullable.GetUnderlyingType(t);
                if (nt != null) return $"{getTypeName(nt)}?";
                if (t.IsGenericType) return $"{t.Name.Remove(t.Name.IndexOf('`'))}<{string.Join(",", t.GetGenericArguments().Select(at => getTypeName(at)))}>";
                if (t.IsArray) return $"{getTypeName(t.GetElementType())}[{new string(',', t.GetArrayRank() - 1)}]";
                return t.Name;
            });

            Func<int?, IEnumerable<IProperty>, string, List<string>, List<string>> recursivePropertyList = null;
            recursivePropertyList = new Func<int?, IEnumerable<IProperty>, string, List<string>, List<string>>((gameObjectCount, props, path, list) =>
            {
                foreach (var grp in props.GroupBy(x => ArkName.Create(x.Name.Name, x.Index)))
                {
                    var prop = path == null ? grp.Key.Token : $"{path}->{grp.Key}";
                    var proparrstruct = grp.OfType<PropertyArray>().Where(x => (x.Value as ArkArrayStruct)?.OfType<StructPropertyList>().Any() == true);
                    var proplist = grp.OfType<PropertyStruct>().Where(x => x.Value is StructPropertyList);
                    var optional = gameObjectCount != null && (gameObjectCount.Value > grp.Count() || gameObjectCount < 0);

                    if (proplist.Any()) recursivePropertyList(optional ? -1 : (int?)null, proplist.SelectMany(x => (x.Value as StructPropertyList).Properties.Values), prop, list);
                    else if(proparrstruct.Any()) recursivePropertyList(optional ? -1 : (int?)null, proparrstruct.SelectMany(x => (x.Value as ArkArrayStruct).OfType<StructPropertyList>().SelectMany(y => y.Properties.Values)), prop, list);
                    else list.Add($"{prop} ({getTypeName(((dynamic)grp.First()).Value.GetType())}){(optional ? " [*]" : "")}");
                }
                return list;
            });
            var classes = gameObjectGroups
                .Select(x =>
                {
                    var c = x.Count();
                    return new
                    {
                        @class = x.Key,
                        count = c,
                        props = recursivePropertyList(c, x.SelectMany(y => y.Properties.Values), null, new List<string>()).OrderBy(y => y).ToArray()
                    };
                })
                .OrderBy(x => x.@class).Distinct().ToList();

            using (var sw = new StreamWriter(filePath, false))
            {
                var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(sw, classes);
            }
        }

        public static void ToJson(string filepath, object value, bool ignoreDefaultValues = false, bool ignoreNullValues = false, bool orderProperties = false)
        {
            using (var sw = new StreamWriter(filepath, false))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DefaultValueHandling = ignoreDefaultValues ? DefaultValueHandling.Ignore : DefaultValueHandling.Include,
                    NullValueHandling = ignoreNullValues ? NullValueHandling.Ignore : NullValueHandling.Include,
                    ContractResolver = orderProperties ? new OrderedContractResolver() : null
                };
                serializer.Serialize(sw, value);
            }
        }

        private class OrderedContractResolver : DefaultContractResolver
        {
            protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
            }
        }

        //private class AbbreviateNamingStrategy : NamingStrategy
        //{
        //    public AbbreviateNamingStrategy()
        //    {
        //        ProcessDictionaryKeys = false;
        //        OverrideSpecifiedNames = false;
        //        ProcessExtensionDataNames = false;
        //    }

        //    protected override string ResolvePropertyName(string name)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
