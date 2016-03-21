using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Logic
{
	public partial struct CryEntity
	{
		/// <summary>
		/// Gets number of entities that are attached to this one as children.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public int ChildCount
		{
			get
			{
				this.AssertEntity();

				return GetChildCount(this.handle);
			}
		}
		/// <summary>
		/// Gets an entity this one is attached to as a child.
		/// </summary>
		/// <returns>A valid object if this entity has a parent, otherwise returns invalid one.</returns>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public CryEntity Parent
		{
			get
			{
				this.AssertEntity();

				return GetParent(this.handle);
			}
		}
		/// <summary>
		/// Gets the matrix that represents world-space transformation of the point this entity is using to
		/// attach to a parent entity.
		/// </summary>
		/// <remarks>
		/// Can yield a result that is different from <c>entity.Parent.WorldTransformationMatrix</c>, e.g.
		/// when attachment point is not a pivot point.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Matrix34 ParentAttachPointWorldTransformationMatrix
		{
			get
			{
				this.AssertEntity();

				return GetParentAttachPointWorldTM(this.handle);
			}
		}
		/// <summary>
		/// Determines whether this entity is attached to another and the point of attachment is represented
		/// by a valid 3x4 matrix.
		/// </summary>
		/// <remarks>
		/// Can return <c>false</c> when this entite e.g. is attached to a geometry cache node but geometry
		/// cache frame is not loaded yet.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool IsParentAttachmentValid
		{
			get
			{
				this.AssertEntity();

				return GetIsParentAttachmentValid(this.handle);
			}
		}
		/// <summary>
		/// Attaches another entity to this one.
		/// </summary>
		/// <param name="child"> Entity to attach to this one.</param>
		/// <param name="flags"> 
		/// A set of flags that specifies how entities will be attached to each other.
		/// </param>
		/// <param name="target">A name of GeomCache node and character bone to attach the child to.</param>
		/// <exception cref="ArgumentNullException">
		/// Cannot attach a null entity as a child to another.
		/// </exception>
		/// <exception cref="ArgumentException">Cannot attach the entity to itself.</exception>
		/// <exception cref="ArgumentException">Cannot create a recursive attachment.</exception>
		public void AttachChild(CryEntity child, EntityAttachmentFlags flags = 0, string target = null)
		{
			if (!child.IsValid)
			{
				throw new ArgumentNullException(nameof(child), "Cannot attach a null entity as a child to another.");
			}
			if (this.handle == child.handle)
			{
				throw new ArgumentException("Cannot attach the entity to itself.");
			}

			AttachChildInternal(this.handle, child.handle, flags, target);
		}
		/// <summary>
		/// Detaches all children from this entity.
		/// </summary>
		/// <param name="keepWorldTM">
		/// Indicates whether all child entities should keep their world transformation matrix.
		/// </param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void DetachAll(bool keepWorldTM = false)
		{
			this.AssertEntity();

			DetachAllInternal(this.handle, keepWorldTM);
		}
		/// <summary>
		/// Detaches this entity from its parent.
		/// </summary>
		/// <param name="keepWorldTM">
		/// Indicates whether this entity should keep its world transformation matrix.
		/// </param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void DetachThis(bool keepWorldTM = false)
		{
			this.AssertEntity();

			DetachThisInternal(this.handle, keepWorldTM);
		}
		/// <summary>
		/// Gets a child entity.
		/// </summary>
		/// <param name="index">Zero-based index of the entity to get.</param>
		/// <returns>An object that represents the child entity.</returns>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the child entity cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the child entity cannot be greater then total number of children.
		/// </exception>
		public CryEntity GetChild(int index)
		{
			this.AssertEntity();

			return GetChildInternal(this.handle, index);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AttachChildInternal(IntPtr handle, IntPtr child, EntityAttachmentFlags flags,
													   string target);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DetachAllInternal(IntPtr handle, bool keepWorldTM);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DetachThisInternal(IntPtr handle, bool keepWorldTM);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetChildCount(IntPtr handle);
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the child entity cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the child entity cannot be greater then total number of children.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		[SuppressMessage("ReSharper", "ExceptionNotThrown")]
		private static extern CryEntity GetChildInternal(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntity GetParent(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix34 GetParentAttachPointWorldTM(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsParentAttachmentValid(IntPtr handle);
	}
}