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

        private byte _byteValue;

        private ArkName _enumName;

        private ArkName _nameValue;

        public ArkByteValue() { }

        public ArkByteValue(byte byteValue)
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

        public ArkByteValue(ArkArchive archive, ArkName enumName, bool propertyIsExcluded = false)
        {
            read(archive, enumName, propertyIsExcluded);
        }

        [JsonProperty]
        public ArkName EnumName => _enumName;
        [JsonProperty]
        public ArkName NameValue => _nameValue;
        [JsonProperty]
        public bool FromEnum => _fromEnum;

        [JsonProperty]
        public byte ByteValue
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
        
        public void read(ArkArchive archive, ArkName enumName, bool propertyIsExcluded = false)
        {
            _enumName = enumName;
            _fromEnum = !enumName.Equals(ArkName.NONE_NAME);
            if (propertyIsExcluded)
            {
                if (_fromEnum) archive.SkipName();
                else archive.Position += 1;
            }
            else
            {
                if (_fromEnum) _nameValue = archive.GetName();
                else _byteValue = archive.GetByte();
            }
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
            var sbyteArray = new sbyte[1];
            Buffer.BlockCopy(new[] { ByteValue }, 0, sbyteArray, 0, 1);
            return sbyteArray[0];
        }

        public byte ToByte(IFormatProvider provider)
        {
            return ByteValue;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return ByteValue;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return ByteValue;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return ByteValue;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return ByteValue;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return ByteValue;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return ByteValue;
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
