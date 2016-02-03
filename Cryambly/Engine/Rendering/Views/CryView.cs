using System;
using System.Runtime.CompilerServices;
using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace CryCil.Engine.Rendering.Views
{
	/// <summary>
	/// Represents an object that provides functionality related to modification of the player's view camera as well as audio listener.
	/// </summary>
	public struct CryView
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets the default distance to the near-clipping plane.
		/// </summary>
		public float DefaultZNear
		{
			get
			{
				return GetDefaultZNear();
			}
		}
		/// <summary>
		/// Indicates if a cutscene is currently being played.
		/// </summary>
		public bool IsCutscene
		{
			get { return IsPlayingCutScene(); }
		}
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
		/// Gets the identifier of the entity this view is linked to.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityId LinkedEntity
		{
			get
			{
				this.AssertInstance();

				return GetLinkedId(this.handle);
			}
		}
		/// <summary>
		/// Sets the scale of this view.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Scale
		{
			set
			{
				this.AssertInstance();

				SetScale(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the zoomed scale of this view.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ZoomedScale
		{
			set
			{
				this.AssertInstance();

				SetZoomedScale(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this view is active.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Active
		{
			set
			{
				this.AssertInstance();

				if (value)
				{
					SetActiveView(this);
				}
			}
		}
		/// <summary>
		/// Gets identifier of this view.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint Id
		{
			get
			{
				this.AssertInstance();

				return GetViewId(this);
			}
		}
		#endregion
		#region Construction
		internal CryView(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Static Interface
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <returns>A new instance of this type.</returns>
		public static CryView Create()
		{
			return CreateView();
		}
		/// <summary>
		/// Destroys given view object.
		/// </summary>
		/// <param name="id">Identifier of the view.</param>
		public static void Destroy(uint id)
		{
			RemoveViewId(id);
		}
		/// <summary>
		/// Sets the view as a currently active one.
		/// </summary>
		/// <param name="id">Identifier of the view to activate.</param>
		public static void SetActiveView(uint id)
		{
			SetActiveViewId(id);
		}
		/// <summary>
		/// Gets the view using specified identifier.
		/// </summary>
		/// <param name="viewId">Identifier of the view to get.</param>
		/// <returns>A valid object if provided identifier was valid.</returns>
		public static CryView GetView(uint viewId)
		{
			return GetViewInternal(viewId);
		}
		/// <summary>
		/// Gets currently active view.
		/// </summary>
		/// <returns>An object that represents currently active view.</returns>
		public static CryView GetActiveView()
		{
			return GetActiveViewInternal();
		}
		/// <summary>
		/// Gets currently active view.
		/// </summary>
		/// <returns>An identifier of the currently active view.</returns>
		public static uint GetActiveViewId()
		{
			return GetActiveViewIdInternal();
		}
		/// <summary>
		/// Gets the view that is linked to the specified entity.
		/// </summary>
		/// <param name="id">Identifier of the entity.</param>
		/// <param name="forceCreate">Indicates whether a view needs to be created and linked to the entity, if it wasn't already.</param>
		/// <returns>An object that represents a view.</returns>
		public static CryView GetView(EntityId id, bool forceCreate = false)
		{
			return GetViewByEntityId(id, forceCreate);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes this view object from the program.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Release()
		{
			this.AssertInstance();

			RemoveView(this);
		}
		/// <summary>
		/// Updates logical state of this view. Not necessary, if this view is linked to an entity.
		/// </summary>
		/// <param name="frameTime">Length of the previous frame.</param>
		/// <param name="active">Whether this view is supposed to be active(?).</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Update(float frameTime, bool active)
		{
			this.AssertInstance();

			UpdateInternal(this.handle, frameTime, active);
		}
		/// <summary>
		/// Links this view to an unmanaged entity.
		/// </summary>
		/// <param name="entity">An entity to bind this view to.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void LinkTo(CryEntity entity)
		{
			if (!entity.IsValid)
			{
				return;
			}
			this.AssertInstance();

			LinkToInternal(this.handle, entity, false);
		}
		/// <summary>
		/// Links this view to an managed entity.
		/// </summary>
		/// <param name="entity">An entity to bind this view to.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void LinkTo(MonoEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			this.AssertInstance();

			LinkToInternal(this.handle, entity.Entity, true);
		}
		/// <summary>
		/// Unlinks this view from its current entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Unlink()
		{
			this.AssertInstance();

			LinkToInternal(this.handle, new CryEntity(), false);
		}
		/// <summary>
		/// Updates a set of parameters that specifies this view.
		/// </summary>
		/// <param name="parameters">Reference to the object that provides the parameters.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetCurrentParameters(ref ViewParameters parameters)
		{
			this.AssertInstance();

			SetCurrentParams(this.handle, ref parameters);
		}
		/// <summary>
		/// Gets a set of parameters that specifies this view.
		/// </summary>
		/// <param name="parameters">Reference to the object that provides the parameters.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void GetCurrentParameters(out ViewParameters parameters)
		{
			this.AssertInstance();

			GetCurrentParams(this.handle, out parameters);
		}
		/// <summary>
		/// Makes this view shake.
		/// </summary>
		/// <param name="shakeAngle">Maximal angle of shaking.</param>
		/// <param name="shakeShift">Maximal offset of shaking.</param>
		/// <param name="duration">Duration of shaking after fade-in and before fade-out in seconds.</param>
		/// <param name="frequency">Frequency of shakes.</param>
		/// <param name="randomness">Randomness of shakes.</param>
		/// <param name="shakeId">Identifier of the shake.</param>
		/// <param name="flipVector">Unknown.</param>
		/// <param name="updateOnly">Unknown.</param>
		/// <param name="groundOnly">Indicates whether shaking should be applied only when the player is on the ground.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetViewShaking(EulerAngles shakeAngle, Vector3 shakeShift, float duration, float frequency,
								   float randomness, int shakeId, bool flipVector = true, bool updateOnly = false,
								   bool groundOnly = false)
		{
			this.AssertInstance();

			SetViewShake(this.handle, shakeAngle, shakeShift, duration, frequency, randomness, shakeId,
						 flipVector, updateOnly, groundOnly);
		}
		/// <summary>
		/// Makes this view shake.
		/// </summary>
		/// <param name="parameters">Reference to the object that specifies shaking.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetViewShaking(ref ShakeParameters parameters)
		{
			this.AssertInstance();

			SetViewShakeEx(this.handle, ref parameters);
		}
		/// <summary>
		/// Stops one of the shake effects.
		/// </summary>
		/// <param name="shakeId">Identifier of the shake effect to stop.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StopShaking(int shakeId)
		{
			this.AssertInstance();

			StopShakeInternal(this.handle, shakeId);
		}
		/// <summary>
		/// Resets all shake effects.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ResetShaking()
		{
			this.AssertInstance();

			ResetShakingInternal(this.handle);
		}
		/// <summary>
		/// Resets the blending.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ResetBlending()
		{
			this.AssertInstance();

			ResetBlendingInternal(this.handle);
		}
		/// <summary>
		/// Sets a set of angles that are added to camera rotation at the end of the frame.
		/// </summary>
		/// <param name="angles">Reference to the object that provides the angles.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetFrameAdditiveAngles(ref EulerAngles angles)
		{
			this.AssertInstance();

			SetFrameAdditiveCameraAngles(this.handle, ref angles);
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
		private static extern void UpdateInternal(IntPtr handle, float frameTime, bool isActive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LinkToInternal(IntPtr handle, CryEntity follow, bool gameObject);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityId GetLinkedId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCurrentParams(IntPtr handle, ref ViewParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCurrentParams(IntPtr handle, out ViewParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetViewShake(IntPtr handle, EulerAngles shakeAngle, Vector3 shakeShift, float duration, float frequency, float randomness, int shakeID, bool bFlipVec, bool bUpdateOnly, bool bGroundOnly);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetViewShakeEx(IntPtr handle, ref ShakeParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopShakeInternal(IntPtr handle, int shakeId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetShakingInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetBlendingInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFrameAdditiveCameraAngles(IntPtr handle, ref EulerAngles addFrameAngles);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScale(IntPtr handle, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetZoomedScale(IntPtr handle, float scale);


		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryView CreateView();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveView(CryView pView);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveViewId(uint viewId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetActiveView(CryView pView);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetActiveViewId(uint viewId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryView GetViewInternal(uint viewId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryView GetActiveViewInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetViewId(CryView pView);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetActiveViewIdInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryView GetViewByEntityId(EntityId id, bool forceCreate);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetDefaultZNear();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPlayingCutScene();
		#endregion
	}
}