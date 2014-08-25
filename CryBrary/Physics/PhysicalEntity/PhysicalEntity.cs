﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Utilities;
using CryEngine.Physics.Status;
using CryEngine.Native;

namespace CryEngine.Physics
{
	/// <summary>
	/// Physical entity present in the physics system.
	/// </summary>
	public class PhysicalEntity
	{
		#region Statics
		internal static PhysicalEntity TryGet(IntPtr IPhysicalEntityHandle)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (IPhysicalEntityHandle == IntPtr.Zero)
				throw new NullPointerException();
#endif

			var physicalEntity = PhysicalEntities.FirstOrDefault(x => x.Handle == IPhysicalEntityHandle);
			if (physicalEntity == null)
			{
				switch (NativePhysicsMethods.GetPhysicalEntityType(IPhysicalEntityHandle))
				{
					case PhysicalizationType.Static:
					case PhysicalizationType.Rigid:
					case PhysicalizationType.WheeledVehicle:
					case PhysicalizationType.Articulated:
					case PhysicalizationType.Soft:
					case PhysicalizationType.Rope:
						physicalEntity = new PhysicalEntity(IPhysicalEntityHandle);
						break;
					case PhysicalizationType.Living:
						physicalEntity = new PhysicalEntityLiving(IPhysicalEntityHandle);
						break;
					case PhysicalizationType.Particle:
						physicalEntity = new PhysicalEntityParticle(IPhysicalEntityHandle);
						break;
					case PhysicalizationType.Area:
						physicalEntity = new PhysicalEntityArea(IPhysicalEntityHandle);
						break;
				}

				if (physicalEntity != null)
					PhysicalEntities.Add(physicalEntity);
			}

			return physicalEntity;
		}

		private static List<PhysicalEntity> PhysicalEntities = new List<PhysicalEntity>();
		#endregion
		protected PhysicalEntity()
		{
		}

		protected PhysicalEntity(IntPtr physEntPtr)
		{
			Handle = physEntPtr;
		}

		public void Break(BreakageParameters breakageParams)
		{
			NativeEntityMethods.BreakIntoPieces(Owner.GetIEntity(), 0, 0, breakageParams);
		}

		public bool AddImpulse(Vector3 vImpulse, Vector3? angImpulse = null, Vector3? point = null)
		{
			var impulse = PhysicalEntityImpulseAction.Create();

			impulse.impulse = vImpulse;

			if (angImpulse != null)
				impulse.angImpulse = angImpulse.Value;
			if (point != null)
				impulse.point = point.Value;

			return NativePhysicsMethods.ActionImpulse(Handle, ref impulse);
		}

		/// <summary>
		/// Gets or sets entity velocity as set by the physics system.
		/// </summary>
		public Vector3 Velocity
		{
			get { return NativePhysicsMethods.GetVelocity(Handle); }
			set { NativePhysicsMethods.SetVelocity(Handle, value); }
		}

		/// <summary>
		/// Determines if this physical entity is in a sleeping state or not. (Will not be affected
		/// by gravity) Autoamtically wakes upon collision.
		/// </summary>
		public bool Resting
		{
			get { throw new NotImplementedException(); }
			set { NativePhysicsMethods.Sleep(Handle, value); }
		}

		public virtual PhysicalizationType Type
		{
			get { return NativePhysicsMethods.GetPhysicalEntityType(Handle); }
		}

		public LivingPhysicsStatus LivingStatus
		{
			get
			{
				var status = LivingPhysicsStatus.Create();

				NativePhysicsMethods.GetLivingEntityStatus(Handle, ref status);

				return status;
			}
		}

		public DynamicsPhysicsStatus DynamicsStatus
		{
			get
			{
				var status = DynamicsPhysicsStatus.Create();

				NativePhysicsMethods.GetDynamicsEntityStatus(Handle, ref status);

				return status;
			}
		}

		[CLSCompliant(false)]
		public bool GetFlags(ref PhysicalFlagsParameters flags)
		{
			return NativePhysicsMethods.GetFlagParams(Handle, ref flags);
		}

		[CLSCompliant(false)]
		public bool SetFlags(ref PhysicalFlagsParameters flags)
		{
			return NativePhysicsMethods.SetFlagParams(Handle, ref flags);
		}

		public bool GetSimulationParameters(ref PhysicalSimulationParameters flags)
		{
			return NativePhysicsMethods.GetSimulationParams(Handle, ref flags);
		}

		public bool SetSimulationParameters(ref PhysicalSimulationParameters flags)
		{
			return NativePhysicsMethods.SetSimulationParams(Handle, ref flags);
		}

		/// <summary>
		/// IPhysicalEntity *
		/// </summary>
		internal IntPtr Handle { get; set; }

		private EntityBase owner;
		public EntityBase Owner
		{
			get
			{
				if (owner == null)
				{
					var entityHandle = NativeEntityMethods.GetEntityFromPhysics(Handle);
					if (entityHandle == IntPtr.Zero)
						return null;

					owner = Entity.Get(entityHandle);
				}

				return owner;
			}
		}
	}
}