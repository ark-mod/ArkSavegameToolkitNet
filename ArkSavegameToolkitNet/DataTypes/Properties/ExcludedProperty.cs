namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class ExcludedProperty : ValueProperty<bool>
    {
        public ExcludedProperty()
        {
            name = ArkName.Empty;
        }

        static ExcludedProperty()
        {
            Instance = new ExcludedProperty();
        }

        public static ExcludedProperty Instance { get; private set; }
    }
}