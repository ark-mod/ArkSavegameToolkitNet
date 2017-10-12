using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayDouble : List<double?>, IArkArray<double?>
    {
        public ArkArrayDouble()
        {
        }

        public ArkArrayDouble(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetDouble());
            }
        }

        public Type ValueClass => typeof(double?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Double.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }

    }
}
