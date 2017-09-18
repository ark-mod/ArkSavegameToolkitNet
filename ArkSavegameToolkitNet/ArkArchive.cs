using ArkSavegameToolkitNet.Types;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class ArkArchive
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ArkArchive));

        private long _position;
        private long _size;
        private MemoryMappedViewAccessor _va;
        private IReadOnlyList<string> _nameTable;
        private ArkNameCache _arkNameCache;

        public ArkArchive(MemoryMappedViewAccessor va, long size, ArkNameCache arkNameCache = null)
        {
            _va = va;
            _size = size;
            _arkNameCache = arkNameCache ?? new ArkNameCache();
        }

        public ArkArchive(ArkArchive toClone, MemoryMappedViewAccessor va)
        {
            _va = va;
            _size = toClone._size;
            _nameTable = toClone._nameTable;
            _arkNameCache = toClone._arkNameCache;
        }

        public long Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (value < 0 || value >= _size)
                {
                    _logger.Error($"Attempted to set position outside steam bounds (offset: {value:X}, size: {_size:X})");
                    throw new OverflowException();
                }

                _position = value;
            }
        }

        public long Size => _size;

        public IReadOnlyList<string> NameTable
        {
            get
            {
                return _nameTable;
            }
            set
            {
                if (value != null) _nameTable = new List<string>(value) as IReadOnlyList<string>;
                else _nameTable = null;
            }
        }

        public ArkName[] GetNames(long position)
        {
            var count = GetInt(position);
            var names = new ArkName[count];
            var oldposition = _position;
            _position = position + 4;
            for (var i = 0; i < count; i++) names[i] = GetName();
            _position = oldposition;

            return names;
        }

        public ArkName GetName(long position)
        {
            if (_nameTable == null)
            {
                var nameAsString = GetString(position);
                return _arkNameCache.Create(nameAsString);
            }
            else
            {
                var id = GetInt(position);

                if (id < 1 || id > _nameTable.Count)
                {
                    _logger.Warn($"Found invalid nametable index {id} at {position:X}");
                    return null;
                }

                var nameString = _nameTable[id - 1];
                var nameIndex = GetInt(position + 4);

                return _arkNameCache.Create(nameString, nameIndex);
            }
        }

        public ArkName GetName()
        {
            if (_nameTable == null)
            {
                var nameAsString = GetString();
                return _arkNameCache.Create(nameAsString);
            }
            else
            {
                var id = GetInt();

                if (id < 1 || id > _nameTable.Count)
                {
                    _logger.Warn($"Found invalid nametable index {id} at {_position - 4:X}");
                    return null;
                }

                var nameString = _nameTable[id - 1];
                var nameIndex = GetInt();

                return _arkNameCache.Create(nameString, nameIndex);
            }
        }

        public int GetNameLength(long position)
        {
            if (_nameTable == null)
            {
                var size = GetInt(position);
                var multibyte = size < 0;
                var absSize = Math.Abs(size);
                var readSize = multibyte ? absSize * 2 : absSize;
                return 4 + readSize;
            }
            else return 8;
        }

        public string GetString(long position)
        {
            var size = GetInt(position);
            if (size == 0) return string.Empty;

            var multibyte = size < 0;
            var absSize = Math.Abs(size);
            var readSize = multibyte ? absSize * 2 : absSize;

            if (readSize + position + 4 >= _size)
            {
                _logger.Error($"Trying to read {readSize} bytes at {position + 4:X} with just {_size - (position + 4)} bytes left");
                throw new OverflowException();
            }

            if (multibyte)
            {
                var buffer = new char[absSize];
                var count = _va.ReadArray(position + 4, buffer, 0, absSize);
                var result = new string(buffer, 0, absSize - 1);

                return result;
            }
            else
            {
                var buffer = new byte[absSize];
                var count = _va.ReadArray(_position + 4, buffer, 0, absSize);

                return Encoding.ASCII.GetString(buffer, 0, absSize - 1);
            }
        }

        public string GetString()
        {
            var size = GetInt();
            if (size == 0) return string.Empty;

            var multibyte = size < 0;
            var absSize = Math.Abs(size);
            var readSize = multibyte ? absSize * 2 : absSize;

            if (readSize + _position >= _size)
            {
                _logger.Error($"Trying to read {readSize} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            if (multibyte)
            {
                var buffer = new char[absSize];
                var count = _va.ReadArray(_position, buffer, 0, absSize);
                _position += absSize * 2;
                var result = new string(buffer, 0, absSize - 1);

                return result;
            }
            else
            {
                var buffer = new byte[absSize];
                var count = _va.ReadArray(_position, buffer, 0, absSize);
                _position += absSize;

                return Encoding.ASCII.GetString(buffer, 0, absSize - 1);
            }
        }

        public void SkipString()
        {
            var size = GetInt();

            var multibyte = size < 0;
            var absSize = Math.Abs(size);
            var readSize = multibyte ? absSize * 2 : absSize;

            if (absSize > 10000) _logger.Info($"Large String ({absSize}) at {_position - 4:X}");

            if (readSize + _position > _size)
            {
                _logger.Error($"Trying to skip {readSize} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            Position += readSize;
        }

        public int GetInt(long position)
        {
            const long size = 4;
            if (position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {position:X} with just {_size - position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadInt32(position);

            return value;
        }

        public int GetInt()
        {
            const long size = 4;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadInt32(_position);
            _position += size;

            return value;
        }

        public sbyte[] GetBytes(int length)
        {
            var buffer = new sbyte[length];

            if (_position + length > _size)
            {
                _logger.Error($"Trying to read {length} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadArray(_position, buffer, 0, length);
            _position += length;

            return buffer;
        }

        public sbyte GetByte()
        {
            const long size = 1;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadSByte(_position);
            _position += size;

            return value;
        }

        public long GetLong()
        {
            const long size = 8;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadInt64(_position);
            _position += size;

            return value;
        }

        public short GetShort()
        {
            const long size = 2;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadInt16(_position);
            _position += size;

            return value;
        }

        public double GetDouble()
        {
            const long size = 8;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadDouble(_position);
            _position += size;

            return value;
        }

        public float GetFloat()
        {
            const long size = 4;
            if (_position + size > _size)
            {
                _logger.Error($"Trying to read {size} bytes at {_position:X} with just {_size - _position} bytes left");
                throw new OverflowException();
            }

            var value = _va.ReadSingle(_position);
            _position += size;

            return value;
        }

        public bool GetBoolean()
        {
            var val = GetInt();
            if (val < 0 || val > 1) _logger.Warn($"Boolean at {_position:X} with value {val}, returning true.");

            return val != 0;
        }
    }
}
