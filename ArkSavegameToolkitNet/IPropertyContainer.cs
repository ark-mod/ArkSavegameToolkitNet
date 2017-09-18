using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public interface IPropertyContainer
    {
        IDictionary<ArkName, IProperty> Properties { get; set; }
    }
}
