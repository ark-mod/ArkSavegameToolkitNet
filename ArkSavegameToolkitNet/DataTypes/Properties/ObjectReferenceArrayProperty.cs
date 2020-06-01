using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class ObjectReferenceArrayProperty : ArrayProperty
    {
        public List<ObjectReferenceProperty> values;

        public override dynamic Value => values;
    }
}
