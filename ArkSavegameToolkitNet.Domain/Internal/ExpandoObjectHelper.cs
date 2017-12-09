using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain.Internal
{
    public static class ExpandoObjectHelper
    {
        public static bool HasProperty(ExpandoObject expandoObject, string propertyName)
        {
            return ((IDictionary<String, object>)expandoObject).ContainsKey(propertyName);
        }
    }
}
