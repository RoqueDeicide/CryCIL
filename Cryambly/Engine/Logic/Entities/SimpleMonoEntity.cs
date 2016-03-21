using System;
using System.Linq;
using CryCil.Engine.Data;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents the most basic implementation of <see cref="MonoEntity"/>.
	/// </summary>
	public class SimpleMonoEntity : MonoEntity
	{
		#region Construction
		/// <summary>
		/// Initializes base properties of all objects that serve as abstraction layers between CryEngine
		/// entities and logic defined in CryCIL for them.
		/// </summary>
		/// <param name="handle">Pointer to the entity itself.</param>
		/// <param name="id">    Identifier of the entity.</param>
		public SimpleMonoEntity(CryEntity handle, EntityId id) : base(handle, id)
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
		/// When implemented in derived class updates logical state of this entity after most other stuff is
		/// updated.
		/// </summary>
		public override void PostUpdate()
		{
		}
		#endregion
	}
}