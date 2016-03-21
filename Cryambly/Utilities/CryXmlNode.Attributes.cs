using System;
using System.Linq;
using CryCil.Geometry;

namespace CryCil.Utilities
{
	public partial class CryXmlNode
	{
		/// <summary>
		/// Gets the name and value of the attribute using the index.
		/// </summary>
		/// <param name="index">Zero-based index of the attribute to get.</param>
		/// <param name="name"> Returned name of the attribute.</param>
		/// <param name="value">Returned value of the attribute.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(int index, out string name, out string value)
		{
			this.AssertInstance();

			return GetAttributeInternal(this.handle, index, out name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned text value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out string value)
		{
			this.AssertInstance();

			return GetAttributestring(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out int value)
		{
			this.AssertInstance();

			return GetAttributeint(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out uint value)
		{
			this.AssertInstance();

			return GetAttributeuint(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned integer value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out long value)
		{
			this.AssertInstance();

			return GetAttributeint64(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name">  Name of the attribute which value to get.</param>
		/// <param name="value"> Returned integer value of the attribute.</param>
		/// <param name="useHex">Indicates whether the value is in hexadecimal format.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out ulong value, bool useHex = true)
		{
			this.AssertInstance();

			return GetAttributeuint64(this.handle, name, out value, useHex);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned floating-point number value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out float value)
		{
			this.AssertInstance();

			return GetAttributefloat(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned floating-point number value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out double value)
		{
			this.AssertInstance();

			return GetAttributedouble(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Vector2 value)
		{
			this.AssertInstance();

			return GetAttributeVec2(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Vector2d value)
		{
			this.AssertInstance();

			return GetAttributeVec2d(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned angles vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out EulerAngles value)
		{
			this.AssertInstance();

			return GetAttributeAng3(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Vector3 value)
		{
			this.AssertInstance();

			return GetAttributeVec3(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Vector3d value)
		{
			this.AssertInstance();

			return GetAttributeVec3d(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned vector value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Vector4 value)
		{
			this.AssertInstance();

			return GetAttributeVec4(this.handle, name, out value);
		}
		/// <summary>
		/// Gets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to get.</param>
		/// <param name="value">Returned quaternion value of the attribute.</param>
		/// <returns>True, if attribute exists, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAttribute(string name, out Quaternion value)
		{
			this.AssertInstance();

			return GetAttributeQuat(this.handle, name, out value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New text value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, string value)
		{
			this.AssertInstance();

			SetAttributestring(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, int value)
		{
			this.AssertInstance();

			SetAttributeint(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, uint value)
		{
			this.AssertInstance();

			SetAttributeuint(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New integer value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, long value)
		{
			this.AssertInstance();

			SetAttributeint64(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name">  Name of the attribute which value to set.</param>
		/// <param name="value"> New integer value of the attribute.</param>
		/// <param name="useHex">Indicates whether the value is in hexadecimal format.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, ulong value, bool useHex = true)
		{
			this.AssertInstance();

			SetAttributeuint64(this.handle, name, value, useHex);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New floating-point number value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, float value)
		{
			this.AssertInstance();

			SetAttributefloat(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New floating-point number value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, double value)
		{
			this.AssertInstance();

			SetAttributedouble(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Vector2 value)
		{
			this.AssertInstance();

			SetAttributeVec2(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Vector2d value)
		{
			this.AssertInstance();

			SetAttributeVec2d(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New angles vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, EulerAngles value)
		{
			this.AssertInstance();

			SetAttributeAng3(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Vector3 value)
		{
			this.AssertInstance();

			SetAttributeVec3(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Vector3d value)
		{
			this.AssertInstance();

			SetAttributeVec3d(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New vector value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Vector4 value)
		{
			this.AssertInstance();

			SetAttributeVec4(this.handle, name, value);
		}
		/// <summary>
		/// Sets the value of the attribute using the name.
		/// </summary>
		/// <param name="name"> Name of the attribute which value to set.</param>
		/// <param name="value">New quaternion value of the attribute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAttribute(string name, Quaternion value)
		{
			this.AssertInstance();

			SetAttributeQuat(this.handle, name, value);
		}
		/// <summary>
		/// Determines whether this Xml node contains an attribute with specified name.
		/// </summary>
		/// <param name="name">Name of the attribute.</param>
		/// <returns>
		/// True, if attribute exists, otherwise false. Will return false, if provided name is null.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HasAttribute(string name)
		{
			this.AssertInstance();

			return HasAttributeInternal(this.handle, name);
		}
		/// <summary>
		/// Copies attributes from given node to this one.
		/// </summary>
		/// <param name="node">Xml node to copy attributes from.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CopyAttributes(CryXmlNode node)
		{
			this.AssertInstance();

			if (node == null || !node.IsValid)
			{
				return;
			}

			CopyAttributesInternal(this.handle, node.handle);
		}
		/// <summary>
		/// Removes an attribute from this node.
		/// </summary>
		/// <param name="name">Name of the attribute to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAttribute(string name)
		{
			this.AssertInstance();

			RemoveAttributeInternal(this.handle, name);
		}
		/// <summary>
		/// Removes all attributes from this node.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAttributes()
		{
			this.AssertInstance();

			RemoveAttributesInternal(this.handle);
		}
	}
}