using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.Engine.Decals;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents a geometric model that is an animated character.
	/// </summary>
	public partial struct Character
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
		/// Queries current reference count of this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ReferenceCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetRefCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that handles skeleton animations for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public SkeletonAnimation SkeletonAnimation
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetISkeletonAnim(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that handles skeleton pose for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public SkeletonPose SkeletonPose
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetISkeletonPose(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that manages objects that are attached to this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentManager AttachmentManager
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIAttachmentManager(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that represents general skeleton model for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public DefaultSkeleton DefaultSkeleton
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIDefaultSkeleton(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that holds description of all animations for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AnimationSet AnimationSet
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIAnimationSet(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the file that contains definitions of all animation events for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string AnimationEventsDatabaseFile
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetModelAnimEventDatabase(this.handle);
			}
		}
		/// <summary>
		/// Gets the current bounding box of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetAABB(this.handle);
			}
		}
		/// <summary>
		/// Gets the squared radius of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float SquaredRadius
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetRadiusSqr(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify how to render this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CharacterRenderFlags Flags
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFlags(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetFlags(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the identifier of the file format that was used by the file this character was loaded
		/// from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AnimationFileFormatIds FileFormat
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetObjectType(this.handle);
			}
		}
		/// <summary>
		/// Gets the path either character's model file or .cdf file.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string FileName
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFilePath(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this character uses any animations that manipulate vertices directly rather
		/// then through the bones.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HasVertexAnimation
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetHasVertexAnimation(this.handle);
			}
		}
		/// <summary>
		/// Gets the material that is currently being used to render this character.
		/// </summary>
		/// <returns>
		/// Either default model's material or a custom material that assigned via
		/// <see cref="CustomMaterial"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material Material
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIMaterial(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the custom material to use to render this character.
		/// </summary>
		/// <returns>
		/// Either custom material that was set before, or invalid instance, if default material is being
		/// used.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Material instance cannot be null.</exception>
		public Material CustomMaterial
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIMaterial_Instance(this.handle);
			}
			set
			{
				this.AssertInstance();
				if (!value.IsValid)
				{
					throw new ArgumentNullException("value", "Material instance cannot be null.");
				}
				Contract.EndContractBlock();

				SetIMaterial_Instance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the object that handles facial animations for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialInstance FacialInstance
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFacialInstance(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the scale of animation playback speed.
		/// </summary>
		/// <value>
		/// Some values and their effect on the animations: <list type="number"><item>0 - animations are
		/// still (stuck on one frame).</item><item>1 - animations are played normally.</item><item>2 -
		/// animations are played at double speed.</item><item>0.5 - animations are played at half
		/// speed.</item></list>
		/// </value>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackScale
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetPlaybackScale(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetPlaybackScale(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this character is visible.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsVisible
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsCharacterVisible(this.handle);
			}
		}
		/// <summary>
		/// Gets the scale of this character.
		/// </summary>
		/// <remarks>
		/// Cannot be used to build the parameters before calling <see cref="UpdateAnimationProcessing"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float UniformScale
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetUniformScale(this.handle);
			}
		}
		#endregion
		#region Construction
		internal Character(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new character instance by loading a model file along with any available animation files.
		/// </summary>
		/// <param name="file">Path to the model file to load.</param>
		/// <param name="flags">A set of flags that specifies how to load the character.</param>
		public Character(string file, CharacterLoadingFlags flags)
		{
			this.handle = CreateInstance(file, flags);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increments the reference count of this instance. Use this to prevent deletion of this character
		/// while you still need it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decrements the reference count of this instance. Use this to allow deletion of this character
		/// when you don't need it anymore.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			Release(this.handle);
		}
		/// <summary>
		/// Enables/disables calls to methods that start this character's animations.
		/// </summary>
		/// <param name="enable">
		/// Indicates whether StartAnimation* methods must be enabled or disabled.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchStartAnimation(bool enable)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableStartAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables calls to methods that start this character's animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableStartAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableStartAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables calls to methods that start this character's animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableStartAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableStartAnimationInternal(this.handle, false);
		}
		/// <summary>
		/// Forces underlying object to update the animation. Needs to be updated every frame unless this
		/// is handled by something else (e.g. when this character is bound to the entity slot).
		/// </summary>
		/// <param name="parameters">
		/// Reference to the object that contains information that is relevant for animation prosess.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UpdateAnimationProcessing(ref AnimationProcessParameters parameters)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			StartAnimationProcessing(this.handle, ref parameters);
		}
		/// <summary>
		/// Gets the random point on this character's model surface.
		/// </summary>
		/// <param name="aspect">Aspect of chracter's geometry to find the point on.</param>
		/// <returns>
		/// An object that contains coordinates of the point and a normal to the surface at that point.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PositionNormal GetRandomPosition(GeometryFormat aspect)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			PositionNormal positionNormal;
			GetRandomPos(this.handle, out positionNormal, aspect);
			return positionNormal;
		}
		/// <summary>
		/// Enables/disables decals on this character.
		/// </summary>
		/// <param name="enable">Indicates whether decals must be enabled or disabled.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchDecals(bool enable)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableDecalsInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables decals on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableDecals()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableDecalsInternal(this.handle, true);
		}
		/// <summary>
		/// Disables decals on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableDecals()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableDecalsInternal(this.handle, false);
		}
		/// <summary>
		/// Puts a decal on this character.
		/// </summary>
		/// <param name="decal">
		/// Reference to the object that contains definition of the decal. All coordinates are in
		/// character's local space.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CreateDecal(ref CryEngineDecalInfo decal)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			CreateDecalInternal(this.handle, ref decal);
		}
		/// <summary>
		/// Enables/disables facial animations on this character.
		/// </summary>
		/// <param name="enable">Indicates whether facial animations must be enabled or disabled.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchFacialAnimation(bool enable)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableFacialAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableFacialAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableFacialAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableFacialAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableFacialAnimationInternal(this.handle, false);
		}
		/// <summary>
		/// Enables/disables procedural facial animations on this character.
		/// </summary>
		/// <param name="enable">
		/// Indicates whether procedural facial animations must be enabled or disabled.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchProceduralFacialAnimation(bool enable)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableProceduralFacialAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables procedural facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableProceduralFacialAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableProceduralFacialAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables procedural facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableProceduralFacialAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			EnableProceduralFacialAnimationInternal(this.handle, false);
		}
		/// <summary>
		/// Starts lip-syncing with the sound.
		/// </summary>
		/// <param name="soundId">Identifier of the sound to sync the lips with.</param>
		/// <param name="stop">   Indicates whether lip syncing should stop rather then start.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SyncLips(uint soundId, bool stop = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			LipSyncWithSound(this.handle, soundId, stop);
		}
		/// <summary>
		/// Spawns an effect on the bone of the skeleton.
		/// </summary>
		/// <param name="animationId">   Identifier of the animation.</param>
		/// <param name="animationName"> Name of animation.</param>
		/// <param name="effectName">    Name of the effect to spawn.</param>
		/// <param name="boneName">      Name of the bone to spawn the effect on.</param>
		/// <param name="offset">        Position of the effect (relative to the bone(?)).</param>
		/// <param name="direction">     Direction of the effect.</param>
		/// <param name="entityLocation">Location of the entity.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SpawnSkeletonEffect(int animationId, string animationName, string effectName, string boneName,
										ref Vector3 offset, ref Vector3 direction, ref Quatvecale entityLocation)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SpawnSkeletonEffectInternal(this.handle, animationId, animationName, effectName, boneName, ref offset,
										ref direction, ref entityLocation);
		}
		/// <summary>
		/// Deletes all effects that were spawned using <see cref="SpawnSkeletonEffect"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void KillSkeletonEffects()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			KillAllSkeletonEffects(this.handle);
		}
		/// <summary>
		/// Copies the pose from another character.
		/// </summary>
		/// <param name="instance">Character to copy the pose from.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CopyPoseFrom(Character instance)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			CopyPoseFromInternal(this.handle, instance);
		}
		/// <summary>
		/// Makes sure that this character is ready for forward kinematics simulation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void FinishAnimationComputations()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			FinishAnimationComputationsInternal(this.handle);
		}
		/// <summary>
		/// Enables/disables rendering for this character.
		/// </summary>
		/// <param name="hide">Indicates whether rendering should be disabled or enabled.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchHiding(bool hide)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			HideMaster(this.handle, hide);
		}
		/// <summary>
		/// Disables rendering for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Hide()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			HideMaster(this.handle, true);
		}
		/// <summary>
		/// Enables rendering for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Unhide()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			HideMaster(this.handle, false);
		}
		/// <summary>
		/// Synchronizes the state of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Synchronize(CrySync sync)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			Serialize(this.handle, sync);
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRefCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SkeletonAnimation GetISkeletonAnim(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SkeletonPose GetISkeletonPose(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AttachmentManager GetIAttachmentManager(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern DefaultSkeleton GetIDefaultSkeleton(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationSet GetIAnimationSet(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetModelAnimEventDatabase(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableStartAnimationInternal(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StartAnimationProcessing(IntPtr handle, ref AnimationProcessParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BoundingBox GetAABB(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetRadiusSqr(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomPos(IntPtr handle, out PositionNormal ran, GeometryFormat eForm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, CharacterRenderFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CharacterRenderFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationFileFormatIds GetObjectType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFilePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableDecalsInternal(IntPtr handle, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateDecalInternal(IntPtr handle, ref CryEngineDecalInfo DecalLCS);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHasVertexAnimation(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetIMaterial(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIMaterial_Instance(IntPtr handle, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetIMaterial_Instance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialInstance GetFacialInstance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableFacialAnimationInternal(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableProceduralFacialAnimationInternal(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LipSyncWithSound(IntPtr handle, uint nSoundId, bool bStop);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPlaybackScale(IntPtr handle, float fSpeed);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetPlaybackScale(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsCharacterVisible(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SpawnSkeletonEffectInternal(IntPtr handle, int animID, string animName,
															   string effectName, string boneName, ref Vector3 offset,
															   ref Vector3 dir, ref Quatvecale entityLoc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void KillAllSkeletonEffects(IntPtr handle);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern void SetViewdir(IntPtr handle, ref Vector3 rViewdir);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetUniformScale(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyPoseFromInternal(IntPtr handle, Character instance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FinishAnimationComputationsInternal(IntPtr handle);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern void ProcessAttachment(IntPtr handle, CharacterAttachment pIAttachment);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void HideMaster(IntPtr handle, bool h);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Serialize(IntPtr handle, CrySync ser);
		#endregion
	}
}