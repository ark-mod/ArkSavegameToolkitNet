namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public abstract class PropertyBase : IDataEntry
    {
        public ArkName name;
        public int index;

        public abstract dynamic Value { get; }
    }
}
