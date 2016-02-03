using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Defines a signature of methods that can handle simple events raised by objects that derive from
	/// <see cref="FlowNode"/>.
	/// </summary>
	/// <param name="sender">
	/// An object of type that derives from <see cref="FlowNode"/> that raised the event.
	/// </param>
	public delegate void FlowNodeSimpleEventHandler(FlowNode sender);
	/// <summary>
	/// Defines a signature of methods that can handle an event <see cref="FlowNode.EntityTargeted"/>
	/// raised by objects that derive from <see cref="FlowNode"/>.
	/// </summary>
	/// <param name="sender">
	/// An object of type that derives from <see cref="FlowNode"/> that raised the event.
	/// </param>
	/// <param name="id">    
	/// Identifier of the entity that is now targeted by the <paramref name="sender"/>. If equal to default
	/// value of <see cref="EntityId"/> then the entity is no longer being targeted.
	/// </param>
	public delegate void FlowNodeTargetEventHandler(FlowNode sender, EntityId id);
	public partial class FlowNode
	{
		#region Events
		/// <summary>
		/// Occurs when identifier of the entity that is associated with this node changes.
		/// </summary>
		public event FlowNodeTargetEventHandler EntityTargeted;
		/// <summary>
		/// Occurs when initialization of this node starts.
		/// </summary>
		public event FlowNodeSimpleEventHandler Initializing;
		/// <summary>
		/// Occurs when initialization of this node is over.
		/// </summary>
		public event FlowNodeSimpleEventHandler Initialized;
		/// <summary>
		/// Occurs when this node is suspended.
		/// </summary>
		public event FlowNodeSimpleEventHandler Suspended;
		/// <summary>
		/// Occurs when this node is no longer suspended.
		/// </summary>
		public event FlowNodeSimpleEventHandler Resumed;
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