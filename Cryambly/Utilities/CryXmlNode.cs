using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Utilities
{
	/// <summary>
	/// Represents an interface for the implementation of the Xml node defined in CryEngine.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed partial class CryXmlNode : IDisposable
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the pointer to the underlying object.
		/// </summary>
		public IntPtr Handle => this.handle;
		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string TagName
		{
			get
			{
				this.AssertInstance();

				return GetTagName(this.handle);
			}
		}
		/// <summary>
		/// Gets number of attributes this node has.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int AttributeCount
		{
			get
			{
				this.AssertInstance();

				return GetAttributeCount(this.handle);
			}
		}
		/// <summary>
		/// Gets number of children this node has.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ChildCount
		{
			get
			{
				this.AssertInstance();

				return GetChildCount(this.handle);
			}
		}
		/// <summary>
		/// Gets parent node of this one.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryXmlNode Parent
		{
			get
			{
				this.AssertInstance();

				return new CryXmlNode(GetParent(this.handle));
			}
		}
		/// <summary>
		/// Gets or sets content of this Xml node in text form.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Content
		{
			get
			{
				this.AssertInstance();

				return GetContent(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetContent(this.handle, value);
			}
		}
		/// <summary>
		/// Creates deep clone of this Xml node.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryXmlNode Clone
		{
			get
			{
				this.AssertInstance();

				return new CryXmlNode(GetClone(this.handle));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new Xml node that is represented by internal CryEngine implementation.
		/// </summary>
		/// <param name="name">Name of the node to create.</param>
		/// <exception cref="ArgumentNullException">Name of the Xml node cannot be null.</exception>
		public CryXmlNode(string name)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "Name of the Xml node cannot be null.");
			}

			this.handle = Ctor(name);
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="handle">Pointer to the underlying implementation object.</param>
		public CryXmlNode(IntPtr handle)
		{
			this.handle = handle;
			if (this.IsValid)
			{
				AddRef(this.handle);
			}
		}
		/// <summary>
		/// Finalizes this object.
		/// </summary>
		~CryXmlNode()
		{
			this.Dispose(false);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases underlying Xml node object.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		/// <summary>
		/// Adds a node to the list of child nodes of this node.
		/// </summary>
		/// <param name="node">Node to add as a child.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given node is not valid.</exception>
		public void AddChild(CryXmlNode node)
		{
			this.AssertInstance();
			if (node == null || !node.IsValid)
			{
				throw new ArgumentNullException(nameof(node), "Given node is not valid.");
			}

			AddChildInternal(this.handle, node.handle);
		}
		/// <summary>
		/// Inserts a child node at the specified position.
		/// </summary>
		/// <param name="index">Zero-based index of the location where to insert a child node.</param>
		/// <param name="node"> Node to insert as a child.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given node is not valid.</exception>
		public void InsertChild(int index, CryXmlNode node)
		{
			this.AssertInstance();
			if (node == null || !node.IsValid)
			{
				throw new ArgumentNullException(nameof(node), "Given node is not valid.");
			}

			InsertChildInternal(this.handle, index, node.handle);
		}
		/// <summary>
		/// Removes a node from the list of child nodes of this node.
		/// </summary>
		/// <param name="node">Node to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given node is not valid.</exception>
		public void RemoveChild(CryXmlNode node)
		{
			this.AssertInstance();
			if (node == null || !node.IsValid)
			{
				throw new ArgumentNullException(nameof(node), "Given node is not valid.");
			}

			RemoveChildInternal(this.handle, node.handle);
		}
		/// <summary>
		/// Removes a node from the list of child nodes of this node.
		/// </summary>
		/// <param name="index">Zero-based index of the node to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveChildAt(int index)
		{
			this.AssertInstance();

			RemoveChildAtInternal(this.handle, index);
		}
		/// <summary>
		/// Removes all children from this node.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveChildren()
		{
			this.AssertInstance();

			RemoveChildrenInternal(this.handle);
		}
		/// <summary>
		/// Gets a child node.
		/// </summary>
		/// <param name="index">Zero-based index of the child node to get.</param>
		/// <returns>A wrapper object for the child node.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		[CanBeNull]
		public CryXmlNode GetChild(int index)
		{
			this.AssertInstance();

			IntPtr ptr = GetChildInternal(this.handle, index);

			return ptr == IntPtr.Zero ? null : new CryXmlNode(ptr);
		}
		/// <summary>
		/// Gets Xml data of this node and its children.
		/// </summary>
		/// <param name="level">Max depth of inheritance from which to get child nodes.</param>
		/// <returns>Xml data in text form.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string XmlData(int level = 0)
		{
			this.AssertInstance();

			return XmlDataInternal(this.handle, level);
		}
		/// <summary>
		/// Saves this node to the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Save(string file)
		{
			this.AssertInstance();

			return SaveInternal(this.handle, file);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		private void Dispose(bool suppressFinalize)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			Release(this.handle);
			if (suppressFinalize)
			{
				GC.SuppressFinalize(this);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Ctor(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetTagName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAttributeCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetChildCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetParent(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetContent(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetContent(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetClone(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddChildInternal(IntPtr handle, IntPtr node);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InsertChildInternal(IntPtr handle, int index, IntPtr node);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveChildInternal(IntPtr handle, IntPtr node);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveChildAtInternal(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveChildrenInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetChildInternal(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string XmlDataInternal(IntPtr handle, int level);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SaveInternal(IntPtr handle, string file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeInternal(IntPtr handle, int index, out string name, out string value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributestring(IntPtr handle, string name, out string value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeint(IntPtr handle, string name, out int value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeuint(IntPtr handle, string name, out uint value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeint64(IntPtr handle, string name, out long value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeuint64(IntPtr handle, string name, out ulong value, bool useHex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributefloat(IntPtr handle, string name, out float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributedouble(IntPtr handle, string name, out double value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeVec2(IntPtr handle, string name, out Vector2 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeVec2d(IntPtr handle, string name, out Vector2d value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeAng3(IntPtr handle, string name, out EulerAngles value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeVec3(IntPtr handle, string name, out Vector3 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeVec3d(IntPtr handle, string name, out Vector3d value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeVec4(IntPtr handle, string name, out Vector4 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAttributeQuat(IntPtr handle, string name, out Quaternion value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributestring(IntPtr handle, string name, string value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeint(IntPtr handle, string name, int value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeuint(IntPtr handle, string name, uint value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeint64(IntPtr handle, string name, long value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeuint64(IntPtr handle, string name, ulong value, bool useHex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributefloat(IntPtr handle, string name, float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributedouble(IntPtr handle, string name, double value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeVec2(IntPtr handle, string name, Vector2 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeVec2d(IntPtr handle, string name, Vector2d value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeAng3(IntPtr handle, string name, EulerAngles value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeVec3(IntPtr handle, string name, Vector3 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeVec3d(IntPtr handle, string name, Vector3d value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeVec4(IntPtr handle, string name, Vector4 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAttributeQuat(IntPtr handle, string name, Quaternion value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasAttributeInternal(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyAttributesInternal(IntPtr handle, IntPtr node);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveAttributeInternal(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveAttributesInternal(IntPtr handle);
		#endregion
	}
}