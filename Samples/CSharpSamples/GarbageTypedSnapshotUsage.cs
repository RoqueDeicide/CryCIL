using System;
using System.Linq;
using CryCil;
using CryCil.Engine.Data;
using CryCil.Engine.Logic;
using CryCil.Engine.Logic.EntityProxies;
using CryCil.Engine.Physics;
using CryCil.Geometry;

namespace CSharpSamples
{
	//[Entity("Sample", false)]
	public class GarbageTypedSnapshotUsage : MonoNetEntity
	{
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		public GarbageTypedSnapshotUsage(CryEntity handle, EntityId id) : base(handle, id)
		{
		}
		/// <summary>
		/// When implemented in derived class, releases resources held by this entity.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether this entity was released from native code.
		/// </param>
		public override void Dispose(bool invokedFromNativeCode)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// When implemented in derived class, performs preliminary initialization of this object.
		/// </summary>
		public override void Initialize()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// When implemented in derived class, performs final initialization of this object.
		/// </summary>
		public override void PostInitialize()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Synchronizes the state of this entity with its representation in other place (e.g. a save game
		/// file) .
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronize(CrySync sync)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this entity.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this entity after most other stuff
		/// is updated.
		/// </summary>
		public override void PostUpdate()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Synchronizes the state of this entity with its representatives on other machines over network.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates the aspect that requires synchronization.</param>
		/// <param name="profile">
		/// A number in range [0; 7] that specifies the data format that has to be used to synchronize the
		/// aspect data.
		/// </param>
		/// <param name="flags">  A set of flags that specify how to write the snapshot.</param>
		/// <returns>True, if synchronization was successful.</returns>
		public override bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile,
													SnapshotFlags flags)
		{
			// This is a snippet of code ported from Projectile.cpp from GameSDK project.
			if (aspect == EntityAspects.Physics)
			{
				CryEntity entity = this.Entity;

				PhysicalEntityType type = PhysicalEntityType.None;
				switch ((PhysicalEntityType)profile)
				{
					case PhysicalEntityType.None:
						return true;
					case PhysicalEntityType.Static:
						Vector3 position = entity.WorldPosition;
						Quaternion orientation = entity.WorldOrientation;

						sync.Sync("pos", ref position);
						sync.Sync("ori", ref orientation);

						if (sync.Reading)
						{
							entity.WorldTransformation = new Matrix34(new Vector3(1), orientation, position);
						}
						break;
					case PhysicalEntityType.Rigid:
						type = PhysicalEntityType.Rigid;
						break;
					case PhysicalEntityType.Particle:
						type = PhysicalEntityType.Particle;
						break;
					default:
						return false;
				}

				CryEntityPhysicalProxy physicalProxy = entity.Proxies.Physics;
				if (sync.Writing)
				{
					if (!physicalProxy.IsValid ||
						!physicalProxy.Physics.IsValid ||
						physicalProxy.Physics.Type != type)
					{
						PhysicalWorld.WriteGarbageTypedSnapshot(sync, type);
					}
				}
				else if (!physicalProxy.IsValid)
				{
					return false;
				}

				physicalProxy.SynchronizeTyped(sync, type, flags);
			}

			return true;
		}
	}
}