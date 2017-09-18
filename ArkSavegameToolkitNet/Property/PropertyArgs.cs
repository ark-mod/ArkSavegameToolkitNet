using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public sealed class PropertyArgs
    {
        public ArkName Name => _name;
        public ArkName TypeName => _typeName;

        private readonly ArkName _name;
        private readonly ArkName _typeName;

        public PropertyArgs(ArkName name, ArkName typeName)
        {
            _name = name;
            _typeName = typeName;
        }
    }
}
