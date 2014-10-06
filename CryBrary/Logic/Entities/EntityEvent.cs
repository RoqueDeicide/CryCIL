using System;
using CryEngine.Engine.Renderer;
using CryEngine.Physics;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Enumeration of entity events.
	/// </summary>
	public enum EntityEvent
	{
		/// <summary>
		/// This event occurs when the entity local or world transformation matrix change
		/// (position/rotation/scale).
		/// </summary>
		/// <remarks>
		/// <para>Integer 1) <see cref="EntityXFormFlags"/> value that describes why the change has happened.</para>
		/// <para>Integer 2) If this version supports segmented worlds, indicates whether physical bounds of the entity need to be recalculated.</para>
		/// </remarks>
		XForm = 0,
		/// <summary>
		/// This event occurs when the entity is moved/scaled/rotated in the editor.
		/// </summary>
		/// <remarks>This event only occurs when the user releases a button.</remarks>
		XformFinishedEditor,
		/// <summary>
		/// This event occurs when entity's timer expires.
		/// </summary>
		/// <remarks>
		/// <para>Integer 1) An identifier of Timer object.</para>
		/// <para>Integer 2) A time elapsed in milliseconds.</para>
		/// <para>Integer 3) An identifier of the entity.</para>
		/// </remarks>
		Timer,
		/// <summary>
		/// This event occurs when entity that cannot be removed respawns.
		/// </summary>
		Initialize,
		/// <summary>
		/// This event occurs when this entity is about to be removed.
		/// </summary>
		Done,
		/// <summary>
		/// This event occurs when the pool entity is returned to the pool.
		/// </summary>
		ReturningToPool,
		/// <summary>
		/// This event occurs when this entity becomes visible or invisible.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter = 0 if entity has disappeared from the view, = 1
		/// otherwise.
		/// </para>
		/// <para>
		/// This event never occurs, since there is no code in CryEntitySystem that does
		/// that.
		/// </para>
		/// </remarks>
		Visiblity,
		/// <summary>
		/// This event occurs when the state of this entity is reset.
		/// </summary>
		/// <remarks>
		/// <para>Used with the Editor.</para>
		/// <para>
		/// First integer parameter indicates whether this entity was reset when the
		/// Editor user has switched to game mode.
		/// </para>
		/// </remarks>
		Reset,
		/// <summary>
		/// This event occurs when another entity is attached to this one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is an identifier of the child entity.
		/// </remarks>
		Attach,
		/// <summary>
		/// This event occurs when this entity is attached to another one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is an identifier of the parent entity.
		/// </remarks>
		AttachThis,
		/// <summary>
		/// This event occurs when another entity is detached from this one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is an identifier of the child entity.
		/// </remarks>
		Detach,
		/// <summary>
		/// This event occurs when this entity is detached from another one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is an identifier of the parent entity.
		/// </remarks>
		DetachThis,
		/// <summary>
		/// This event occurs when another entity is linked to this one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a handle of IEntityLink structure.
		/// </remarks>
		Link,
		/// <summary>
		/// This event occurs when another entity is unlinked from this one.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a handle of IEntityLink structure.
		/// </remarks>
		Delink,
		/// <summary>
		/// This event occurs when this entity must be hidden.
		/// </summary>
		Hide,
		/// <summary>
		/// This event occurs when this entity must be revealed.
		/// </summary>
		Unhide,
		/// <summary>
		/// This event occurs when physics of this entity should (not) be processed.
		/// </summary>
		/// <remarks>
		/// First integer parameter indicates whether physics should be enabled/disabled.
		/// </remarks>
		EnablePhysics,
		/// <summary>
		/// This event occurs when physics of this entity falls asleep/ awakens.
		/// </summary>
		/// <remarks>
		/// First integer parameter indicates whether physics awakes or falls asleep.
		/// </remarks>
		PhysicsChangeState,
		/// <summary>
		/// This event occurs when Lua script associated with this entity broadcasts its
		/// events.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is a pointer to ASCIIZ string with the name of the
		/// script event.
		/// </para>
		/// <para>
		/// Second integer parameter is a <see cref="EntityEventValueType"/> value.
		/// </para>
		/// <para>Third integer parameter is a pointer to the value.</para>
		/// </remarks>
		ScriptEvent,
		/// <summary>
		/// This event occurs when the triggering entity enters the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is an identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Second integer parameter is an identifier of the area.</para>
		/// <para>Third integer parameter is an identifier of the area entity.</para>
		/// <para>This event is sent to all entities targeted by the area.</para>
		/// </remarks>
		EnterArea,
		/// <summary>
		/// This event occurs when triggering entity leaves the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is an identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Second integer parameter is an identifier of the area.</para>
		/// <para>Third integer parameter is an identifier of the area entity.</para>
		/// <para>This event is sent to all entities targeted by the area.</para>
		/// </remarks>
		LeaveArea,
		/// <summary>
		/// This event occurs when triggering entity is near the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is an identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Second integer parameter is an identifier of the area.</para>
		/// <para>Third integer parameter is an identifier of the area entity.</para>
		/// <para>This event is sent to all entities targeted by the area.</para>
		/// </remarks>
		EnterNearArea,
		/// <summary>
		/// This event occurs when triggering entity is no longer near the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is an identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Second integer parameter is an identifier of the area.</para>
		/// <para>Third integer parameter is an identifier of the area entity.</para>
		/// </remarks>
		LeaveNearArea,
		/// <summary>
		/// This event occurs when triggering entity moves inside the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is an identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Second integer parameter is an identifier of the area.</para>
		/// <para>Third integer parameter is an identifier of the area entity.</para>
		/// </remarks>
		MoveInsideArea,
		/// <summary>
		/// This event occurs when triggering entity moves near the area.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Integer 1) An identifier of the entity that represents a
		/// triggering entity.
		/// </para>
		/// <para>Integer 2) An identifier of the area.</para>
		/// <para>Integer 3) An identifier of the area entity.</para>
		/// <para>
		/// <see cref="Single"/> 1) An indicator of how close the entity is to the
		/// area. (1.0 - on the border, 0.0 on the border of detection).
		/// </para>
		/// </remarks>
		MoveNearArea,
		/// <summary>
		/// This event occurs when triggering entity crosses a group of active areas.
		/// </summary>
		/// <remarks>
		/// <para>Integer 1) Identifier of the triggering entity.</para>
		/// <para>This event is sent to all entities targeted by the area.</para></remarks>
		CrossArea,
		/// <summary>
		/// This event occurs when pef_monitor_poststep receives a post-step notification.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This event occurs within a different thread, don't handle, if you don't know
		/// what you're doing.
		/// </para>
		/// <para>First floating point parameter is time interval.</para>
		/// </remarks>
		PhysicsPostStep,
		/// <summary>
		/// This event occurs when this entity is a breakable object that is broken.
		/// </summary>
		PhysicsBreak,
		/// <summary>
		/// This event occurs when AI object attached to this entity has finished
		/// executing current order/action.
		/// </summary>
		AiDone,
		/// <summary>
		/// This event occurs when a sound attached to this entity finished or was stopped
		/// playing.
		/// </summary>
		SoundDone,
		/// <summary>
		/// This event occurs when has not been rendered for a while.
		/// </summary>
		/// <remarks>
		/// <para>Timeout is specified by a console variable "es_not_seen_timeout".</para>
		/// <para>
		/// This event only occurs if this entity is subscribed to such an event.
		/// </para>
		/// </remarks>
		NotSeenTimeout,
		/// <summary>
		/// This event occurs when this entity collides with something.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a pointer <see cref="CryCollision"/> object that
		/// describes the collision.
		/// </remarks>
		Collision,
		/// <summary>
		/// This event occurs when this entity is rendered.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is a pointer to the current rendering
		/// <see cref="RenderParams"/> structure.
		/// </para>
		/// <para>
		/// This event only occurs if this entity is subscribed to such an event.
		/// </para>
		/// </remarks>
		Render,
		/// <summary>
		/// This event occurs when pre-physics update is done.
		/// </summary>
		/// <remarks>First floating point parameter is the frame time.</remarks>
		PrePhysicsUpdate,
		/// <summary>
		/// This event occurs when level loading is complete.
		/// </summary>
		LevelLoaded,
		/// <summary>
		/// This event occurs when the level starts.
		/// </summary>
		StartLevel,
		/// <summary>
		/// This event occurs when the game starts (can happen multiple times).
		/// </summary>
		StartGame,
		/// <summary>
		/// This event occurs when the entity enters a script state.
		/// </summary>
		EnterScriptState,
		/// <summary>
		/// This event occurs when the entity leaves a script state.
		/// </summary>
		LeaveScriptState,
		/// <summary>
		/// This event occurs before the game is synchronized with the file.
		/// </summary>
		PreSerialize,
		/// <summary>
		/// This event occurs after the game is synchronized with the file.
		/// </summary>
		PostSerialize,
		/// <summary>
		/// This event occurs when this entity becomes invisible.
		/// </summary>
		Invisible,
		/// <summary>
		/// This event occurs when this entity becomes visible.
		/// </summary>
		Visible,
		/// <summary>
		/// This event occurs when the entity switches to the different material.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a pointer the new material in CryEngine memory.
		/// </remarks>
		Material,
		/// <summary>
		/// This event occurs when the entity's material layer mask changes.
		/// </summary>
		/// <remarks>
		/// <para>Integer 1) <see cref="Byte"/> that represents new material layer.</para>
		/// <para>Integer 2) <see cref="Byte"/> that represents old material layer.</para>
		/// </remarks>
		MaterialLayer,
		/// <summary>
		/// This event occurs when this entity is hit by a weapon.
		/// </summary>
		/// <remarks>
		/// This event never actually occurs.
		/// </remarks>
		OnHit,
		/// <summary>
		/// This event occurs when an animation event (placed on animations in editor) is
		/// encountered.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a pointer the AnimEventInstance* object.
		/// </remarks>
		AnimationEvent,
		/// <summary>
		/// This event occurs when Lua script attached to the entity requests to change
		/// collider mode.
		/// </summary>
		/// <remarks>
		/// First integer parameter is a new <see cref="ColliderMode"/> value.
		/// </remarks>
		ScriptRequestColliderMode,
		/// <summary>
		/// This event occurs when output port of the flow node that is connected to the
		/// entity activates.
		/// </summary>
		/// <remarks>
		/// <para>
		/// First integer parameter is a pointer to null-terminated string that is a name
		/// of the output port.
		/// </para>
		/// <para>
		/// Second integer parameter is <see cref="EntityEventValueType"/> value.
		/// </para>
		/// <para>Third integer parameter is a pointer to the value.</para>
		/// </remarks>
		ActiveFlowNodeOutput,
		/// <summary>
		/// This event occurs when one of the entity's editor properties is changed on the
		/// selected entity.
		/// </summary>
		EditorPropertyChanged,
		/// <summary>
		/// This event occurs when a Lua script is reloaded in the editor.
		/// </summary>
		ReloadScript,
		/// <summary>
		/// This event occurs when this entity is activated.
		/// </summary>
		Activated,
		/// <summary>
		/// This event occurs when this entity is deactivated.
		/// </summary>
		Deactivated,
		/// <summary>
		/// Last entry. Not used, other then for going through this enumeration with the
		/// loop.
		/// </summary>
		Last
	}
}