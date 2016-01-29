using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents a proxy that represents a proximity trigger.
	/// </summary>
	public struct CryEntityTriggerProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets or sets the bounding box (coordinates are in entity-space) that defines the bounds of the
		/// proximity trigger that is bound to this entity.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When entities enter/leave these bounds this entity (or one that was specified when
		/// <see cref="ForwardTo"/> was called) receives <see cref="MonoEntity.AreaEntered"/>/
		/// <see cref="MonoEntity.AreaLeft"/> events.
		/// </para>
		/// <para>You can disable the trigger by setting this property to the empty bounding box.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox TriggerBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetTriggerBounds(this.handle, out box);
				return box;
			}
			set
			{
				this.AssertInstance();

				SetTriggerBounds(this.handle, ref value);
			}
		}
		#endregion
		#region Construction
		internal CryEntityTriggerProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Forwards events <see cref="MonoEntity.AreaEntered"/>/ <see cref="MonoEntity.AreaLeft"/> caused
		/// by this trigger to the specified entity.
		/// </summary>
		/// <param name="id">Identifier of the entity to forward events to.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ForwardTo(EntityId id)
		{
			this.AssertInstance();

			ForwardEventsTo(this.handle, id);
		}
		/// <summary>
		/// Invalidates the state of this trigger and makes it redetect the entities inside.
		/// </summary>
		/// <remarks>
		/// Use it when enabling the trigger to make sure that entities that were in it, when it was
		/// inactive are properly registered.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Invalidate()
		{
			this.AssertInstance();

			InvalidateTrigger(this.handle);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTriggerBounds(IntPtr handle, ref BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetTriggerBounds(IntPtr handle, out BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ForwardEventsTo(IntPtr handle, EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvalidateTrigger(IntPtr handle);
		#endregion
	}
}