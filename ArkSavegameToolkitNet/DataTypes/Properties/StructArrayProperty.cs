using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class StructArrayProperty : ArrayProperty
    {
        public List<StructProperty> values;

        public override dynamic Value => values;
    }
}
