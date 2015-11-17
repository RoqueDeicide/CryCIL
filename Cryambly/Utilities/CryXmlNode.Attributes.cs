using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Utilities
{
	// ReSharper disable ExceptionNotThrown
	public partial class CryXmlNode
	{
		/// <summary>
		/// Gets the name and value of the attribute using the index.
		/// </summary>
		/// <param name="index">Zero-based index of the attribute to get.</param>
		/// <param name="name"> Returned name of the attribute.</param>
		/// <param name="value">Returned value of the attribute.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(int index, out string name, out string value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned text value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out string value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out int value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out uint value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out long value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name">  Name of the attribute which value to get.</param>
		/// <param name="value"> Returned integer value of the attribute.</param>
		/// <param name="useHex">Indicates whether the value is in hexadecimal format.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out ulong value, bool useHex = true);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned floating-point number value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out float value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned floating-point number value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out double value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Vector2 value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Vector2d value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned angles vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out EulerAngles value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Vector3 value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Vector3d value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Vector4 value);
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned quaternion value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetAttribute(string name, out Quaternion value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New text value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, string value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, int value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, uint value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, long value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name">  Name of the attribute which value to set.</param>
		/// <param name="value"> New integer value of the attribute.</param>
		/// <param name="useHex">Indicates whether the value is in hexadecimal format.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, ulong value, bool useHex = true);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New floating-point number value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, float value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New floating-point number value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, double value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Vector2 value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Vector2d value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New angles vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, EulerAngles value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Vector3 value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Vector3d value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Vector4 value);
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New quaternion value of the attribute.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAttribute(string name, Quaternion value);
		/// <summary>
		/// Determines whether this Xml node contains an attribute with specified name.
		/// </summary>
		/// <param name="name">Name of the attribute.</param>
		/// <returns>
		/// True, if attribute exists, otherwise false. Will return false, if provided name is null.
		/// </returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasAttribute(string name);
		/// <summary>
		/// Copies attributes from given node to this one.
		/// </summary>
		/// <param name="node">Xml node to copy attributes from.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyAttributes(CryXmlNode node);
		/// <summary>
		/// Removes an attribute from this node.
		/// </summary>
		/// <param name="name">Name of the attribute to remove.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAttribute(string name);
		/// <summary>
		/// Removes all attributes from this node.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAttributes();
	}
}