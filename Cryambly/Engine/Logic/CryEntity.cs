using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Engine.Rendering.Nodes;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a wrapper object for CryEngine entities.
	/// </summary>
	public partial struct CryEntity : IForeignDataProvider
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the pointer to the ILevel object that can be used as foreign data.
		/// </summary>
		public ForeignData ForeignData
		{
			get { return new ForeignData(this); }
		}
		/// <summary>
		/// Assigns foreign data to this object.
		/// </summary>
		/// <param name="handle">Foreign data.</param>
		public void SetForeignData(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Gets the identifier of foreign data type for this type.
		/// </summary>
		public ForeignDataIds ForeignDataId
		{
			get { return ForeignDataIds.Entity; }
		}
		/// <summary>
		/// Gets the pointer to the underlying object.
		/// </summary>
		public IntPtr Handle
		{
			get { return this.handle; }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets or sets flags that describe this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public EntityFlags Flags
		{
			get
			{
				this.AssertEntity();

				return (EntityFlags)GetFlags(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetFlags(this.handle, (ulong)value);
			}
		}
		/// <summary>
		/// Indicates whether this entity is scheduled to be deleted on the next frame.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool IsGarbage
		{
			get
			{
				this.AssertEntity();

				return GetIsGarbage(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the name of this entity.
		/// </summary>
		/// <remarks>The name is not required to be unique.</remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="ArgumentNullException">Name of the entity cannot be null.</exception>
		public string Name
		{
			get
			{
				this.AssertEntity();

				return GetNameInternal(this.handle);
			}
			set
			{
				this.AssertEntity();
				if (value == null)
				{
					throw new ArgumentNullException("value", "Name of the entity cannot be null.");
				}

				SetNameInternal(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this entity was created by the entity load manager using information stored
		/// in the level file.
		/// </summary>
		/// <remarks>Returns <c>false</c> for any entities created dynamically through code.</remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool IsLoadedFromLevelFile
		{
			get
			{
				this.AssertEntity();

				return GetIsLoadedFromLevelFile(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this entity is a part of the entity pool.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool IsFromPool
		{
			get
			{
				this.AssertEntity();

				return GetIsFromPool(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is considered active.
		/// </summary>
		/// <remarks>
		/// <para>Active entities are updated by the engine every frame.</para>
		/// <para>
		/// Make sure to add this instruction: <c>this.Entity.Active = true;</c> in
		/// <see cref="MonoEntity.PostInitialize"/> if you rely on any custom functionality defined in
		/// <see cref="MonoEntity.Update"/> and <see cref="MonoEntity.PostUpdate"/>.
		/// </para>
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool Active
		{
			get
			{
				this.AssertEntity();

				return IsActive(this.handle);
			}
			set
			{
				this.AssertEntity();

				Activate(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is going to receive an extra update
		/// just before the update of the physics engine.
		/// </summary>
		/// <remarks>
		/// Make sure to add this instruction: <c>this.Entity.ReceivesPrePhysicsUpdates = true;</c> in
		/// <see cref="MonoEntity.PostInitialize"/> if your entity is going to have its own physical
		/// movement logic.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool ReceivesPrePhysicsUpdates
		{
			get
			{
				this.AssertEntity();

				return IsPrePhysicsActive(this.handle);
			}
			set
			{
				this.AssertEntity();

				PrePhysicsActivate(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is hidden.
		/// </summary>
		/// <remarks>
		/// Hidden entities don't receive updates unless they have <see cref="EntityFlags.UpdateHidden"/>
		/// flag set, are not rendered and their physics is disabled.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool Hidden
		{
			get
			{
				this.AssertEntity();

				return IsHidden(this.handle);
			}
			set
			{
				this.AssertEntity();

				Hide(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is invisible.
		/// </summary>
		/// <remarks>
		/// Hidden entities don't receive updates, are not rendered and their physics is disabled.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool Invisible
		{
			get
			{
				this.AssertEntity();

				return IsInvisible(this.handle);
			}
			set
			{
				this.AssertEntity();

				MakeInvisible(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets entity's automatic (de-)activation policy.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public EntityUpdatePolicy UpdatePolicy
		{
			get
			{
				this.AssertEntity();

				return GetUpdatePolicy(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetUpdatePolicy(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the pointer to the material that this entity is using.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Material Material
		{
			get
			{
				this.AssertEntity();

				return GetMaterial(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetMaterial(this.handle, value);
			}
		}
		/// <summary>
		/// Gets an object that provides access to slots of this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public EntitySlotCollection Slots
		{
			get
			{
				this.AssertEntity();

				return new EntitySlotCollection(this.handle);
			}
		}
		/// <summary>
		/// Gets the first in a linked list of entity links.
		/// </summary>
		/// <returns>
		/// If this entity is linked to at least one other entity, a valid object of type
		/// <see cref="EntityLink"/> will be returned, otherwise an invalid one will be returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public EntityLink Links
		{
			get
			{
				this.AssertEntity();

				return GetEntityLinks(this.handle);
			}
		}
		/// <summary>
		/// Gets the physical entity that represents this entity in physical world.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public PhysicalEntity Physics
		{
			get
			{
				this.AssertEntity();

				return GetPhysics(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that represents this entity in the render world.
		/// </summary>
		public CryRenderNode RenderNode
		{
			get
			{
				this.AssertEntity();

				return GetRenderNode(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryEntity(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Marks a specified set of flags as set for this entity.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		/// entity.Flags |= someFlags;
		/// </code>
		/// This function is faster because it involves only one internal call and only one validation
		/// check.
		/// </remarks>
		/// <param name="flagsToAdd">Flags to set.</param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void AddFlags(EntityFlags flagsToAdd)
		{
			this.AssertEntity();

			if (flagsToAdd == 0)
			{
				return;
			}

			AddFlagsInternal(this.handle, (ulong)flagsToAdd);
		}
		/// <summary>
		/// Removes flags from the current set of entity flags.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		///  entity.Flags &amp;= ~someFlags;
		/// </code>
		/// This function is faster because it involves only one internal call and only one validation
		/// check.
		/// </remarks>
		/// <param name="flagsToClear">Combination of bit flags to remove.</param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void ClearFlags(EntityFlags flagsToClear)
		{
			this.AssertEntity();

			if (flagsToClear == 0)
			{
				return;
			}

			ClearFlagsInternal(this.handle, (ulong)flagsToClear);
		}
		/// <summary>
		/// Checks if specified set of flags is enabled.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		/// entity.Flags.HasFlag(someFlags);
		/// </code>This function is faster because it doesn't involve boxing.
		/// </remarks>
		/// <param name="flagsToCheck">A combination of flags to check.</param>
		/// <param name="all">         
		/// Indicates whether all specified flags must be check in order for this method to return
		/// <c>true</c>.
		/// </param>
		/// <returns>True, if all specified flags are checked.</returns>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public bool CheckFlags(EntityFlags flagsToCheck, bool all = true)
		{
			this.AssertEntity();

			return flagsToCheck == 0 || CheckFlagsInternal(this.handle, (ulong)flagsToCheck, all);
		}
		/// <summary>
		/// Starts a timer.
		/// </summary>
		/// <remarks>
		/// <para>
		/// All timers are specific to entities, so timer identifiers don't have to be globally unique.
		/// </para>
		/// <para>
		/// When timer is set using an identifier of a timer that is still active, that timer will be
		/// overridden without raising a Time-Out event.
		/// </para>
		/// <para>
		/// For native entities ENTITY_EVENT_TIME will be sent, while for Mono entities
		/// <see cref="MonoEntity.TimedOut"/> event is raised.
		/// </para>
		/// </remarks>
		/// <param name="timerId">Identifier of the timer to start.</param>
		/// <param name="time">   Amount of time the timer should be active for.</param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Identifier of the timer cannot be less then 0.
		/// </exception>
		public void SetTimer(int timerId, TimeSpan time)
		{
			this.AssertEntity();
			if (timerId < 0)
			{
				throw new ArgumentOutOfRangeException("timerId", "Identifier of the timer cannot be less then 0.");
			}

			try
			{
				SetTimerInternal(this.handle, timerId, time.Duration().Milliseconds);
			}
			catch (OverflowException)
			{
			}
		}
		/// <summary>
		/// Stops an already active timer.
		/// </summary>
		/// <remarks>Kill timers don't send or raise any events.</remarks>
		/// <param name="timerId">
		/// Identifier of the timer to kill. If this value is less then 0 then all timers will be killed.
		/// </param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void KillTimer(int timerId)
		{
			this.AssertEntity();

			KillTimerInternal(this.handle, timerId);
		}
		/// <summary>
		/// Links another entity to this one.
		/// </summary>
		/// <param name="linkName">Name of the link between this and another entity.</param>
		/// <param name="id">      Identifier of the entity to link to this one.</param>
		/// <param name="guid">    Globally unique identifier of the entity to link.</param>
		/// <returns>An object that represents the link.</returns>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="ArgumentNullException">Name of the link cannot be null or empty.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Name of the link cannot be longer then 31 symbol.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Unable to identify the entity that will be linked to this one.
		/// </exception>
		public EntityLink Link(string linkName, EntityId id, EntityGUID guid)
		{
			this.AssertEntity();
			if (string.IsNullOrEmpty(linkName))
			{
				throw new ArgumentNullException("linkName", "Name of the link cannot be null or empty.");
			}
			if (linkName.Length > 31)
			{
				throw new ArgumentOutOfRangeException("linkName", "Name of the link cannot be longer then 31 symbol.");
			}
			if (id == default(EntityId) && guid == default(EntityGUID))
			{
				throw new ArgumentException("Unable to identify the entity that will be linked to this one.");
			}

			return AddEntityLink(this.handle, linkName, id, guid);
		}
		/// <summary>
		/// Unlinks this entity from another.
		/// </summary>
		/// <param name="link">
		/// An object that represents the link between this entity and another that must be removed.
		/// </param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="ArgumentNullException">
		/// Object that represents the link that must be removed cannot be null.
		/// </exception>
		public void Unlink(EntityLink link)
		{
			this.AssertEntity();
			if (!link.IsValid)
			{
				throw new ArgumentNullException("link", "Object that represents the link that must be removed cannot be null.");
			}

			RemoveEntityLink(this.handle, link);
		}
		/// <summary>
		/// Unlinks this entity from everything.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void UnlinkEverything()
		{
			this.AssertEntity();

			RemoveAllEntityLinks(this.handle);
		}
		/// <summary>
		/// Physicalizes this entity.
		/// </summary>
		/// <param name="parameters">
		/// A set of parameters that describes how to physicalize this entity.
		/// </param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		/// <exception cref="PhysicalizationException">
		/// An array of points must be provided when creating an area definition for a spline or shape
		/// area.
		/// </exception>
		/// <exception cref="PhysicalizationException">
		/// Physicalization of entity as area requires a valid pointer to AreaDefinition structure.
		/// </exception>
		public void Physicalize(ref EntityPhysicalizationParameters parameters)
		{
			this.AssertEntity();
			parameters.Validate();

			PhysicalizeInternal(this.handle, ref parameters);
		}
		/// <summary>
		/// Dephysicalizes this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void Unphysicalize()
		{
			this.AssertEntity();

			UnphysicalizeInternal(this.handle);
		}
		#endregion
		#region Utilities
		// Assertion method.
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		private void AssertEntity()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("This entity is not usable.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, ulong flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ulong GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddFlagsInternal(IntPtr handle, ulong flagsToAdd);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearFlagsInternal(IntPtr handle, ulong flagsToClear);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckFlagsInternal(IntPtr handle, ulong flagsToCheck, bool all);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsGarbage(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetNameInternal(IntPtr handle, string sName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetNameInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsLoadedFromLevelFile(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsFromPool(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Activate(IntPtr handle, bool bActive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsActive(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrePhysicsActivate(IntPtr handle, bool bActive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPrePhysicsActive(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTimerInternal(IntPtr handle, int nTimerId, int nMilliSeconds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void KillTimerInternal(IntPtr handle, int nTimerId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Hide(IntPtr handle, bool bHide);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsHidden(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeInvisible(IntPtr handle, bool bInvisible);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInvisible(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetUpdatePolicy(IntPtr handle, EntityUpdatePolicy eUpdatePolicy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityUpdatePolicy GetUpdatePolicy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterial(IntPtr handle, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetMaterial(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityLink GetEntityLinks(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityLink AddEntityLink(IntPtr handle, string linkName, EntityId entityId,
													   EntityGUID entityGuid);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveEntityLink(IntPtr handle, EntityLink pLink);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveAllEntityLinks(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern EntityLink GetNextLink(IntPtr linkHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetLinkName(IntPtr linkHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern EntityId GetLinkedEntityId(IntPtr linkHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern EntityGUID GetLinkedEntityGuid(IntPtr linkHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PhysicalizeInternal(IntPtr handle, ref EntityPhysicalizationParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnphysicalizeInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetPhysics(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderNode GetRenderNode(IntPtr handle);
		#endregion
	}
}