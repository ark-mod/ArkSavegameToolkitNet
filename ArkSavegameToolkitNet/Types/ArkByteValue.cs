using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ArkByteValue : INameContainer, IConvertible
    {
        private bool _fromEnum;

        private sbyte _byteValue;

        private ArkName _enumName;

        private ArkName _nameValue;

        public ArkByteValue() { }

        public ArkByteValue(sbyte byteValue)
        {
            _fromEnum = false;
            _enumName = ArkName.NONE_NAME;
            _byteValue = byteValue;
        }

        public ArkByteValue(ArkName enumName, ArkName nameValue)
        {
            _fromEnum = true;
            _enumName = enumName;
            _nameValue = nameValue;
        }

        public ArkByteValue(ArkArchive archive, ArkName enumName)
        {
            read(archive, enumName);
        }

        [JsonProperty]
        public ArkName EnumName => _enumName;
        [JsonProperty]
        public ArkName NameValue => _nameValue;
        [JsonProperty]
        public bool FromEnum => _fromEnum;

        [JsonProperty]
        public sbyte ByteValue
        {
            get
            {
                return _byteValue;
            }
            set
            {
                _fromEnum = false;
                _enumName = ArkName.NONE_NAME;
                _byteValue = value;
            }
        }

        public void SetEnumValue(ArkName enumName, ArkName nameValue)
        {
            _fromEnum = true;
            _enumName = enumName;
            _nameValue = nameValue;
        }

        //public int getSize(bool nameTable)
        //{
        //    return _fromEnum ? ArkArchive.GetNameLength(_nameValue, nameTable) : 1;
        //}

        public void read(ArkArchive archive, ArkName enumName)
        {
            _enumName = enumName;
            _fromEnum = !enumName.Equals(ArkName.NONE_NAME);
            if (_fromEnum) _nameValue = archive.GetName();
            else _byteValue = archive.GetByte();
        }

        public void CollectNames(ISet<string> nameTable)
        {
            nameTable.Add(_enumName.Name);
            if (_fromEnum) nameTable.Add(_nameValue.Name);
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return ByteValue;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte)ByteValue;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)ByteValue;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)ByteValue;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (int)ByteValue;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)ByteValue;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long)ByteValue;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)ByteValue;
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
