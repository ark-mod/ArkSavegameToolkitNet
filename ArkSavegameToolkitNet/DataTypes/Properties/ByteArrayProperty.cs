using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class ByteArrayProperty : ArrayProperty
    {
        public List<byte> values;

        public override dynamic Value => values;
    }
}
