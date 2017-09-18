using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayStruct : List<IStruct>, IArkArray<IStruct>
    {
        public ArkArrayStruct()
        {
        }

        public ArkArrayStruct(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();

            ArkName structType;
            if (size * 4 + 4 == dataSize)
            {
                structType = ArkName.Create("Color");
            }
            else if (size * 12 + 4 == dataSize)
            {
                structType = ArkName.Create("Vector");
            }
            else if (size * 16 + 4 == dataSize)
            {
                structType = ArkName.Create("LinearColor");
            }
            else
            {
                structType = null;
            }

            if (structType != null)
            {
                for (int n = 0; n < size; n++)
                {
                    Add(StructRegistry.read(archive, structType));
                }
            }
            else
            {
                for (int n = 0; n < size; n++)
                {
                    Add(new StructPropertyList(archive, null));
                }
            }
        }

        public Type ValueClass => typeof(IStruct);

        //public int calculateSize(bool nameTable)
        //{
        //    int size = Integer.BYTES;

        //    size += this.Select(s => s.getSize(nameTable)).Sum();

        //    return size;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
            foreach(var spl in this) spl.CollectNames(nameTable);
        }

    }
}
