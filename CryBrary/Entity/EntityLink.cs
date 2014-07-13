using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine
{
	public class EntityLink
	{
		#region Statics
		public static EntityLink Create(EntityBase parent, EntityBase slave, string linkName, Vec3? relativePos = null, Quat? relativeRot = null)
		{
			return new EntityLink(NativeEntityMethods.AddEntityLink(parent.GetIEntity(), linkName, slave.Id, slave.GUID, relativeRot ?? Quat.Identity, relativePos ?? Vec3.Zero), parent);
		}

		public static void RemoveAll(EntityBase parent)
		{
			NativeEntityMethods.RemoveAllEntityLinks(parent.GetIEntity());
		}
		#endregion

		internal EntityLink(IntPtr handle, EntityBase entity)
		{
			Handle = handle;
			Parent = entity;
		}

		public void Remove()
		{
			NativeEntityMethods.RemoveEntityLink(Parent.GetIEntity(), Handle);
		}

		/// <summary>
		/// Native IEntityLink handle
		/// </summary>
		internal IntPtr Handle { get; set; }

		public EntityBase Parent { get; private set; }
		public EntityBase Slave
		{
			get
			{
				var slaveId = NativeEntityMethods.GetEntityLinkTarget(Handle);
				if (slaveId == 0)
					return null;

				return Entity.Get(slaveId);
			}
			set { NativeEntityMethods.SetEntityLinkTarget(Handle, value.Id); }
		}

		public string Name { get { return NativeEntityMethods.GetEntityLinkName(Handle); } }

		public Quat RelativeRotation
		{
			get { return NativeEntityMethods.GetEntityLinkRelativeRotation(Handle); }
			set { NativeEntityMethods.SetEntityLinkRelativeRotation(Handle, value); }
		}

		public Vec3 RelativePosition
		{
			get { return NativeEntityMethods.GetEntityLinkRelativePosition(Handle); }
			set { NativeEntityMethods.SetEntityLinkRelativePosition(Handle, value); }
		}
	}
}