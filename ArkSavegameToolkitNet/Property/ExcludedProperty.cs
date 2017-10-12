using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkSavegameToolkitNet.Types;

namespace ArkSavegameToolkitNet.Property
{
    public class ExcludedProperty : PropertyBase<bool>
    {
        public ExcludedProperty() : base(string.Empty, string.Empty, 0, false)
        {
        }

        static ExcludedProperty()
        {
            Instance = new ExcludedProperty();
        }
        
        public static ExcludedProperty Instance { get; private set; }

        public override bool Value { get; set; }
    }
}
