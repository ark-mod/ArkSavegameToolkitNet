using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Properties
{
    public abstract class ArrayProperty : PropertyBase
    {
    }

    public class ArrayProperty<TValue> : ArrayProperty
    {
        public List<TValue> values = new List<TValue>();

        public override dynamic Value => values;
    }
}
