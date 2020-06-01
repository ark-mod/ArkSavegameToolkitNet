using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System;

namespace ArkSavegameToolkitNet.Utils.Extensions
{
    public static class IPropertyContainerExtensions
    {
        public static TProperty GetProperty<TProperty>(this IPropertyContainer self, string token)
            where TProperty : PropertyBase
        {
            return self.Properties.TryGetValue(ArkName.Create(token), out var prop) ? prop as TProperty : null;
        }

        public static TProperty GetProperty<TProperty>(this IPropertyContainer self, ArkName name)
            where TProperty : PropertyBase
        {
            return self.Properties.TryGetValue(name, out var prop) ? prop as TProperty : null;
        }

        public static TPropertyValue GetPropertyValue<TPropertyValue>(this IPropertyContainer self, ArkName name)
        {
            PropertyBase prop = null;
            if (self.Properties.TryGetValue(name, out prop))
            {
                var value = prop.Value;
                if (value == null) return default;

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TPropertyValue)) ?? typeof(TPropertyValue));
            }
            else return default;
        }
    }
}
