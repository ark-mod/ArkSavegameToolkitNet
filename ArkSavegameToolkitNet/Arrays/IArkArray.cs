using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public interface IArkArray<T> : IList<T>, IArkArray
    {
    }

    public interface IArkArray : IList, INameContainer
    {
        Type ValueClass { get; }

        //int calculateSize(bool nameTable);
    }
}
