using System;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine.Entities
{
	/// <summary>
	/// Represents a link between two entities.
	/// </summary>
	public class EntityLink
	{
		#region Statics
		/// <summary>
		/// Creates a link between two entities.
		/// </summary>
		/// <param name="parent">
		/// Parent entity. Changes to parent entity's rotation and position affect <paramref
		/// name="child" />.
		/// </param>
		/// <param name="child">
		/// Slave entity. Changes to slave entity's rotation and position do not affect <paramref
		/// name="parent" />.
		/// </param>
		/// <param name="linkName">Name of the link.</param>
		/// <param name="relativePos">
		/// Relative position of <paramref name="child" /> entity relative to a <paramref
		/// name="parent" />.
		/// </param>
		/// <param name="relativeRot">
		/// Relative orientation of <paramref name="child" /> entity relative to a <paramref
		/// name="parent" />.
		/// </param>
		/// <returns>Wrapper object for a link.</returns>
		public static EntityLink Create(EntityBase parent, EntityBase child, string linkName, Vector3? relativePos = null,
										Quaternion? relativeRot = null)
		{
			return
				new EntityLink(
					EntityInterop.AddEntityLink(parent.GetIEntity(), linkName, child.Id, child.GUID), parent);
		}
		/// <summary>
		/// Unlinks everything from the entity.
		/// </summary>
		/// <param name="parent">Entity to unlink everything from.</param>
		public static void RemoveAll(EntityBase parent)
		{
			EntityInterop.RemoveAllEntityLinks(parent.GetIEntity());
		}
		#endregion
		#region Properties
		/// <summary>
		/// Native IEntityLink handle
		/// </summary>
		internal IntPtr Handle { get; set; }
		/// <summary>
		/// Parent entity.
		/// </summary>
		/// <remarks>
		/// Parent entity's movement can affect child entities but not vise versa.
		/// </remarks>
		public EntityBase Parent { get; private set; }
		/// <summary>
		/// Gets or sets a child entity <see cref="Parent" /> is attached to.
		/// </summary>
		public EntityBase Child
		{
			get
			{
				var slaveId = EntityInterop.GetEntityLinkTarget(this.Handle);
				return slaveId == 0 ? null : Entity.Get(slaveId);
			}
			set { EntityInterop.SetEntityLinkTarget(this.Handle, value.Id); }
		}
		/// <summary>
		/// Gets the name of the link.
		/// </summary>
		public string Name
		{
			get { return EntityInterop.GetEntityLinkName(this.Handle); }
		}
		#endregion
		#region Construction
		internal EntityLink(IntPtr handle, EntityBase entity)
		{
			this.Handle = handle;
			this.Parent = entity;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Destroys this link.
		/// </summary>
		public void Remove()
		{
			EntityInterop.RemoveEntityLink(this.Parent.GetIEntity(), this.Handle);
		}
		#endregion
	}
}