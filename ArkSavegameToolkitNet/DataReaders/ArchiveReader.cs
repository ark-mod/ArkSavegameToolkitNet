using ArkSavegameToolkitNet.Cache;
using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DeveloperTools;
using ArkSavegameToolkitNet.SaveGameReaders;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ArkSavegameToolkitNet.DataReaders
{
    struct ArchiveReaderNameTableState
    {
        public IReadOnlyList<string> nameTable;
        public int nameOffset;
        public bool instanceInTable;
    }

    struct ArchiveReaderSectionState
    {
        public long position;
        public int size;
    }

    class ArchiveReaderBuffer
    {
        internal byte[] buffer;
        internal Memory<byte> mem;
        internal long position;
        internal int offset;
        internal int size;
    }

    class ArchiveReader : IDisposable
    {
        public ArkSaveGameReaderState SaveReaderState
        {
            get => _saveReaderState;
            set
            {
                _saveReaderState = value;
                _exclusivePropertyNameTree = _saveReaderState._exclusivePropertyNameTree;
                _structureLog = _saveReaderState._structureLog;
                _dataConsumer = _saveReaderState._dataConsumer;
                _stringCache = _saveReaderState._stringCache;
                _uuidCache = _saveReaderState._uuidCache;
                _devFlags = _saveReaderState._devFlags;
            }
        }

        private ArkSaveGameReaderState _saveReaderState;
        internal StructureLog _structureLog;
        internal IDataConsumer _dataConsumer;
        internal IArkStringCache _stringCache;
        internal IArkUuidCache _uuidCache;
        internal DevFlags _devFlags;

        internal char[] _stringBuffer;

        internal Stream _stream;
        internal bool _bufferWasRented;
        internal readonly int _bufferSize;
        internal ArkNameTree _exclusivePropertyNameTree;
        internal byte[] _buffer;
        internal Memory<byte> _mem;
        internal long _position;
        internal int _offset;
        internal int _size;
        internal ArchiveReaderNameTableState _nameTableState;
        internal ArchiveReaderSectionState _sectionState;

        private const bool _useNameTable = true;

        public ArchiveReader(int bufferSize)
        {
            _stringBuffer = ArrayPool<char>.Shared.Rent(100);

            _bufferWasRented = true;
            _bufferSize = bufferSize;
            _buffer = ArrayPool<byte>.Shared.Rent(_bufferSize); //new byte[_bufferSize];

            _mem = new Memory<byte>(_buffer);
            _nameTableState = new ArchiveReaderNameTableState();
        }

        public ArchiveReader(byte[] data)
        {
            _stringBuffer = ArrayPool<char>.Shared.Rent(100);

            _bufferSize = data.Length;
            _buffer = data;
            _size = data.Length;

            _mem = new Memory<byte>(_buffer);
            _nameTableState = new ArchiveReaderNameTableState();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Prepare(int size)
        {
            if (size > 10_485_760) throw new ApplicationException($"Attempting to read {size} bytes, which is more than the max allowed limit in this library.");
            if (size > _buffer.Length)
            {
                if (_stream == null) throw new IndexOutOfRangeException("Attempting to read outside data");

                _buffer = ArrayPool<byte>.Shared.Rent(_bufferSize * ((int)Math.Ceiling(size / (double)_bufferSize)));
                _mem = new Memory<byte>(_buffer);
            }

            if (_stream != null && _stream.Position != _position) _stream.Seek(_position, SeekOrigin.Begin);


            _size = _stream.Read(_mem.Span);
            _offset = 0;
            if (_size < size)
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetByte(out byte value, string varName)
        {
            const int byteSize = 1;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte>(_position, byteSize, varName);

            value = _mem.Span[_offset];
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetSByte(out sbyte value, string varName)
        {
            const int byteSize = 1;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<sbyte>(_position, byteSize, varName);

            value = (sbyte)_mem.Span[_offset];
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetBool(out bool value, string varName)
        {
            const int byteSize = 4;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<int>(_position, byteSize, varName);

            var tmp = BitConverter.ToInt32(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;

            value = tmp != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetInt(out int value, string varName)
        {
            const int byteSize = 4;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<int>(_position, byteSize, varName);

            value = MemoryMarshal.AsRef<int>(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUInt(out uint value, string varName)
        {
            const int byteSize = 4;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<uint>(_position, byteSize, varName);

            value = BitConverter.ToUInt32(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetShort(out short value, string varName)
        {
            const int byteSize = 2;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<short>(_position, byteSize, varName);

            value = BitConverter.ToInt16(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUShort(out ushort value, string varName)
        {
            const int byteSize = 2;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<ushort>(_position, byteSize, varName);

            value = BitConverter.ToUInt16(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetLong(out long value, string varName)
        {
            const int byteSize = 8;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<long>(_position, byteSize, varName);

            value = BitConverter.ToInt64(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetULong(out ulong value, string varName)
        {
            const int byteSize = 8;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<ulong>(_position, byteSize, varName);

            value = BitConverter.ToUInt64(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetFloat(out float value, string varName)
        {
            const int byteSize = 4;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<float>(_position, byteSize, varName);

            value = BitConverter.ToSingle(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetDouble(out double value, string varName)
        {
            const int byteSize = 8;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<double>(_position, byteSize, varName);

            value = MemoryMarshal.AsRef<double>(_mem.Span.Slice(_offset));
            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetArray(char[] buffer, int offset, int size, string varName)
        {
            var byteSize = size * 2;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte[]>(_position, byteSize, varName);

            var source = _mem.Span.Slice(_offset, byteSize);
            var n = 0;
            for (var i = 0; i < byteSize; i+=2)
                buffer[offset + n++] = (char)((char)source[i] | (char)source[i + 1] << 8);

            _offset += size * 2;
            _position += size * 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetArray(byte[] buffer, int offset, int size, string varName)
        {
            if ((_size == 0 || _size - _offset < size) && !Prepare(size)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte[]>(_position, size, varName);

            _mem.Span.Slice(_offset, size).CopyTo(new Span<byte>(buffer, offset, size));

            _offset += size;
            _position += size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUtf16Chars(Span<char> buffer, string varName)
        {
            var byteSize = buffer.Length * 2;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte[]>(_position, byteSize, varName);

            var source = _mem.Span.Slice(_offset, byteSize);

            for (int i = 0, n = 0; i < byteSize; i += 2, n++)
                buffer[n] = (char)((char)source[i] | (char)source[i + 1] << 8);

            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetAsciiChars(Span<char> buffer, string varName)
        {
            var byteSize = buffer.Length;
            if ((_size == 0 || _size - _offset < byteSize) && !Prepare(byteSize)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte[]>(_position, byteSize, varName);

            var source = _mem.Span.Slice(_offset, byteSize);
            for (var i = 0; i < byteSize; i++) buffer[i] = (char)source[i];

            _offset += byteSize;
            _position += byteSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetArray(Span<byte> buffer, string varName)
        {
            var size = buffer.Length;
            if ((_size == 0 || _size - _offset < size) && !Prepare(size)) throw new IndexOutOfRangeException();

            _structureLog?.Add<byte[]>(_position, size, varName);

            _mem.Span.Slice(_offset, size).CopyTo(buffer);

            _offset += size;
            _position += size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetString(string varName)
        {
            try
            {
                _structureLog?.PushStack("String", varName);

                GetInt(out var size, "size");
                if (size == 0) return null;
                if (size == 1)
                {
                    _position += 1;
                    return string.Empty;
                }
                if (size == -1)
                {
                    _position += 2;
                    return string.Empty;
                }

                var absSize = size < 0 ? -size : size;

                if (absSize - 1 > _stringBuffer.Length)
                {
                    ArrayPool<char>.Shared.Return(_stringBuffer);
                    _stringBuffer = ArrayPool<char>.Shared.Rent((absSize - 1) * 2);
                }

                var span = _stringBuffer.AsSpan().Slice(0, absSize - 1);

                if (size < 0)
                {
                    // multi-byte string
                    GetUtf16Chars(span, "multibyte-str");
                    _offset += 2; // skipped one '\0' byte in str
                    _position += 2;

                    ReadOnlySpan<char> ros = span;
                    var str = _stringCache.Add(ros);

                    _structureLog?.UpdateStack(str);
                    return str;
                }
                else
                {
                    // normal ascii string
                    GetAsciiChars(span, "str");
                    _offset += 1; // skipped one '\0' byte in str
                    _position += 1;
                    ReadOnlySpan<char> ros = span;
                    var str = _stringCache.Add(ros);

                    _structureLog?.UpdateStack(str);
                    return str;
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArkName GetStringAsName(string varName)
        {
            try
            {
                _structureLog?.PushStack("String", varName);

                GetInt(out var size, "size");
                if (size == 0) return null;
                if (size == 1)
                {
                    _position += 1;
                    return null;
                }
                if (size == -1)
                {
                    _position += 2;
                    return null;
                }

                var absSize = size < 0 ? -size : size;

                if (absSize - 1 > _stringBuffer.Length)
                {
                    ArrayPool<char>.Shared.Return(_stringBuffer);
                    _stringBuffer = ArrayPool<char>.Shared.Rent((absSize - 1) * 2);
                }

                var buffer = _stringBuffer.AsSpan().Slice(0, absSize - 1);

                if (size < 0)
                {
                    // multi-byte string
                    GetUtf16Chars(buffer, "multibyte-str");
                    _offset += 2; // skipped one '\0' byte in str
                    _position += 2;

                    var name = _stringCache.AddName(buffer);

                    _structureLog?.UpdateStack(name.Token);
                    return name;
                }
                else
                {
                    // normal ascii string
                    GetAsciiChars(buffer, "str");
                    _offset += 1; // skipped one '\0' byte in str
                    _position += 1;

                    var name = _stringCache.AddName(buffer);

                    _structureLog?.UpdateStack(name.Token);
                    return name;
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArkName GetName(string varName)
        {
            try
            {
                _structureLog?.PushStack("Name", varName);
                if (_nameTableState.nameTable == null || !_useNameTable)
                {
                    var name = GetStringAsName("nameAsString");
                    _structureLog?.UpdateStack(name.Token);
                    return name;
                }
                else
                {
                    var nameFromTable = GetNameFromTable();
                    _structureLog?.UpdateStack(nameFromTable.Token);
                    return nameFromTable;
                }
            }
            finally
            {
                _structureLog?.PopStack();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ArkName GetNameFromTable()
        {
            GetInt(out var id, "id");
            int internalId = id - _nameTableState.nameOffset;

            if (internalId < 0 || internalId >= _nameTableState.nameTable.Count)
            {
                return null;
            }

            var nameString = _nameTableState.nameTable[internalId];
            if (_nameTableState.instanceInTable) return _stringCache.AddName(nameString);

            GetInt(out var instance, "instance");

            // Get or create ArkName
            return _stringCache.AddName(nameString, instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            if (count < 0) throw new ApplicationException($"{nameof(Advance)} does not support seeking backwards.");

            _offset += count;
            _position += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AdvanceTo(long position)
        {
            if (position < _position) throw new ApplicationException($"{nameof(AdvanceTo)} does not support seeking backwards.");

            _offset += (int)(position - _position);
            _position = position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SeekTo(long position)
        {
            _offset += (int)(position - _position);
            _position = position;

            // flag that we need to update the buffer when seeking backwards and ending up outside the current buffer 
            if (_offset < 0) _size = _offset = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipString()
        {
            GetInt(out var size, "size");
            if (size == 0) return;
            if (size == 1)
            {
                _position += 1;
                return;
            }
            if (size == -1)
            {
                _position += 2;
                return;
            }

            var multibyte = size < 0;
            var absSize = Math.Abs(size);
            var readSize = multibyte ? absSize * 2 : absSize;

            _offset += readSize;
            _position += readSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipName()
        {
            if (_nameTableState.nameTable == null || !_useNameTable)
            {
                SkipString();
            }
            else
            {
                Advance(8);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNameTable(string[] nameTable, out ArchiveReaderNameTableState prevState, int offset = 1, bool instanceInTable = false)
        {
            prevState = _nameTableState;
            _nameTableState = new ArchiveReaderNameTableState();

            if (nameTable == null) return;

            _nameTableState.nameTable = nameTable;
            _nameTableState.nameOffset = offset;
            _nameTableState.instanceInTable = instanceInTable;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNameTable(ArchiveReaderNameTableState prevState)
        {
            _nameTableState = prevState;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSection(int size, out ArchiveReaderSectionState prevState)
        {
            prevState = _sectionState;
            _sectionState = new ArchiveReaderSectionState();

            _sectionState.position = _position;
            _sectionState.size = size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetSection(ArchiveReaderSectionState prevState)
        {
            _sectionState = prevState;
        }

        /// <summary>
        /// Create a secondary buffer for use with <see cref="ArchiveReader.HotSwapBuffer"/>.
        /// Used for improved performance when reading multiple sections concurrently (i.e. game-objects and properties).
        /// Caller is responsible for calling <see cref="ReleaseBuffer"/> when done with the returned buffer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArchiveReaderBuffer CreateBuffer(long position)
        {
            // avoid using multiple buffers if reading from memory
            if (_stream == null) return null;


            // create a new buffer with default values
            var buffer = new ArchiveReaderBuffer();
            buffer.offset = 0;
            buffer.size = 0;
            buffer.position = position;
            buffer.buffer = ArrayPool<byte>.Shared.Rent(_bufferSize); // new byte[_bufferSize];
            buffer.mem = new Memory<byte>(buffer.buffer);

            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseBuffer(ArchiveReaderBuffer buffer)
        {
            if (buffer == null) return;

            ArrayPool<byte>.Shared.Return(buffer.buffer);
            buffer.buffer = null;
        }

        /// <summary>
        /// Hot swap the buffer used by this instance of <see cref="ArchiveReader"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HotSwapBuffer(ArchiveReaderBuffer buffer, ref ArchiveReaderBuffer prevBuffer)
        {
            // avoid using multiple buffers if reading from memory
            if (_stream == null) return false;

            // store current values
            prevBuffer.offset = _offset;
            prevBuffer.size = _size;
            prevBuffer.position = _position;
            prevBuffer.buffer = _buffer;
            prevBuffer.mem = _mem;

            // hot swap buffer
            _offset = buffer.offset;
            _size = buffer.size;
            _position = buffer.position;
            _buffer = buffer.buffer;
            _mem = buffer.mem;

            return true;
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
                    //_dataReader?.Dispose();
                    //_dataReader = null;

                    _stream?.Dispose();
                    _stream = null;

                    _structureLog?.Dispose();
                    _structureLog = _saveReaderState._structureLog = null;

                    if (_bufferWasRented)
                    {
                        ArrayPool<byte>.Shared.Return(_buffer);
                        _buffer = null;
                    }

                    ArrayPool<char>.Shared.Return(_stringBuffer);
                    _stringBuffer = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ArkSaveReader()
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
