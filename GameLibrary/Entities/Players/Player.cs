using System;
using System.Linq;
using CryCil.Engine.Data;
using CryCil.Engine.Logic;
using CryCil.Engine.Physics;
using GameLibrary.Entities.Players.Extensions;

namespace GameLibrary.Entities.Players
{
	/// <summary>
	/// Represents an entity that directly controlled by the player.
	/// </summary>
	public class Player : MonoNetEntity
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		public Player(CryEntity handle, EntityId id) : base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, releases resources held by this entity.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether this entity was released from native code.
		/// </param>
		public override void Dispose(bool invokedFromNativeCode)
		{
		}
		/// <summary>
		/// When implemented in derived class, performs preliminary initialization of this object.
		/// </summary>
		public override void Initialize()
		{
		}
		/// <summary>
		/// When implemented in derived class, performs final initialization of this object.
		/// </summary>
		public override void PostInitialize()
		{
			this.Extensions.Add<PlayerView>();
		}
		/// <summary>
		/// Synchronizes the state of this entity with its representation in other place (e.g. a save game
		/// file) .
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronize(CrySync sync)
		{
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this entity.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this entity after most other stuff
		/// is updated.
		/// </summary>
		public override void PostUpdate()
		{
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
		public override bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile, SnapshotFlags flags)
		{
			return true;
		}
		#endregion
		#region Utilities
		#endregion
	}
}