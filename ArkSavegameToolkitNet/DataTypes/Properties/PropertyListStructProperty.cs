using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class PropertyListStructProperty : StructProperty, IPropertyContainer
    {
        public Dictionary<ArkName, PropertyBase> properties;

        // interfaces
        public Dictionary<ArkName, PropertyBase> Properties => properties;

        public override dynamic Value => throw new System.NotImplementedException();
    }
}
