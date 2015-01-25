using System;
using System.Runtime.InteropServices;
using CryEngine.Entities;
using CryEngine.Mathematics;

namespace CryEngine.Logic.Entities
{
	public partial class GameObjectExtension
	{
	}
	/// <summary>
	/// Provides data to be used during the reload of the entity.
	/// </summary>
	public class EntityReloadEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the set of parameters that was used to spawn the entity.
		/// </summary>
		public EntitySpawnParameters Parameters { get; set; }
		/// <summary>
		/// When set to true, signals CryEngine to remove the entity during the reload.
		/// </summary>
		/// <remarks>
		/// <see cref="GameObjectExtension.Reloaded"/> won't occur if this property is set
		/// to true.
		/// </remarks>
		public bool DeleteOnReload { get; set; }
	}
	/// <summary>
	/// Provides data to be used during the post-reload of the entity or when it is
	/// spawned.
	/// </summary>
	public class EntitySpawnEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the set of parameters that was used to spawn the entity.
		/// </summary>
		public EntitySpawnParameters Parameters { get; set; }
	}
	/// <summary>
	/// Provides data to be used during the client side initialization of the entity.
	/// </summary>
	public class EntityClientInitializationEventArgs : EventArgs
	{
		/// <summary>
		/// Identifier of the client's channel.
		/// </summary>
		public int ChannelId { get; set; }
	}
	/// <summary>
	/// Provides data specific for a thinking events.
	/// </summary>
	public class EntityThinkingEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the object that describes the context of entity's thinking.
		/// </summary>
		public EntityUpdateContext Context { get; private set; }
		/// <summary>
		/// Gets the identifier of the slot that is updated.
		/// </summary>
		public int UpdateSlot { get; private set; }
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="context">   
		/// The object that describes the context of entity's thinking.
		/// </param>
		/// <param name="updateSlot">The identifier of the slot that is updated.</param>
		public EntityThinkingEventArgs(EntityUpdateContext context, int updateSlot)
		{
			this.Context = context;
			this.UpdateSlot = updateSlot;
		}
	}
	/// <summary>
	/// Provides extra data about entity movement.
	/// </summary>
	public class EntityMovementEventArgs : EventArgs
	{
		/// <summary>
		/// Gets flags that describe changes to the transformation of this entity.
		/// </summary>
		public EntityXFormFlags XFormChanges { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="xFormChanges">
		/// Flags that describe changes to the transformation of this entity.
		/// </param>
		public EntityMovementEventArgs(EntityXFormFlags xFormChanges)
		{
			this.XFormChanges = xFormChanges;
		}
	}
	/// <summary>
	/// Provides info when this entity times out.
	/// </summary>
	public class EntityTimerEventArgs : EventArgs
	{
		/// <summary>
		/// Gets timer object that handled the timing.
		/// </summary>
		public CryTimer Timer { get; private set; }
		/// <summary>
		/// Time in milliseconds.
		/// </summary>
		public int Time { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="timer">Timer object that handled the timing.</param>
		/// <param name="time"> Time in milliseconds.</param>
		public EntityTimerEventArgs(CryTimer timer, int time)
		{
			this.Timer = timer;
			this.Time = time;
		}
	}
	/// <summary>
	/// Encapsulates details about a script event.
	/// </summary>
	public unsafe class EntityScriptEventEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the name of the event.
		/// </summary>
		public string EventName { get; private set; }
		/// <summary>
		/// Value attached to the event.
		/// </summary>
		public object Value { get; private set; }
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="namePtr">  
		/// Pointer to ASCIIZ string that is a name of the event.
		/// </param>
		/// <param name="eventType">Type of the event.</param>
		/// <param name="valuePtr"> Pointer to the value of the event.</param>
		public EntityScriptEventEventArgs
			(IntPtr namePtr, EntityEventValueType eventType, IntPtr valuePtr)
		{
			this.EventName = Marshal.PtrToStringAnsi(namePtr);
			switch (eventType)
			{
				case EntityEventValueType.Int:
					this.Value = *((int*)valuePtr.ToPointer());
					break;
				case EntityEventValueType.Float:
					this.Value = *((float*)valuePtr.ToPointer());
					break;
				case EntityEventValueType.Bool:
					this.Value = *((bool*)valuePtr.ToPointer());
					break;
				case EntityEventValueType.Vector:
					this.Value = *((Vector3*)valuePtr.ToPointer());
					break;
				case EntityEventValueType.Entity:
					this.Value = *((EntityId*)valuePtr.ToPointer());
					break;
				case EntityEventValueType.String:
					this.Value = Marshal.PtrToStringAnsi(valuePtr);
					break;
				default:
					throw new ArgumentOutOfRangeException("eventType");
			}
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="value">    Value attached to the event.</param>
		public EntityScriptEventEventArgs(string eventName, object value)
		{
			this.EventName = eventName;
			this.Value = value;
		}
	}
	/// <summary>
	/// Represents extra data about events related to entities and areas.
	/// </summary>
	public class EntityAreaPositionEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the identifier of the entity that has triggered the event.
		/// </summary>
		public EntityId TriggeringEntityId { get; private set; }
		/// <summary>
		/// Gets the identifier of the area that has been triggered.
		/// </summary>
		public int AreaId { get; private set; }
		/// <summary>
		/// Gets the identifier of the entity associated with the area that has been
		/// triggered.
		/// </summary>
		public int AreaEntityId { get; private set; }
		/// <summary>
		/// If the event is <see cref="EntityEvent.MoveNearArea"/> determines how close
		/// the entity is to the area.
		/// </summary>
		public float FadeRatio { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="triggeringEntityId">
		/// The identifier of the entity that has triggered the event.
		/// </param>
		/// <param name="areaId">            
		/// The identifier of the area that has been triggered.
		/// </param>
		/// <param name="areaEntityId">      
		/// The identifier of the entity associated with the area that has been triggered.
		/// </param>
		public EntityAreaPositionEventArgs(EntityId triggeringEntityId, int areaId, int areaEntityId)
		{
			this.TriggeringEntityId = triggeringEntityId;
			this.AreaId = areaId;
			this.AreaEntityId = areaEntityId;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="triggeringEntityId">
		/// The identifier of the entity that has triggered the event.
		/// </param>
		/// <param name="areaId">            
		/// The identifier of the area that has been triggered.
		/// </param>
		/// <param name="areaEntityId">      
		/// The identifier of the entity associated with the area that has been triggered.
		/// </param>
		/// <param name="fade">              
		/// If the event is <see cref="EntityEvent.MoveNearArea"/> determines how close
		/// the entity is to the area.
		/// </param>
		public EntityAreaPositionEventArgs
			(EntityId triggeringEntityId, int areaId, int areaEntityId, float fade)
		{
			this.TriggeringEntityId = triggeringEntityId;
			this.AreaId = areaId;
			this.AreaEntityId = areaEntityId;
			this.FadeRatio = fade;
		}
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
	public class EntityMovementEventArgs : EventArgs
	{
	}
}