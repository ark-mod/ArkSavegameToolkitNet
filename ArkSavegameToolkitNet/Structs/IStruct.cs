using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Structs
{
    public interface IStruct : INameContainer
    {
        /// <summary>
        /// May be null if struct is contained in ArrayProperty
        /// 
        /// @return
        /// </summary>
        ArkName StructType { get; set; }

        bool Native { get; }

        //int getSize(bool nameTable);
    }
}
