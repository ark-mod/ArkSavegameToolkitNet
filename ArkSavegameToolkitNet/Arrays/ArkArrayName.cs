using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayName : List<ArkName>, IArkArray<ArkName>
    {
        public ArkArrayName()
        {
        }

        public ArkArrayName(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                this.Add(archive.GetName());
            }
        }


        public Type ValueClass => typeof(ArkName);

        //public int calculateSize(bool nameTable)
        //{
        //    int size = Integer.BYTES;

        //    size += stream().mapToInt(n => ArkArchive.getNameLength(n, nameTable)).sum();

        //    return size;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
            foreach(var n in this) nameTable.Add(n.Name);
        }
    }
}
