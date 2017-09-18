using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public static class IPropertyContainerExtensions
    {
        //public static IProperty GetProperty(this IPropertyContainer self, string name)
        //{
        //    return self.GetProperty(name, 0);
        //}

        //public static IProperty GetProperty(this IPropertyContainer self, string name, int index)
        //{
        //    foreach (var prop in self.Properties)
        //    {
        //        if (prop.Index == index && prop.Name.Token.Equals(name))
        //        {
        //            return prop;
        //        }
        //    }
        //    return null;
        //}

        public static TProperty GetProperty<TProperty>(this IPropertyContainer self, string token)
            where TProperty: class, IProperty
        {
            IProperty prop = null;
            return self.Properties.TryGetValue(ArkName.Create(token), out prop) ? prop as TProperty : (TProperty)null;
        }

        public static TProperty GetProperty<TProperty>(this IPropertyContainer self, ArkName name)
            where TProperty : class, IProperty
        {
            IProperty prop = null;
            return self.Properties.TryGetValue(name, out prop) ? prop as TProperty : (TProperty)null;
        }

        public static TPropertyValue GetPropertyValue<TPropertyValue>(this IPropertyContainer self, ArkName name)
        {
            IProperty prop = null;
            if (self.Properties.TryGetValue(name, out prop))
            {
                var value = ((dynamic)prop).Value;
                if (value == null) return default(TPropertyValue);

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TPropertyValue)) ?? typeof(TPropertyValue));
            }
            else return default(TPropertyValue);
        }

        //public static bool IsTamed(this IPropertyContainer self)
        //{
        //    return self.GetProperty<PropertyInt32>("TargetingTeam")?.Value >= 50000;
        //}

        //public static bool IsWild(this IPropertyContainer self)
        //{
        //    return self.GetProperty<PropertyInt32>("TargetingTeam")?.Value < 50000;
        //}

        //public static IProperty FindProperty(this IPropertyContainer self, string name)
        //{
        //    return Optional.ofNullable(getProperty(name, 0));
        //}

        //	  public default java.util.Optional<qowyn.ark.properties.Property<JavaToDotNetGenericWildcard>> findProperty(String name, int index)
        //  {
        //	return Optional.ofNullable(getProperty(name, index));
        //  }

        //	  public default <T> T getTypedProperty(String name, Class<T> clazz)
        //  {
        //	return getTypedProperty(name, clazz, 0);
        //  }

        //@SuppressWarnings("unchecked") public default <T extends qowyn.ark.properties.Property<?>> T getTypedProperty(String name, Class<T> clazz, int index)
        //	  public default <T> T getTypedProperty(String name, Class<T> clazz, int index)
        //  {
        //	for (Property<?> prop : getProperties())
        //	{
        //	  if (prop.getIndex() == index && prop.getNameString().equals(name) && clazz.isInstance(prop))
        //	  {
        //		return (T) prop;
        //	  }
        //	}
        //	return null;
        //  }

        //	  public default <T> java.util.Optional<T> findTypedProperty(String name, Class<T> clazz)
        //  {
        //	return Optional.ofNullable(getTypedProperty(name, clazz, 0));
        //  }

        //	  public default <T> java.util.Optional<T> findTypedProperty(String name, Class<T> clazz, int index)
        //  {
        //	return Optional.ofNullable(getTypedProperty(name, clazz, index));
        //  }

        //	  public default <T> T getPropertyValue(String name, Class<T> clazz)
        //  {
        //	return getPropertyValue(name, clazz, 0);
        //  }

        //@SuppressWarnings("unchecked") public default <T> T getPropertyValue(String name, Class<T> clazz, int index)
        //	  public default <T> T getPropertyValue(String name, Class<T> clazz, int index)
        //  {
        //	for (Property<?> prop : getProperties())
        //	{
        //	  if (prop.getIndex() == index && prop.getNameString().equals(name) && clazz.isInstance(prop.getValue()))
        //	  {
        //		return (T) prop.getValue();
        //	  }
        //	}
        //
        //	return null;
        //  }

        //	  public default <T> java.util.Optional<T> findPropertyValue(String name, Class<T> clazz)
        //  {
        //	return Optional.ofNullable(getPropertyValue(name, clazz, 0));
        //  }

        //	  public default <T> java.util.Optional<T> findPropertyValue(String name, Class<T> clazz, int index)
        //  {
        //	return Optional.ofNullable(getPropertyValue(name, clazz, index));
        //  }

        //	  public default boolean hasAnyProperty(String name)
        //  {
        //	for (Property<?> prop : getProperties())
        //	{
        //	  if (prop.getNameString().equals(name))
        //	  {
        //		return true;
        //	  }
        //	}
        //	return false;
        //  }
    }
}
