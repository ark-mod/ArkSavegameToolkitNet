using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayObjectReference : List<ObjectReference>, IArkArray<ObjectReference>
    {
        public ArkArrayObjectReference()
        {
        }

        public ArkArrayObjectReference(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();

            for (int n = 0; n < size; n++)
            {
                Add(new ObjectReference(archive, 8)); // Fixed size?
            }
        }

        public Type ValueClass => typeof(ObjectReference);

        //public int calculateSize(bool nameTable)
        //{
        //    int size = Integer.BYTES;

        //    size += this.Select(or => or.getSize(nameTable)).Sum();

        //    return size;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
            foreach(var or in this) or.CollectNames(nameTable);
        }
    }
}
