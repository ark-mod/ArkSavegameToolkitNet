using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class NameArrayProperty : ArrayProperty
    {
        public List<ArkName> values;

        public override dynamic Value => values;
    }
}
