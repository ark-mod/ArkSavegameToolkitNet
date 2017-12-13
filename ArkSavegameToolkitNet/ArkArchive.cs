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
        private ArkStringCache _arkStringCache;
        private ArkNameTree _exclusivePropertyNameTree;
        private int _nameTableOffset = 1;   // 0 for hibernation object archives
        private bool _isInstanceInTable ;   // true only for hibernation object archives
       // private bool _isSlice;              // true if this a subarchive
        public ArkArchive(MemoryMappedViewAccessor va, long size, ArkNameCache arkNameCache = null, ArkStringCache arkStringCache = null, ArkNameTree exclusivePropertyNameTree = null)
        {
            _va = va;
            _size = size;
            _arkNameCache = arkNameCache ?? new ArkNameCache();
            _arkStringCache = arkStringCache ?? new ArkStringCache();
            _exclusivePropertyNameTree = exclusivePropertyNameTree;
        }

        public ArkArchive(ArkArchive toClone, MemoryMappedViewAccessor va)
        {
            _va = va;
            _size = toClone._size;
            _nameTable = toClone._nameTable;
            _arkNameCache = toClone._arkNameCache;
            _arkStringCache = toClone._arkStringCache;
            _exclusivePropertyNameTree = toClone._exclusivePropertyNameTree;
        }

       /// <summary>Creates a subArchive of an existing archive (slice)</summary>
       /// <param name="toClone">the archive to clone</param>
       /// <param name="offset">position of the sub archive</param>
       /// <param name="size">size of the sub archive</param>
        public ArkArchive Slice ( long offset,int size)
        {
            ArkArchive sliced = new ArkArchive(this, this._va);
            _nameTable = null;
            sliced._size = size+1;
            sliced._offset = offset;
            //sliced._isSlice = true;
            return sliced;
        }

        public ArkNameTree ExclusivePropertyNameTree => _exclusivePropertyNameTree;

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
                    _logger.Error($"Attempted to set position outside stream bounds (offset: {value:X}, size: {_size:X})");
                    throw new OverflowException();
                }

                _position = value;
            }
        }
        private long _offset ;
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

        /// <summary>Set the nameTable and it's properties</summary>
        /// <param name="NameTable">the nameTable to set</param>
        /// <param name="nameTableOffset">the offset where indices start</param>
        /// <param name="isInstanceInTable">true to get arkName from nameTable</param>
        public void SetNameTable(List<string> nameTable, int nameTableOffset, bool isInstanceInTable)
        {
            this.NameTable = nameTable;
            this._nameTableOffset = nameTableOffset;
            this._isInstanceInTable = isInstanceInTable;
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
                // some tables start with an offset of 1
                var internalID = id - _nameTableOffset;
                if (internalID < 0 || id >= _nameTable.Count)
                {
                    _logger.Warn($"Found invalid nametable index {id} at {position:X}");
                    return null;
                }

                var nameString = _nameTable[internalID];
                // only true for hibernation entries
                if (_isInstanceInTable) return _arkNameCache.Create(nameString);
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
                var internalID = id - _nameTableOffset;
                if (internalID < 0 || internalID >= _nameTable.Count) throw new IndexOutOfRangeException($"invalid name index: {id} at {_position - 4}");

               var nameString = _nameTable[internalID];
                // only true for hibernation entries
                if (_isInstanceInTable) return _arkNameCache.Create(nameString);
                var nameIndex = GetInt();

                return _arkNameCache.Create(nameString, nameIndex);
            }
        }

        public void SkipName()
        {
            if (_nameTable == null)
            {
                SkipString();
            }
            else
            {
                if (_isInstanceInTable)
                    _position += 4;
                else
                    _position += 8;
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
                var count = _va.ReadArray(_offset + _position + 4, buffer, 0, absSize);
                var result = new string(buffer, 0, absSize - 1);

                //return result;
                return _arkStringCache.Add(result);
            }
            else
            {
                var buffer = new byte[absSize];
                var count = _va.ReadArray(_offset+_position + 4, buffer, 0, absSize);

                //return Encoding.ASCII.GetString(buffer, 0, absSize - 1);
                return _arkStringCache.Add(Encoding.ASCII.GetString(buffer, 0, absSize - 1));
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
                var count = _va.ReadArray(_offset + _position, buffer, 0, absSize);
                _position += absSize * 2;
                var result = new string(buffer, 0, absSize - 1);

                //return result;
                return _arkStringCache.Add(result);
            }
            else
            {
                var buffer = new byte[absSize];
                var count = _va.ReadArray(_offset + _position, buffer, 0, absSize);
                _position += absSize;

                //return Encoding.ASCII.GetString(buffer, 0, absSize - 1);
                return _arkStringCache.Add(Encoding.ASCII.GetString(buffer, 0, absSize - 1));
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

            var value = _va.ReadInt32(_offset + _position);

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

            var value = _va.ReadInt32(_offset + _position);
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

            var value = _va.ReadArray(_offset + _position, buffer, 0, length);
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

            var value = _va.ReadSByte(_offset + _position);
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

            var value = _va.ReadInt64(_offset + _position);
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

            var value = _va.ReadInt16(_offset + _position);
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

            var value = _va.ReadDouble(_offset + _position);
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

            var value = _va.ReadSingle(_offset + _position);
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
