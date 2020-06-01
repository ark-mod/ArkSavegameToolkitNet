using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes
{
    public interface IPropertyContainer
    {
        Dictionary<ArkName, PropertyBase> Properties { get; }
    }
}
