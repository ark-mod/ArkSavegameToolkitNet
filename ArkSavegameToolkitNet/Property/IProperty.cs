using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{ 
    public interface IProperty : INameContainer
    {
        Type ValueClass { get; }
        ArkName Name { get; set; }
        ArkName TypeName { get; set; }
        int DataSize { get; set; }
        int Index { get; set; }
    }

    public interface IProperty<TValue> : IProperty
    {
        TValue Value { get; set; }


        ///// <summary>
        ///// Calculates the value for the dataSize field
        ///// </summary>
        ///// <param name="nameTable"> <tt>true</tt> if using String deduplication will be used </param>
        ///// <returns> value of dataSize field </returns>
        //int calculateDataSize(bool nameTable);

        ///// <summary>
        ///// Calculates the amount of bytes required to serialize this property.
        ///// 
        ///// Includes everything contained in this property.
        ///// </summary>
        ///// <param name="nameTable"> <tt>true</tt> if using String deduplication will be used </param>
        ///// <returns> amount of bytes required to write this property in raw binary representation </returns>
        //int calculateSize(bool nameTable);
    }
}
