using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryCil.Annotations;

namespace CryCil
{
	/// <summary>
	/// Defines extension methods for reflection-related types.
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
		[Pure]
		public static bool Implements([NotNull] this Type thisType, [NotNull] Type baseType)
		{
			return baseType.IsAssignableFrom(thisType) && thisType != baseType;
		}
		/// <summary>
		/// Determines whether a given type is the child of another.
		/// </summary>
		/// <typeparam name="T">The possible parent type.</typeparam>
		/// <param name="thisType">The child type.</param>
		/// <returns>True if thisType implements type T.</returns>
		[Pure]
		public static bool Implements<T>([NotNull] this Type thisType)
		{
			return thisType.Implements(typeof(T));
		}
		/// <summary>
		/// Determines whether objects of this type can be assigned to another.
		/// </summary>
		/// <param name="thisType">This type.</param>
		/// <param name="baseType">Another type.</param>
		/// <returns>
		/// True, if <paramref name="thisType"/> is <paramref name="baseType"/> or is derived from it.
		/// </returns>
		[Pure]
		public static bool ImplementsOrEquals([NotNull] this Type thisType, [NotNull] Type baseType)
		{
			return thisType == baseType || baseType.IsAssignableFrom(thisType);
		}
		/// <summary>
		/// Determines whether objects of this type can be assigned to another.
		/// </summary>
		/// <typeparam name="T">Another type.</typeparam>
		/// <param name="thisType">This type.</param>
		/// <returns>
		/// True, if <paramref name="thisType"/> is <typeparamref name="T"/> or is derived from it.
		/// </returns>
		[Pure]
		public static bool ImplementsOrEquals<T>([NotNull] this Type thisType)
		{
			return thisType.ImplementsOrEquals(typeof(T));
		}
		/// <summary>
		/// Determines whether this type implements given generic type.
		/// </summary>
		/// <param name="thisType">       This type.</param>
		/// <param name="genericBaseType">Generic type.</param>
		/// <returns>True, this type implements given generic type.</returns>
		/// <exception cref="TargetInvocationException">
		/// A static initializer is invoked and throws an exception.
		/// </exception>
		[Pure]
		public static bool ImplementsGeneric([NotNull] this Type thisType, [NotNull] Type genericBaseType)
		{
			var type = thisType;

			while (type != null)
			{
				var interfaceTypes = type.GetInterfaces();

				if (interfaceTypes.Any(it => it.IsGenericType &&
											 it.GetGenericTypeDefinition() == genericBaseType))
				{
					return true;
				}

				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
					return true;

				type = type.BaseType;
			}

			return false;
		}
		/// <summary>
		/// Gets a collection of generic type arguments.
		/// </summary>
		/// <param name="thisType">       This type.</param>
		/// <param name="genericBaseType">Generic base type.</param>
		/// <returns>Enumeration of types.</returns>
		/// <exception cref="TargetInvocationException">
		/// A static initializer is invoked and throws an exception.
		/// </exception>
		[Pure]
		public static IEnumerable<Type> GetGenericArguments([NotNull] this Type thisType,
															[NotNull] Type genericBaseType)
		{
			var type = thisType;

			while (type != null)
			{
				var interfaceTypes = type.GetInterfaces();
				Type args = interfaceTypes.FirstOrDefault(x => x.IsGenericType &&
															   x.GetGenericTypeDefinition() == genericBaseType);
				if (args != null)
				{
					return args.GetGenericArguments();
				}

				if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
					return type.GetGenericArguments();

				type = type.BaseType;
			}

			return null;
		}
		/// <summary>
		/// Gets all types given one inherits from.
		/// </summary>
		/// <param name="type">Type which base types to get.</param>
		/// <returns>An array of all types given one inherits from.</returns>
		[Pure]
		public static Type[] GetInheritanceChain(this Type type)
		{
			return type.EnumerateInheritancChain().ToArray();
		}
		/// <summary>
		/// Enumerates through types this one inherits from.
		/// </summary>
		/// <param name="type">This type.</param>
		/// <returns>Enumerable collection that represents the inheritance chain.</returns>
		[Pure]
		public static IEnumerable<Type> EnumerateInheritancChain(this Type type)
		{
			for (var current = type; current != null; current = current.BaseType)
				yield return current;
		}
		#endregion
		#region Attributes
		/// <summary>
		/// Determines whether this reflection object is decorated with at least one instance of a given
		/// attribute.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The member on which the search is performed.</param>
		/// <returns>
		/// True if the member is decorated with at least one instance of attribute
		/// <typeparamref name="T"/>.
		/// </returns>
		[Pure]
		public static bool ContainsAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			return reflectionObject.IsDefined(typeof(T), true);
		}
		/// <summary>
		/// Gets all instances of a given attribute on the specified reflection object.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The object on which the search is performed.</param>
		/// <returns>
		/// An array of <typeparamref name="T"/> that contains all found attributes. Can be empty.
		/// </returns>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[Pure]
		[NotNull]
		public static T[] GetAttributes<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			// ReSharper disable once ExceptionNotDocumented
			return (T[])reflectionObject.GetCustomAttributes(typeof(T), true);
		}
		/// <summary>
		/// Gets the first instance of a given attribute on the specified reflection object.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The object on which the search is performed.</param>
		/// <returns>
		/// The first instance of attribute <typeparamref name="T"/>, or null if none is found.
		/// </returns>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[Pure]
		public static T GetAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject)
			where T : Attribute
		{
			return reflectionObject.GetAttributes<T>().FirstOrDefault();
		}
		/// <summary>
		/// Tests whether the reflection object is decorated with a given attribute, and if so, assigns it
		/// via the out variable.
		/// </summary>
		/// <typeparam name="T">The attribute to search for.</typeparam>
		/// <param name="reflectionObject">The reflection object on which the search is performed.</param>
		/// <param name="attribute">       
		/// The out parameter to which the attribute will be assigned.
		/// </param>
		/// <returns>True if the attribute exists.</returns>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		[Pure]
		[ContractAnnotation("=>true, attribute:notnull; =>false, attribute:null")]
		public static bool TryGetAttribute<T>([NotNull] this ICustomAttributeProvider reflectionObject,
											  out T attribute)
			where T : Attribute
		{
			var attributes = reflectionObject.GetCustomAttributes(typeof(T), true);

			if (attributes.Length > 0)
			{
				attribute = attributes[0] as T;
				return true;
			}

			attribute = null;
			return false;
		}
		#endregion
		#region Member Types
		/// <summary>
		/// Gets the type that is associated with a given member.
		/// </summary>
		/// <param name="info">Member.</param>
		/// <returns>Type of the field or property, if member is one of those, otherwise null.</returns>
		[Pure]
		public static Type GetAssociatedType([NotNull] this MemberInfo info)
		{
			switch (info.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)info).FieldType;
				case MemberTypes.Property:
					return ((PropertyInfo)info).PropertyType;
				default:
					return null;
			}
		}
		#endregion
		#region Methods
		/// <summary>
		/// Creates a delegate for given method.
		/// </summary>
		/// <typeparam name="T">Type of delegate to create.</typeparam>
		/// <param name="info">Method.</param>
		/// <returns>Delegate that allows to invoke method represented by <paramref name="info"/>.</returns>
		[Pure]
		[ContractAnnotation("info:null => null")]
		public static T CreateDelegate<T>(this MethodInfo info) where T : class
		{
			return (T)(object)info?.CreateDelegate(typeof(T));
		}
		/// <summary>
		/// Determines whether a list of parameters of the method matches specified types.
		/// </summary>
		/// <param name="info">An object that represents the method.</param>
		/// <param name="parameterTypes">An array of types of parameters the method's list must match.</param>
		/// <returns>True, if method's parameter count matches the length of <paramref name="parameterTypes"/> array, and all respecitive types are equal to each other.</returns>
		[Pure]
		[ContractAnnotation("info:null => false")]
		public static bool MatchesParameters(this MethodInfo info, params Type[] parameterTypes)
		{
			var parameters = info?.GetParameters();
			if (parameters?.Length != parameterTypes.Length)
			{
				return false;
			}

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType != parameterTypes[i])
				{
					return false;
				}
			}
			return true;
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Indicates whether one can create a default instance of this type.
		/// </summary>
		/// <param name="type">This type.</param>
		/// <returns>
		/// True, if <paramref name="type"/> is a value-type or a reference type that has defined public
		/// parameterless constructor.
		/// </returns>
		[Pure]
		[ContractAnnotation("type:null => false")]
		public static bool HasDefaultConstructor(this Type type)
		{
			if (type == null)
			{
				return false;
			}

			return type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
		}
		/// <summary>
		/// Determines whether this type contains a constructor that accepts parameters of specified types.
		/// </summary>
		/// <param name="type">          This type.</param>
		/// <param name="parameterTypes">Array of types of respective parameters.</param>
		/// <returns>
		/// True, if this type defines public constructor that accepts parameters of specified types.
		/// </returns>
		[Pure]
		[ContractAnnotation("type:null => false")]
		public static bool HasConstructor(this Type type, params Type[] parameterTypes)
		{
			if (type == null)
			{
				return false;
			}

			if (parameterTypes.IsNullOrEmpty())
			{
				return type.HasDefaultConstructor();
			}

			return type.GetConstructor(parameterTypes) != null;
		}
		#endregion
		#region Assemblies
		/// <summary>
		/// Gets file that contains the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <returns>Full path to the .dll file.</returns>
		public static string GetLocation([NotNull] this Assembly assembly)
		{
			return Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path);
		}
		#endregion
	}
}