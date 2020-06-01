using ArkSavegameToolkitNet.DataTypes;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ArkSavegameToolkitNet.Cache
{
    public interface IArkStringCache : IDisposable
    {
        string Add(ReadOnlySpan<char> value);
        ArkName AddName(ReadOnlySpan<char> token);
        ArkName AddName(string token);
        ArkName AddName(string name, int index);
    }

    class ArkStringCache : IArkStringCache
    {
        private StringCacheDictionary<ArkName> _items;

        internal int _numNull;
        internal int _numAdded;
        internal int _numDuplicates;

        public ArkStringCache()
        {
            _items = new StringCacheDictionary<ArkName>();
        }

        internal char[] _stringBuffer = ArrayPool<char>.Shared.Rent(100);

        private ArkName CreateName(string token)
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
                name = AddWithoutIndex(token.AsSpan().Slice(0, i));
            }

            return new ArkName(name, instance, token);
        }

        private ArkName CreateName(string name, int instance, string token = null)
        {
            if (token == null)
            {
                if (instance == 0) token = name;
                else
                {
                    token = Add(GetTokenAsSpan(name, instance));
                }
            }
            return new ArkName(name, instance, token);
        }

        public string Add(ReadOnlySpan<char> value)
        {
            if (value == null)
            {
                _numNull++;
                return null;
            }

            if (_items.TryGetValue(value, out var result))
            {
                _numDuplicates++;
                return result.Token;
            }

            var i = CreateName(value.ToString());
            _items.Add(i.Token, i);
            _numAdded++;
            return i.Token;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string AddWithoutIndex(ReadOnlySpan<char> value)
        {
            if (_items.TryGetValue(value, out var result))
            {
                return result.Token;
            }

            var token = value.ToString();
            var i = CreateName(token, 0, token);
            _items.Add(i.Token, i);
            return i.Token;
        }

        public ArkName AddName(ReadOnlySpan<char> token)
        {
            if (_items.TryGetValue(token, out var result))
            {
                return result;
            }

            var i = CreateName(token.ToString());
            _items.Add(i.Token, i);
            return i;
        }

        public ArkName AddName(string token)
        {
            if (_items.TryGetValue(token, out var result))
            {
                return result;
            }

            var i = CreateName(token);
            _items.Add(token, i);
            return i;
        }

        public ArkName AddName(string name, int instance)
        {
            if (instance == 0)
            {
                if (_items.TryGetValue(name, out var result))
                {
                    return result;
                }

                var an = new ArkName(name, instance, name);
                _items.Add(name, an);
                return an;
            }
            else
            {
                var stoken = GetTokenAsSpan(name, instance);
                if (_items.TryGetValue(stoken, out var result))
                {
                    return result;
                }

                var token = stoken.ToString();
                var an = new ArkName(name, instance, token);
                _items.Add(token, an);
                return an;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<char> GetTokenAsSpan(string name, int instance)
        {
            var len = name.Length;
            if (len + 11 > _stringBuffer.Length) // 11: int.MaxValue.ToString().Length + 1 ('_')
            {
                ArrayPool<char>.Shared.Return(_stringBuffer);
                _stringBuffer = ArrayPool<char>.Shared.Rent(11 + name.Length * 2);
            }

            // copy token to end of string buffer
            var n = instance;
            var sbl = _stringBuffer.Length - 1;
            while (n > 0)
            {
                _stringBuffer[sbl--] = (char)(n % 10 + 48);
                n /= 10;
            }
            _stringBuffer[sbl--] = '_';
            for (var i = len - 1; i >= 0; i--) _stringBuffer[sbl--] = name[i];

            return _stringBuffer.AsSpan().Slice(sbl + 1);
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
                    ArrayPool<char>.Shared.Return(_stringBuffer);
                    _stringBuffer = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ArkStringCache()
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
