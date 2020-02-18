using ArkSavegameToolkitNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayByte : List<byte?>, IArkArray<byte?>
    {
        public ArkArrayByte()
        {
        }

        public ArkArrayByte(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            if (size + 4 != dataSize)
            {
                throw new UnreadablePropertyException();
            }

            AddRange(archive.GetBytes(size).Cast<byte?>().ToArray());
        }

        public Type ValueClass => typeof(byte?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Byte.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
