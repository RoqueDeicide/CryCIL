using System;
using System.Linq;
using CryCil.Engine.Data;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents the most basic implementation of <see cref="EntityExtension"/>.
	/// </summary>
	public class SimpleEntityExtension : EntityExtension
	{
		#region Interface
		/// <summary>
		/// When implemented in derived class, releases resources held by this extension.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether a hosting entity was released from native code.
		/// </param>
		public override void Dispose(bool invokedFromNativeCode)
		{
		}
		/// <summary>
		/// When implemented in derived class, performs preliminary initialization of this extension.
		/// </summary>
		public override void Initialize()
		{
		}
		/// <summary>
		/// When implemented in derived class, performs final initialization of this extension.
		/// </summary>
		public override void PostInitialize()
		{
		}
		/// <summary>
		/// Synchronizes the state of this extension with its representation in other place (e.g. a save
		/// game
		/// file) .
		/// </summary>
		/// <remarks>
		/// All extensions are synchronized after the hosting entity in the order they were added to the
		/// entity.
		/// </remarks>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronize(CrySync sync)
		{
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this extension.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this extension after most other
		/// stuff is updated.
		/// </summary>
		public override void PostUpdate()
		{
		}
		/// <summary>
		/// When overridden in derived class, performs operations that must be done when this extension is
		/// removed from the entity.
		/// </summary>
		/// <remarks>
		/// <see cref="EntityExtension.Host"/> property still returns the hosting entity during this
		/// method.
		/// </remarks>
		/// <param name="disposing">
		/// Indicates whether release was caused by the entity getting disposed of.
		/// </param>
		public override void Release(bool disposing)
		{
		}
		/// <summary>
		/// When overridden in derived class, performs operations that must be done when this extension is
		/// added to the entity.
		/// </summary>
		/// <remarks>
		/// <see cref="EntityExtension.Host"/> property already returns the hosting entity during this
		/// method.
		/// </remarks>
		public override void Bind()
		{
		}
		#endregion
	}
}