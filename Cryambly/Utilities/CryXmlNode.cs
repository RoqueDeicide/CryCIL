using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Utilities
{
	/// <summary>
	/// Represents an interface for the implementation of the Xml node defined in CryEngine.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed partial class CryXmlNode : IDisposable
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		public extern string TagName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets number of attributes this node has.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		public extern int AttributeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets number of children this node has.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		public extern int ChildCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets parent node of this one.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		public extern CryXmlNode Parent
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets or sets content of this Xml node in text form.
		/// </summary>
		public extern string Content
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Creates deep clone of this Xml node.
		/// </summary>
		public extern CryXmlNode Clone
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates new Xml node that is represented by internal CryEngine implementation.
		/// </summary>
		/// <param name="name">Name of the node to create.</param>
		/// <exception cref="ArgumentNullException">Name of the Xml node cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CryXmlNode([UsedImplicitly] string name);
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
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given wrapper object cannot be null.</exception>
		/// <exception cref="NullReferenceException">Given Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddChild(CryXmlNode node);
		/// <summary>
		/// Inserts a child node at the specified position.
		/// </summary>
		/// <param name="index">Zero-based index of the location where to insert a child node.</param>
		/// <param name="node"> Node to insert as a child.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given wrapper object cannot be null.</exception>
		/// <exception cref="NullReferenceException">Given Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InsertChild(int index, CryXmlNode node);
		/// <summary>
		/// Removes a node from the list of child nodes of this node.
		/// </summary>
		/// <param name="node">Node to remove.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveChild(CryXmlNode node);
		/// <summary>
		/// Removes a node from the list of child nodes of this node.
		/// </summary>
		/// <param name="index">Zero-based index of the node to remove.</param>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveChildAt(int index);
		/// <summary>
		/// Removes all children from this node.
		/// </summary>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveChildren();
		/// <summary>
		/// Gets a child node.
		/// </summary>
		/// <param name="index">Zero-based index of the child node to get.</param>
		/// <returns>A wrapper object for the child node.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of bounds.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CryXmlNode GetChild(int index);
		/// <summary>
		/// Gets Xml data of this node and its children.
		/// </summary>
		/// <param name="level">Max depth of inheritance from which to get child nodes.</param>
		/// <returns>Xml data in text form.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string XmlData(int level = 0);
		/// <summary>
		/// Saves this node to the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This Xml node is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Save(string file);
		#region Attributes
		#endregion
		#endregion
		#region Utilities
		private void Dispose(bool suppressFinalize)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}
			this.Release();
			if (suppressFinalize)
			{
				GC.SuppressFinalize(this);
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();
		#endregion
	}
}