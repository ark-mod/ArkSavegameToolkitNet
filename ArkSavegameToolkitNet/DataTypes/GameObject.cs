using ArkSavegameToolkitNet.DataTypes.Extras;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes
{
    public class GameObject : IDataEntry, IPropertyContainer
    {
        public int propertiesOffset;
        public int objectId;
        public Guid uuid;
        public ArkName className;
        public bool isItem;
        public List<ArkName> names;
        public bool fromDataFile;
        public int dataFileIndex;
        public LocationData? location;
        public Dictionary<ArkName, PropertyBase> properties = new Dictionary<ArkName, PropertyBase>();

        // interfaces
        public Dictionary<ArkName, PropertyBase> Properties => properties;
    }
}
