using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a sub object of a static object.
	/// </summary>
	public unsafe struct StaticSubObject
	{
		#region Nested Types
		private struct SubObject
		{
#pragma warning disable 649,169
			public StaticSubObjectType type;
			public IntPtr name;
			public IntPtr properties;
			public int parent; // Index of the parent sub object, if there`s hierarchy between them.
			public Matrix34 tm; // Transformation matrix.
			public Matrix34 localTM; // Local transformation matrix, relative to parent.
			public StaticObject statObj; // Static object for sub part of CGF.
			public Vector3 helperSize; // Size of the helper (if helper).
			public IntPtr weights; // render mesh with a single deformation weights stream
			public IntPtr foliage; // for bendable foliage
#pragma warning restore 649,169
		}
		#endregion
		#region Fields
		private readonly SubObject* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != null;
		/// <summary>
		/// Gets or sets the type of this static sub-object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticSubObjectType Type
		{
			get
			{
				this.AssertInstance();

				return this.handle->type;
			}
			set
			{
				this.AssertInstance();

				this.handle->type = value;
			}
		}
		/// <summary>
		/// Gets or sets the name of this sub-object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the text that contains text-based properties that could have been assigned to this
		/// sub-object when exporting CGF.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Properties
		{
			get
			{
				this.AssertInstance();

				return GetProperties(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProperties(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the zero-based index of the parent sub-object within this compound static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ParentIndex
		{
			get
			{
				this.AssertInstance();

				return this.handle->parent;
			}
			set
			{
				this.AssertInstance();

				this.handle->parent = value;
			}
		}
		/// <summary>
		/// Gets the current world-space(?) transformation matrix of this sub-object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 Transformation
		{
			get
			{
				this.AssertInstance();

				return this.handle->tm;
			}
		}
		/// <summary>
		/// Gets the current transformation matrix of this sub-object that is relative to the parent static
		/// object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 LocalTransformation
		{
			get
			{
				this.AssertInstance();

				return this.handle->localTM;
			}
			set
			{
				this.AssertInstance();

				this.handle->localTM = value;
			}
		}
		/// <summary>
		/// Gets the static object that is wrapped by this sub-object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject StaticObject
		{
			get
			{
				this.AssertInstance();

				return this.handle->statObj;
			}
			set
			{
				this.AssertInstance();

				this.handle->statObj = value;
			}
		}
		/// <summary>
		/// Gets the size of this sub-object, if it's a helper.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 HelperSize
		{
			get
			{
				this.AssertInstance();

				return this.handle->helperSize;
			}
		}
		#endregion
		#region Construction
		internal StaticSubObject(IntPtr handle)
		{
			this.handle = (SubObject*)handle.ToPointer();
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(SubObject* obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetName(SubObject* obj, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetProperties(SubObject* obj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProperties(SubObject* obj, string props);
		#endregion
	}
}