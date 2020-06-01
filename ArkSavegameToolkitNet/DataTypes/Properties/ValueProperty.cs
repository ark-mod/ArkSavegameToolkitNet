namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public class ValueProperty<TValue> : PropertyBase
    {
        public TValue value;

        public override dynamic Value => value;
    }
}
