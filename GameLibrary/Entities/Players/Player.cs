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
	public class Player : SimpleNetEntity
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
		/// When implemented in derived class, performs final initialization of this object.
		/// </summary>
		public override void PostInitialize()
		{
			this.Extensions.Add<PlayerView>();
			this.Extensions.Add<PlayerInput>();
			this.Extensions.Add<PlayerMovement>();
		}
		#endregion
		#region Utilities
		#endregion
	}
}