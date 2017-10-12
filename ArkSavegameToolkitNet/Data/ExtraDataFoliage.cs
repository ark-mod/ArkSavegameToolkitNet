using ArkSavegameToolkitNet.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataFoliage : IExtraData, INameContainer
    {
        public IList<IDictionary<string, StructPropertyList>> StructMapList { get; set; }


        //public int calculateSize(bool nameTable)
        //{
        //    var size = /* integer size */ 4 * 2;

        //    size += /* integer size */ 4 * structMapList.Count;
        //    foreach (IDictionary<string, StructPropertyList> structMap in structMapList)
        //    {
        //        foreach (KeyValuePair<string, StructPropertyList> entry in structMap)
        //        {
        //            size += ArkArchive.getStringLength(entry.Key);
        //            size += entry.Value.getSize(nameTable);
        //            size += /* integer size */ 4;
        //        }
        //    }

        //    return size;
        //}

        //public void CollectNames(ISet<string> nameTable)
        //{
        //    foreach (IDictionary<string, StructPropertyList> structMap in StructMapList)
        //    {
        //        foreach (KeyValuePair<string, StructPropertyList> entry in structMap)
        //        {
        //            entry.Value.CollectNames(nameTable);
        //        }
        //    }
        //}
    }
}
