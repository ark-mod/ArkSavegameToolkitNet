namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public abstract class EnumProperty : PropertyBase
    {
        public ArkName @enum;
    }

    public class EnumNameProperty : EnumProperty
    {
        public ArkName value;

        public override dynamic Value => value;
    }

    public class EnumByteProperty : EnumProperty
    {
        public byte value;

        public override dynamic Value => value;
    }
}
