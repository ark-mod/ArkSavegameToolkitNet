namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public abstract class ObjectReferenceProperty : PropertyBase
    {
        public int type;
    }

    public class ObjectReferenceIdProperty : ObjectReferenceProperty
    {
        public int id;

        public override dynamic Value => id;
    }

    public class ObjectReferencePathProperty : ObjectReferenceProperty
    {
        public ArkName path;

        public override dynamic Value => path;
    }
}
