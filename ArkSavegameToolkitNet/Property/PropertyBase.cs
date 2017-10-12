using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class PropertyBase<TValue> : IProperty<TValue>
    {
        [JsonProperty(Order = 0)]
        public ArkName Name { get; set; }
        //[JsonProperty(Order = 1)]
        //public ArkName TypeName { get; set; }
        public int DataSize { get; set; }
        public int Index { get; set; }
        [JsonProperty(Order = 1)]
        public abstract TValue Value { set; get; }
        //public abstract Type ValueClass { get; }

        protected internal TValue _value;

        public PropertyBase(string name, string typeName, int index, TValue value)
        {
            Name = ArkName.Create(name);
            //TypeName = ArkName.Create(typeName);
            Index = index;
            _value = value;
        }

        public PropertyBase(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false)
        {
            Name = args.Name;
            //TypeName = args.TypeName;
            DataSize = archive.GetInt();

            if (propertyIsExcluded)
            {
                archive.Position += 4;
                return;
            }

            Index = archive.GetInt();
        }

        //public string NameString
        //{
        //    get
        //    {
        //        return name.ToString();
        //    }
        //    set
        //    {
        //        name = ArkName.Create(value);
        //    }
        //}




        //public string TypeString
        //{
        //    get
        //    {
        //        return typeName.ToString();
        //    }
        //    set
        //    {
        //        this.typeName = ArkName.Create(value);
        //    }
        //}



        ///// <summary>
        ///// Calculates additional space required to serialize fields of this property.
        ///// </summary>
        ///// <param name="nameTable">
        ///// @return </param>
        //protected internal virtual int calculateAdditionalSize(bool nameTable)
        //{
        //    return 0;
        //}

        //public int calculateSize(bool nameTable)
        //{
        //    // dataSize index
        //    int size = Integer.BYTES * 2;

        //    size += ArkArchive.getNameLength(name, nameTable);
        //    size += ArkArchive.getNameLength(typeName, nameTable);
        //    size += calculateAdditionalSize(nameTable);
        //    size += calculateDataSize(nameTable);

        //    return size;
        //}

        ///// <summary>
        ///// Determines if the dataSize cannot be calculated and thus needs to be recorded.
        ///// </summary>
        ///// <returns> <tt>true</tt> if dataSize needs to be recorded </returns>
        //protected internal virtual bool DataSizeNeeded
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        //public virtual void CollectNames(ISet<string> nameTable)
        //{
        //    nameTable.Add(Name.Name);
        //    nameTable.Add(TypeName.Name);
        //}
    }
}
