using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a wrapper object for CryEngine entities.
	/// </summary>
	public partial struct CryEntity
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties
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
		public EntityFlags Flags
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return (EntityFlags)GetFlags(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetFlags(this.handle, (ulong)value);
			}
		}
		/// <summary>
		/// Indicates whether this entity is scheduled to be deleted on the next frame.
		/// </summary>
		public bool IsGarbage
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetIsGarbage(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the name of this entity.
		/// </summary>
		/// <remarks>The name is not required to be unique.</remarks>
		/// <exception cref="ArgumentNullException">Name of the entity cannot be null.</exception>
		public string Name
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetNameInternal(this.handle);
			}
			set
			{
				this.AssertEntity();
				if (value == null)
				{
					throw new ArgumentNullException("value", "Name of the entity cannot be null.");
				}

				Contract.EndContractBlock();

				SetNameInternal(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this entity was created by the entity load manager using information stored
		/// in the level file.
		/// </summary>
		/// <remarks>Returns <c>false</c> for any entities created dynamically through code.</remarks>
		public bool IsLoadedFromLevelFile
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetIsLoadedFromLevelFile(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this entity is a part of the entity pool.
		/// </summary>
		public bool IsFromPool
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

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
		public bool Active
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return IsActive(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

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
		public bool ReceivesPrePhysicsUpdates
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return IsPrePhysicsActive(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

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
		public bool Hidden
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return IsHidden(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				Hide(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is invisible.
		/// </summary>
		/// <remarks>
		/// Hidden entities don't receive updates, are not rendered and their physics is disabled.
		/// </remarks>
		public bool Invisible
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return IsInvisible(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				MakeInvisible(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets entity's automatic (de-)activation policy.
		/// </summary>
		public EntityUpdatePolicy UpdatePolicy
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return GetUpdatePolicy(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				SetUpdatePolicy(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the pointer to the material that this entity is using.
		/// </summary>
		public Material Material
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return GetMaterial(this.handle);
			}
			set
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				SetMaterial(this.handle, value);
			}
		}
		/// <summary>
		/// Gets an object that provides access to slots of this entity.
		/// </summary>
		public EntitySlotCollection Slots
		{
			get
			{
				this.AssertEntity();
				Contract.EndContractBlock();

				return new EntitySlotCollection(this.handle);
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
		public void AddFlags(EntityFlags flagsToAdd)
		{
			this.AssertEntity();

			Contract.EndContractBlock();

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
		public void ClearFlags(EntityFlags flagsToClear)
		{
			this.AssertEntity();

			Contract.EndContractBlock();

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
		public bool CheckFlags(EntityFlags flagsToCheck, bool all = true)
		{
			this.AssertEntity();

			Contract.EndContractBlock();

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
			Contract.EndContractBlock();

			SetTimerInternal(this.handle, timerId, time.Duration().Milliseconds);
		}
		/// <summary>
		/// Stops an already active timer.
		/// </summary>
		/// <remarks>Kill timers don't send or raise any events.</remarks>
		/// <param name="timerId">
		/// Identifier of the timer to kill. If this value is less then 0 then all timers will be killed.
		/// </param>
		public void KillTimer(int timerId)
		{
			this.AssertEntity();
			Contract.EndContractBlock();

			KillTimerInternal(this.handle, timerId);
		}
		#endregion
		#region Utilities
		// Assertion method.
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
		#endregion
	}
}