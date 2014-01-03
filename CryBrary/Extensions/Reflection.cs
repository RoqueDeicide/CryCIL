using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CryEngine.Extensions
{
    /// <summary>
    /// Useful extensions when working with reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        #region Inheritance
        /// <summary>
        /// Determines whether a given type is the child of another.
        /// </summary>
        /// <param name="thisType">The child type.</param>
        /// <param name="baseType">The possible parent type.</param>
        /// <returns>True if thisType is a child of baseType.</returns>
        public static bool Implements(this Type thisType, Type baseType)
        {
            return baseType.IsAssignableFrom(thisType) && thisType != baseType;
        }

        /// <summary>
        /// Determines whether a given type is the child of another.
        /// </summary>
        /// <typeparam name="T">The possible parent type.</typeparam>
        /// <param name="thisType">The child type.</param>
        /// <returns>True if thisType implements type T.</returns>
        public static bool Implements<T>(this Type thisType)
        {
            return thisType.Implements(typeof(T));
        }

        public static bool ImplementsOrEquals(this Type thisType, Type baseType)
        {
            return thisType == baseType || baseType.IsAssignableFrom(thisType);
        }

        public static bool ImplementsOrEquals<T>(this Type thisType)
        {
            return thisType.ImplementsOrEquals(typeof(T));
        }

        public static bool ImplementsGeneric(this Type thisType, Type genericBaseType)
        {
            var type = thisType;

            while (type != null)
            {
                var interfaceTypes = type.GetInterfaces();

                foreach (var it in interfaceTypes)
                {
                    if (it.IsGenericType && it.GetGenericTypeDefinition() == genericBaseType)
                        return true;
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        public static IEnumerable<Type> GetGenericArguments(this Type thisType, Type genericBaseType)
        {
            var type = thisType;

            while (type != null)
            {
                var interfaceTypes = type.GetInterfaces();

                foreach (var it in interfaceTypes)
                {
                    if (it.IsGenericType && it.GetGenericTypeDefinition() == genericBaseType)
                        return it.GetGenericArguments();
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                    return type.GetGenericArguments();

                type = type.BaseType;
            }

            return null;
        }
        #endregion

        #region Attributes
        /// <summary>
        /// Determines whether this member is decorated with at least one instance of a given attribute.
        /// </summary>
        /// <typeparam name="T">The attribute to search for.</typeparam>
        /// <param name="info">The member on which the search is performed.</param>
        /// <returns>True if the member is decorated with at least one instance of attribute T.</returns>
        public static bool ContainsAttribute<T>(this MemberInfo info) where T : Attribute
        {
            return info.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        /// <summary>
        /// Gets all instances of a given attribute on the selected member.
        /// </summary>
        /// <typeparam name="T">The attribute to search for.</typeparam>
        /// <param name="memberInfo">The member on which the search is performed.</param>
        /// <returns>The first instance of attribute T, or null if none is found.</returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo memberInfo) where T : Attribute
        {
            return (T[])memberInfo.GetCustomAttributes(typeof(T), true);
        }

        /// <summary>
        /// Gets the first instance of a given attribute on the selected member.
        /// </summary>
        /// <typeparam name="T">The attribute to search for.</typeparam>
        /// <param name="memberInfo">The member on which the search is performed.</param>
        /// <returns>The first instance of attribute T, or null if none is found.</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            var attributes = memberInfo.GetAttributes<T>();
            if (attributes.Any())
                return attributes.FirstOrDefault();

            return null;
        }

        /// <summary>
        /// Tests whether the method is decorated with a given attribute, and if so, assigns it via the out variable.
        /// </summary>
        /// <typeparam name="T">The attribute to search for.</typeparam>
        /// <param name="memberInfo">The member on which the search is performed.</param>
        /// <param name="attribute">The out parameter to which the attribute will be assigned.</param>
        /// <returns>True if the attribute exists.</returns>
        public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T attribute) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), true);

            if (attributes.Length > 0)
            {
                attribute = attributes[0] as T;
                return true;
            }

            attribute = null;
            return false;
        }
        #endregion
    }
}
