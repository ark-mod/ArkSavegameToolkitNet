using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class Int8ArrayProperty : ArrayProperty
    {
        public List<byte> values;

        public override dynamic Value => values;
    }
}
