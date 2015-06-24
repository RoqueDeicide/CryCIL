using System;

namespace CryCil.Engine.Logic
{
	public partial class FlowNode
	{
		#region Events
		/// <summary>
		/// Occurs when identifier of the entity that is associated with this node changes.
		/// </summary>
		public event Action<FlowNode, EntityId> EntityTargeted;
		/// <summary>
		/// Occurs when initialization of this node starts.
		/// </summary>
		public event Action<FlowNode> Initializing;
		/// <summary>
		/// Occurs when initialization of this node is over.
		/// </summary>
		public event Action<FlowNode> Initialized;
		/// <summary>
		/// Occurs when this node is suspended.
		/// </summary>
		public event Action<FlowNode> Suspended;
		/// <summary>
		/// Occurs when this node is no longer suspended.
		/// </summary>
		public event Action<FlowNode> Resumed;
		#endregion
		#region Raisers
		/// <summary>
		/// Raises the event <see cref="EntityTargeted"/>.
		/// </summary>
		/// <param name="obj">Identifier of the target entity.</param>
		protected virtual void OnEntityTargeted(EntityId obj)
		{
			if (this.EntityTargeted != null) this.EntityTargeted(this, obj);
		}
		/// <summary>
		/// Raises the event <see cref="Initializing"/>.
		/// </summary>
		protected virtual void OnInitializing()
		{
			if (this.Initializing != null) this.Initializing(this);
		}
		/// <summary>
		/// Raises the event <see cref="Initialized"/>.
		/// </summary>
		protected virtual void OnInitialized()
		{
			if (this.Initialized != null) this.Initialized(this);
		}
		/// <summary>
		/// Raises the event <see cref="Suspended"/>.
		/// </summary>
		protected virtual void OnSuspended()
		{
			if (this.Suspended != null) this.Suspended(this);
		}
		/// <summary>
		/// Raises the event <see cref="Resumed"/>.
		/// </summary>
		protected virtual void OnResumed()
		{
			if (this.Resumed != null) this.Resumed(this);
		}
		#endregion
	}
}