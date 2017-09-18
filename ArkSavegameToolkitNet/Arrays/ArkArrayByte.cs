using ArkSavegameToolkitNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayByte : List<sbyte?>, IArkArray<sbyte?>
    {
        public ArkArrayByte()
        {
        }

        public ArkArrayByte(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();

            if (size + 4 != dataSize)
            {
                throw new UnreadablePropertyException();
            }

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetByte());
            }
        }

        public Type ValueClass => typeof(sbyte?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Byte.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
