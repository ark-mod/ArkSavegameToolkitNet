using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Domain.Utils.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public static class DomainHelper
    {
        /// <summary>
        /// Write property data structure information for a collection of GameObjects to specified file path (use default grouping on classname).
        /// </summary>
        public static void WriteDataStructure(IEnumerable<GameObject> gameObjects, string filePath)
        {
            WriteDataStructure(gameObjects.GroupBy(x => x.className.Token), filePath);
        }

        /// <summary>
        /// Write property data structure information for a collection of GameObjects to specified file path.
        /// </summary>
        public static void WriteDataStructure(IEnumerable<IGrouping<string, GameObject>> gameObjectGroups, string filePath)
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

            Func<int?, IEnumerable<PropertyBase>, string, List<string>, List<string>> recursivePropertyList = null;
            recursivePropertyList = new Func<int?, IEnumerable<PropertyBase>, string, List<string>, List<string>>((gameObjectCount, props, path, list) =>
            {
                foreach (var grp in props.GroupBy(x => ArkName.Create(x.name.Name, x.index)))
                {
                    var prop = path == null ? grp.Key.Token : $"{path}->{grp.Key}";
                    var proparrstruct = grp.OfType<StructArrayProperty>().Where(x => x.values.OfType<PropertyListStructProperty>().Any() == true);
                    var proplist = grp.OfType<PropertyListStructProperty>();
                    var optional = gameObjectCount != null && (gameObjectCount.Value > grp.Count() || gameObjectCount < 0);

                    if (proplist.Any()) recursivePropertyList(optional ? -1 : (int?)null, proplist.SelectMany(x => x.Properties.Values), prop, list);
                    else if(proparrstruct.Any()) recursivePropertyList(optional ? -1 : (int?)null, proparrstruct.SelectMany(x => x.values.OfType<PropertyListStructProperty>().SelectMany(y => y.Properties.Values)), prop, list);
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
                    ContractResolver = new OrderedContractResolver(orderProperties)
                };
                serializer.Serialize(sw, value);
            }
        }

        private class OrderedContractResolver : DefaultContractResolver
        {
            private bool _orderProperties;

            public OrderedContractResolver(bool orderProperties = false)
            {
                _orderProperties = orderProperties;
            }

            protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
            {
                if (_orderProperties) return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
                else return base.CreateProperties(type, memberSerialization);
            }

            protected override JsonContract CreateContract(Type objectType)
            {
                JsonContract contract = base.CreateContract(objectType);

                // this will only be called once and then cached
                if (objectType == typeof(float) || objectType == typeof(float?)
                    || objectType == typeof(double) || objectType == typeof(double?)
                    || objectType == typeof(decimal) || objectType == typeof(decimal?))
                {
                    contract.Converter = new DecimalConverter();
                }
                else if (objectType == typeof(byte[]))
                {
                    contract.Converter = new ByteArrayConverter();
                }

                return contract;
            }
        }

        public class DecimalConverter : JsonConverter
        {
            public override void WriteJson(
                JsonWriter writer,
                object value,
                JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }

                decimal f = Convert.ToDecimal(value);

                writer.WriteValue(Math.Round(f, 2));
            }

            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Float) return Convert.ToSingle(reader.Value);
                else
                {
                    throw new Exception(
                        string.Format(
                            "Unexpected token parsing float, got {0}.",
                            reader.TokenType));
                }
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(float) || objectType == typeof(float?)
                    || objectType == typeof(double) || objectType == typeof(double?)
                    || objectType == typeof(decimal) || objectType == typeof(decimal?);
            }
        }
    }
}
