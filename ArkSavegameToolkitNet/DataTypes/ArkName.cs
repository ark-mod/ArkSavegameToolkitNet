using System;
using System.Diagnostics.CodeAnalysis;

namespace ArkSavegameToolkitNet.DataTypes
{
    public sealed class ArkName : IEquatable<ArkName>, IComparable<ArkName>, IEquatable<string>, IComparable<string>
    {
        public static readonly ArkName Empty = ArkName.Create("");
        public static readonly ArkName None = ArkName.Create("None");

        public readonly string Name;
        public readonly int Instance;
        public readonly string Token;

        public ArkName(string name, int instance, string token)
        {
            Name = name;
            Instance = instance;
            Token = token;
        }

        public static ArkName Create(string token)
        {
            if (token == null) return null;

            var instance = 0;
            var i = token.Length - 1;
            var r = 1;
            while (i >= 0 && (token[i] >= '0' && token[i] <= '9'))
            {
                instance += (token[i] - '0') * r;
                r *= 10;
                i--;
            }

            string name;
            if (i < 0 || token[i] != '_') name = token;
            else
            {
                name = token.Substring(0, i);
            }

            return new ArkName(name, instance, token);
        }

        public static ArkName Create(string name, int instance, string token = null)
        {
            return new ArkName(name, instance, token ?? (instance == 0 ? name : string.Join("_", name, instance))); //$"{name}_{instance}"
        }

        public override string ToString() => Token;
        public override int GetHashCode() => Token.GetHashCode();

        public bool Equals(ArkName other) => Token.Equals(other.Token);
        public int CompareTo(ArkName other) => Token.CompareTo(other.Token);
        public bool Equals([AllowNull] string other) => Token.Equals(other);
        public int CompareTo([AllowNull] string other) => Token.CompareTo(other);
    }
}
