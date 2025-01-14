using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Win32;

namespace Lib.Tools
{
    public static class Ext
    {
        #region Public Methods

        /// <summary>
        /// Copy properties from one to another class objects
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="onlySelectedProperties">
        /// Copy only properties, which have attribute [Copy]. Optional, false by default.
        /// </param>
        public static void Copy<TParent, TChild>(TParent parent, TChild child, bool onlySelectedProperties = false)
            where TParent : class
            where TChild : class
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();
            Dictionary<string, PropertyInfo> parentProps = [];
            Dictionary<string, PropertyInfo> childProps = [];

            // Get parent properties
            foreach (var parentProperty in parentProperties)
            {
                if (onlySelectedProperties)
                {
                    if (!CopyAttribute.IsCopy(parentProperty))
                    {
                        continue;
                    }
                }
                parentProps[parentProperty.Name] = parentProperty;
            }

            // Get child properties
            foreach (var childProperty in childProperties)
            {
                if (onlySelectedProperties)
                {
                    if (!CopyAttribute.IsCopy(childProperty))
                    {
                        continue;
                    }
                }
                childProps[childProperty.Name] = childProperty;
            }

            // Set all founded properties
            foreach (var parentProperty in parentProps)
            {
                if (
                    childProps.TryGetValue(
                        parentProperty.Key,
                        out PropertyInfo childProperty)                                    // Some properties from parent can be missing in child
                    && parentProperty.Value.PropertyType == childProperty.PropertyType    // Check properties types
                )
                {
                    childProperty.SetValue(child, parentProperty.Value.GetValue(parent));
                }
            }
        }

        public static string CurrentWallpaper()
        {
            byte[] path = (byte[])Registry.CurrentUser
                .OpenSubKey(@"Control Panel\Desktop")
                .GetValue(@"TranscodedImageCache");
            string fullPath = Encoding.Unicode.GetString(SliceBytes(path, 24)).TrimEnd("\0".ToCharArray());
            return fullPath;
        }

        public static void Deconstruct<T>(this T[] items, out T t0, out T t1)
        {
            t0 = items.Length > 0 ? items[0] : default;
            t1 = items.Length > 1 ? items[1] : default;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list) { action(item); }
            return list;
        }

        public static IEnumerable<KeyValuePair<K, V>> Each<K, V>(
            this IEnumerable<KeyValuePair<K, V>> dict,
            Action<K, V> action
        )
        {
            foreach (KeyValuePair<K, V> kv in dict) { action(kv.Key, kv.Value); }
            return dict;
        }

        public static IDictionary<K, V> Each<K, V>(
            this IDictionary<K, V> dict,
            Action<K, V> action
        )
        {
            foreach (KeyValuePair<K, V> kv in dict) { action(kv.Key, kv.Value); }
            return dict;
        }

        /// <summary>
        /// Parse generic enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="theEnum"></param>
        /// <returns></returns>
        public static bool EnumTryParse<T>(string input, out T theEnum)
        {
            foreach (string en in Enum.GetNames(typeof(T)))
            {
                if (en.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    theEnum = (T)Enum.Parse(typeof(T), input, true);
                    return true;
                }
            }

            theEnum = default;
            return false;
        }

        /// <summary>
        /// Find all assignable types from type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssignableFrom<T>() where T : class
        {
            var type = typeof(T);
            return Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableFrom(type));
        }

        /// <summary>
        /// Find all inheritted types of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetSubclassOf<T>() where T : class
        {
            var type = typeof(T);
            return Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(type));
        }

        /// <summary>
        /// Check if string is enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="theEnum"></param>
        /// <returns></returns>
        public static bool IsEnum<T>(string input, T theEnum)
        {
            foreach (string en in Enum.GetNames(typeof(T)))
            {
                if (en.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    return en.Equals(theEnum.ToString(), StringComparison.CurrentCultureIgnoreCase);
                }
            }
            return false;
        }

        public static bool IsHandlerRegistered(this EventHandler ev, Delegate handler)
        {
            if (ev != null)
            {
                foreach (Delegate h in ev.GetInvocationList())
                {
                    if (h == handler)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static IDictionary<K, V> RemoveAll<K, V>(
                    this IDictionary<K, V> dict,
            IEnumerable<K> keys
        )
        {
            foreach (K key in keys) { dict.Remove(key); }
            return dict;
        }

        #endregion Public Methods

        //private static bool IsSubclassOfGeneric(Type target, Type type)
        //{
        //    Type currentType = target;
        //    while (currentType != null)
        //    {
        //        if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == type)
        //        {
        //            return true;
        //        }
        //        currentType = currentType.BaseType;
        //    }
        //    return false;
        //}

        #region Private Methods

        // Source: http://stackoverflow.com/a/406576/441907
        private static byte[] SliceBytes(byte[] source, int pos)
        {
            byte[] destfoo = new byte[source.Length - pos];
            Array.Copy(source, pos, destfoo, 0, destfoo.Length);
            return destfoo;
        }

        #endregion Private Methods

        #region Public Classes

        /// <summary>
        /// Allow to copy this property to another object
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class CopyAttribute : Attribute
        {
            #region Public Methods

            /// <summary>
            /// Check if property have Copy attribute
            /// </summary>
            /// <param name="prop"></param>
            /// <returns></returns>
            public static bool IsCopy(PropertyInfo prop)
            {
                var attributes = prop.GetCustomAttributes(typeof(CopyAttribute), true);
                foreach (var attribute in attributes)
                {
                    if (attribute.GetType() == typeof(CopyAttribute))
                    {
                        return true;
                    }
                }
                return false;
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}
