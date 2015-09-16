using System;
using System.Collections;
using System.Collections.Generic;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Marks methods that handle physics events immediately when they are raised.
	/// </summary>
	/// <remarks>Methods that handle immediate events must be thread-safe.</remarks>
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class ImmediatePhysicsEventHandlerAttribute : Attribute
	{
	}
	internal class SafeIterationList<T> : IList<T>
	{
		#region Fields
		private readonly List<T> items;
		private readonly List<T> itemsToAdd;
		private readonly List<T> itemsToRemove;
		private bool iterating;
		#endregion
		#region Properties
		public int Count
		{
			get { return this.items.Count; }
		}
		public bool IsReadOnly
		{
			get { return false; }
		}
		public T this[int index]
		{
			get
			{
				if (this.iterating)
				{
					throw new InvalidOperationException("Cannot access the collection before beginning the iteration.");
				}
				return this.items[index];
			}
			set
			{
				if (this.iterating)
				{
					throw new InvalidOperationException("Cannot access the collection before beginning the iteration.");
				}
				this.items[index] = value;
			}
		}
		#endregion
		#region Construction
		internal SafeIterationList(int capacity)
		{
			this.items = new List<T>(capacity);
			this.itemsToAdd = new List<T>(5);
			this.itemsToRemove = new List<T>(5);
			this.iterating = false;
		}
		#endregion
		#region Interface
		internal void BeginIteration()
		{
			this.iterating = true;
		}
		internal void EndIteration()
		{
			this.iterating = false;

			for (int i = this.itemsToRemove.Count - 1; i >= 0; i--)
			{
				this.items.Remove(this.itemsToRemove[i]);
				this.itemsToRemove.RemoveAt(i);
			}

			for (int i = this.itemsToAdd.Count - 1; i >= 0; i--)
			{
				this.items.Add(this.itemsToAdd[i]);
				this.itemsToAdd.RemoveAt(i);
			}
		}
		public IEnumerator<T> GetEnumerator()
		{
			this.BeginIteration();

			for (int i = 0; i < this.items.Count; i++)
			{
				yield return this.items[i];
			}

			this.EndIteration();
		}
		public void Add(T item)
		{
			if (this.iterating)
			{
				this.itemsToAdd.Add(item);
			}
			else
			{
				this.items.Add(item);
			}
		}
		public void Clear()
		{
			throw new NotSupportedException();
		}
		public bool Contains(T item)
		{
			throw new NotSupportedException();
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}
		public bool Remove(T item)
		{
			if (this.iterating)
			{
				this.itemsToRemove.Add(item);
			}
			else
			{
				return this.items.Remove(item);
			}
			return true;
		}
		public int IndexOf(T item)
		{
			throw new NotSupportedException();
		}
		public void Insert(int index, T item)
		{
			throw new NotSupportedException();
		}
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}
		#endregion
		#region Utilities
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}

	public partial class PhysicalWorld
	{
		#region Fields
		private static readonly SafeIterationList<BoundingBoxOverlapEventHandler> overlapHandlers =
			new SafeIterationList<BoundingBoxOverlapEventHandler>(50);
		private static readonly SafeIterationList<PhysicsCollisionEventHandler> collisionHandlers =
			new SafeIterationList<PhysicsCollisionEventHandler>(50);
		private static readonly SafeIterationList<PhysicalEntityStateChangeEventHandler> entityStateHandlers =
			new SafeIterationList<PhysicalEntityStateChangeEventHandler>(50);
		private static readonly SafeIterationList<PhysicsEnvironmentStateChangeEventHandler> envStateHandlers =
			new SafeIterationList<PhysicsEnvironmentStateChangeEventHandler>(50);
		private static readonly SafeIterationList<SimulationStepCompleteEventHandler> stepHandlers =
			new SafeIterationList<SimulationStepCompleteEventHandler>(50);
		private static readonly SafeIterationList<PhysicsMeshChangedEventHandler> meshHandlers =
			new SafeIterationList<PhysicsMeshChangedEventHandler>(50);
		private static readonly SafeIterationList<EntityPartCreatedEventHandler> createPartHandlers =
			new SafeIterationList<EntityPartCreatedEventHandler>(50);
		private static readonly SafeIterationList<EntityPartRevealedEventHandler> revealPartHandlers =
			new SafeIterationList<EntityPartRevealedEventHandler>(50);
		private static readonly SafeIterationList<PhysicsJointBrokenEventHandler> jointHandlers =
			new SafeIterationList<PhysicsJointBrokenEventHandler>(50);
		private static readonly SafeIterationList<PhysicalEntityDeletedEventHandler> deleteHandlers =
			new SafeIterationList<PhysicalEntityDeletedEventHandler>(50);
		private static readonly SafeIterationList<BoundingBoxOverlapEventHandler> overlapHandlersLogged =
			new SafeIterationList<BoundingBoxOverlapEventHandler>(50);
		private static readonly SafeIterationList<PhysicsCollisionEventHandler> collisionHandlersLogged =
			new SafeIterationList<PhysicsCollisionEventHandler>(50);
		private static readonly SafeIterationList<PhysicalEntityStateChangeEventHandler> entityStateHandlersLogged =
			new SafeIterationList<PhysicalEntityStateChangeEventHandler>(50);
		private static readonly SafeIterationList<PhysicsEnvironmentStateChangeEventHandler> envStateHandlersLogged =
			new SafeIterationList<PhysicsEnvironmentStateChangeEventHandler>(50);
		private static readonly SafeIterationList<SimulationStepCompleteEventHandler> stepHandlersLogged =
			new SafeIterationList<SimulationStepCompleteEventHandler>(50);
		private static readonly SafeIterationList<PhysicsMeshChangedEventHandler> meshHandlersLogged =
			new SafeIterationList<PhysicsMeshChangedEventHandler>(50);
		private static readonly SafeIterationList<EntityPartCreatedEventHandler> createPartHandlersLogged =
			new SafeIterationList<EntityPartCreatedEventHandler>(50);
		private static readonly SafeIterationList<EntityPartRevealedEventHandler> revealPartHandlersLogged =
			new SafeIterationList<EntityPartRevealedEventHandler>(50);
		private static readonly SafeIterationList<PhysicsJointBrokenEventHandler> jointHandlersLogged =
			new SafeIterationList<PhysicsJointBrokenEventHandler>(50);
		private static readonly SafeIterationList<PhysicalEntityDeletedEventHandler> deleteHandlersLogged =
			new SafeIterationList<PhysicalEntityDeletedEventHandler>(50);
		#endregion
		#region Properties
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the bounding box of one physical entity gets overlapped with another one's.
		/// </summary>
		public static event BoundingBoxOverlapEventHandler BoundingBoxOverlapped
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? overlapHandlersLogged : overlapHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? overlapHandlersLogged : overlapHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when one physical entity collides with another.
		/// </summary>
		public static event PhysicsCollisionEventHandler CollisionHappened
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? collisionHandlersLogged : collisionHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? collisionHandlersLogged : collisionHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when one of the physical entities changes its simulation class.
		/// </summary>
		public static event PhysicalEntityStateChangeEventHandler EntityStateChanged
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? entityStateHandlersLogged : entityStateHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? entityStateHandlersLogged : entityStateHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a part of the entity breaks off when near another entity.
		/// </summary>
		public static event PhysicsEnvironmentStateChangeEventHandler EnvironmentChanged
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? envStateHandlersLogged : envStateHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? envStateHandlersLogged : envStateHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a simulation step is complete for one of the physical entities.
		/// </summary>
		public static event SimulationStepCompleteEventHandler StepComplete
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? stepHandlersLogged : stepHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? stepHandlersLogged : stepHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a physical entity has its physical geometry changed.
		/// </summary>
		public static unsafe event PhysicsMeshChangedEventHandler MeshChanged
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? meshHandlersLogged : meshHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? meshHandlersLogged : meshHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a part breaks off a physical entity.
		/// </summary>
		public static event EntityPartCreatedEventHandler PartCreated
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? createPartHandlersLogged : createPartHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? createPartHandlersLogged : createPartHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a part of the physical entity that was hidden due to hierarchical breakability
		/// becomes revealed.
		/// </summary>
		public static event EntityPartRevealedEventHandler PartRevealed
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? revealPartHandlersLogged : revealPartHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? revealPartHandlersLogged : revealPartHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a structural joint is broken.
		/// </summary>
		public static event PhysicsJointBrokenEventHandler JointBroken
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? jointHandlersLogged : jointHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? jointHandlersLogged : jointHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		/// <summary>
		/// Occurs when a physical entity is deleted from physical world.
		/// </summary>
		public static event PhysicalEntityDeletedEventHandler EntityDeleted
		{
			add
			{
				var handlers = IsHandlerForLogged(value) ? deleteHandlersLogged : deleteHandlers;
				lock (handlers)
				{
					handlers.Add(value);
				}
			}
			remove
			{
				var handlers = IsHandlerForLogged(value) ? deleteHandlersLogged : deleteHandlers;
				lock (handlers)
				{
					handlers.Remove(value);
				}
			}
		}
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		private static bool IsHandlerForLogged(Delegate @delegate)
		{
			return !@delegate.Method.ContainsAttribute<ImmediatePhysicsEventHandlerAttribute>();
		}

		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnBoundingBoxOverlapped(ref StereoPhysicsEventData entities, bool logged)
		{
			var handlers = logged ? overlapHandlersLogged : overlapHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entities))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnCollisionHappened(ref StereoPhysicsEventData entities, ref CollisionInfo collision,
												ref CollisionParticipantInfo collider,
												ref CollisionParticipantInfo collidee, bool logged)
		{
			var handlers = logged ? collisionHandlersLogged : collisionHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entities, ref collision, ref collider, ref collidee))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnEntityStateChanged(ref MonoPhysicsEventData entity, ref PhysicalEntityStateInfo oldState,
												 ref PhysicalEntityStateInfo newState, float idleTime, bool logged)
		{
			var handlers = logged ? entityStateHandlersLogged : entityStateHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, ref oldState, ref newState, idleTime))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnEnvironmentChanged(ref MonoPhysicsEventData entity, PhysicalEntity brokenEntity,
												 PhysicalEntity brokedOffEntity, bool logged)
		{
			var handlers = logged ? envStateHandlersLogged : envStateHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, brokenEntity, brokedOffEntity))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnStepComplete(ref MonoPhysicsEventData entity, ref TimeStepInfo stepInfo, bool logged)
		{
			var handlers = logged ? stepHandlersLogged : stepHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, ref stepInfo))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static unsafe bool OnMeshChanged(ref MonoPhysicsEventData entity, int partId, bool invalid,
												 PhysicsMeshUpdateReason reason, GeometryShape mesh,
												 MeshUpdate* lastUpdate, ref Matrix34 skeletonToMesh,
												 GeometryShape skeletonMesh, bool logged)
		{
			var handlers = logged ? meshHandlersLogged : meshHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, partId, invalid, reason, mesh, lastUpdate, ref skeletonToMesh,
									 skeletonMesh))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnPartCreated(ref MonoPhysicsEventData entity, ref CreatedPartInfo partInfo, bool logged)
		{
			var handlers = logged ? createPartHandlersLogged : createPartHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, ref partInfo))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnPartRevealed(ref MonoPhysicsEventData entity, int partId, bool logged)
		{
			var handlers = logged ? revealPartHandlersLogged : revealPartHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, partId))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnJointBroken(ref StereoPhysicsEventData entities, ref JointBreakInfo info, bool logged)
		{
			var handlers = logged ? jointHandlersLogged : jointHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entities, ref info))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		[UnmanagedThunk("Raises one of the physical events.")]
		private static bool OnEntityDeleted(ref MonoPhysicsEventData entity, PhysicalEntityRemovalMode mode, bool logged)
		{
			var handlers = logged ? deleteHandlersLogged : deleteHandlers;
			bool proceed = true;
			lock (handlers)
			{
				handlers.BeginIteration();

				int count = handlers.Count;
				for (int i = 0; i < count; i++)
				{
					if (!handlers[i](ref entity, mode))
					{
						proceed = false;
						break;
					}
				}

				handlers.EndIteration();
			}
			return proceed;
		}
		#endregion
	}
}