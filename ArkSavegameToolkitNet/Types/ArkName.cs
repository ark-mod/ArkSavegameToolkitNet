using ArkSavegameToolkitNet.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(ArkNameJsonConverter))]
    public sealed class ArkName : IEquatable<ArkName>, IComparable<ArkName>
    {
        public static readonly ArkName NONE_NAME;
        public static readonly ArkName EMPTY_NAME;

        private static readonly Regex NAME_INDEX_PATTERN;

        private readonly string _name;
        private readonly int _index;
        private readonly string _token;

        public string Name => _name;
        public int Index => _index;

        [JsonProperty]
        public string Token => _token;

        static ArkName()
        {
            NAME_INDEX_PATTERN = new Regex("^(.*)_([0-9]+)$");
            NONE_NAME = new ArkName("None");
            EMPTY_NAME = new ArkName("");
        }

        public static ArkName Create(string token)
        {
            return new ArkName(token);
        }

        public static ArkName Create(string name, int index)
        {
            return new ArkName(name, index);
        }

        public ArkName(string token)
        {
            var m = NAME_INDEX_PATTERN.Match(token);
            if (m.Success)
            {
                _name = m.Groups[1].Value;
                _index = int.Parse(m.Groups[2].Value);
            }
            else
            {
                _name = token;
                _index = 0;
            }
            _token = token;
        }

        public ArkName(string name, int index, string token = null)
        {
            _name = name;
            _index = index;
            _token = token ?? (index == 0 ? name : $"{name}_{index}");
        }

        public override string ToString()
        {
            return _token;
        }

        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }

        public bool Equals(ArkName other)
        {
            return _token.Equals(other._token);
        }

        public int CompareTo(ArkName other)
        {
            return _token.CompareTo(other._token);
        }
    }
}