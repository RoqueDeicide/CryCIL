using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CryEngine.Initialization;
using CryEngine.Extensions;
using CryEngine.Native;

namespace CryEngine
{
    public abstract partial class Actor 
        : ActorBase
    {
        private void InternalFullSerialize(Serialization.CrySerialize serialize)
        {
            //var serialize = new Serialization.CrySerialize();
            //serialize.Handle = handle;

            FullSerialize(serialize);
        }

        private void InternalNetSerialize(Serialization.CrySerialize serialize, int aspect, byte profile, int flags)
        {
           // var serialize = new Serialization.CrySerialize();
            //serialize.Handle = handle;

            NetSerialize(serialize, aspect, profile, flags);
        }

        /// <summary>
        /// Sets / gets the current health of this actor.
        /// </summary>
        public override float Health { get; set; }
        /// <summary>
        /// Sets / gets the max health value for this actor.
        /// </summary>
        public override float MaxHealth { get; set; }

        public PrePhysicsUpdateMode PrePhysicsUpdateMode
        {
            set { GameObject.PrePhysicsUpdateMode = value; }
        }

        public bool ReceivePostUpdates
        {
            set { GameObject.QueryExtension(ClassName).ReceivePostUpdates = value; }
        }

        #region Callbacks

        #region Entity events
        /// <summary>
        /// Called after level has been loaded, is not called on serialization.
        /// Note that this is called prior to GameRules.OnClientConnect and OnClientEnteredGame!
        /// </summary>
        protected virtual void OnLevelLoaded() { }

        /// <summary>
        /// Called when resetting the state of the entity in Editor.
        /// </summary>
        /// <param name="enteringGame">true if currently entering gamemode, false if exiting.</param>
        protected virtual void OnEditorReset(bool enteringGame) { }

        /// <summary>
        /// Sent on entity collision.
        /// </summary>
        protected virtual void OnCollision(ColliderInfo source, ColliderInfo target, Vec3 hitPos, Vec3 contactNormal, float penetration, float radius) { }

        /// <summary>
        /// Called when game is started (games may start multiple times)
        /// </summary>
        protected virtual void OnStartGame() { }

        /// <summary>
        /// Called when the level is started.
        /// </summary>
        protected virtual void OnStartLevel() { }

        /// <summary>
        /// Sent when entity enters to the area proximity.
        /// </summary>
        /// <param name="entityId"></param>
        protected virtual void OnEnterArea(EntityId entityId, int areaId, EntityId areaEntityId) { }

        /// <summary>
        /// Sent when entity moves inside the area proximity.
        /// </summary>
        /// <param name="entityId"></param>
        protected virtual void OnMoveInsideArea(EntityId entityId, int areaId, EntityId areaEntityId) { }

        /// <summary>
        /// Sent when entity leaves the area proximity.
        /// </summary>
        /// <param name="entityId"></param>
        protected virtual void OnLeaveArea(EntityId entityId, int areaId, EntityId areaEntityId) { }

        protected virtual void OnEnterNearArea(EntityId entityId, int areaId, EntityId areaEntityId) { }
        protected virtual void OnLeaveNearArea(EntityId entityId, int areaId, EntityId areaEntityId) { }
        protected virtual void OnMoveNearArea(EntityId entityId, int areaId, EntityId areaEntityId, float fade) { }

        /// <summary>
        /// Called when the entities local or world transformation matrix changes. (Position / Rotation / Scale)
        /// </summary>
        protected virtual void OnMove(EntityMoveFlags moveFlags) { }

        /// <summary>
        /// Called whenever another entity has been linked to this entity.
        /// </summary>
        /// <param name="child"></param>
        protected virtual void OnAttach(EntityId child) { }
        /// <summary>
        /// Called whenever another entity has been unlinked from this entity.
        /// </summary>
        /// <param name="child"></param>
        protected virtual void OnDetach(EntityId child) { }
        /// <summary>
        /// Called whenever this entity is unliked from another entity.
        /// </summary>
        /// <param name="parent"></param>
        protected virtual void OnDetachThis(EntityId parent) { }

        protected virtual void OnPrePhysicsUpdate() { }

        /// <summary>
        /// Called when an animation event (placed on animations via the Character Editor) is encountered.
        /// </summary>
        /// <param name="animEvent"></param>
        [CLSCompliant(false)]
        protected virtual void OnAnimationEvent(AnimationEvent animEvent) { }
        #endregion

        /// <summary>
        /// Called to update the view associated to this actor.
        /// </summary>
        /// <param name="viewParams"></param>
        protected virtual void UpdateView(ref ViewParams viewParams) { }

        /// <summary>
        /// Called after updating the view associated to this actor.
        /// </summary>
        /// <param name="viewParams"></param>
        protected virtual void PostUpdateView(ref ViewParams viewParams) { }

        protected virtual void OnPostUpdate() { }

		[CLSCompliant(false)]
		protected virtual void FullSerialize(Serialization.ICrySerialize serialize) { }

		[CLSCompliant(false)]
		protected virtual void NetSerialize(Serialization.ICrySerialize serialize, int aspect, byte profile, int flags) { }

        protected virtual void PostSerialize() { }
        #endregion
    }
}
