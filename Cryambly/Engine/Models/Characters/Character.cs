﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.Engine.Models.Characters.Attachments;
using CryCil.Engine.Models.Characters.Faces;
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
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Queries current reference count of this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ReferenceCount
		{
			get
			{
				this.AssertInstance();

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

				return GetFlags(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetFlags(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the identifier of the file format that was used by the file this character was loaded from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AnimationFileFormatIds FileFormat
		{
			get
			{
				this.AssertInstance();

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

				return GetIMaterial_Instance(this.handle);
			}
			set
			{
				this.AssertInstance();
				if (!value.IsValid)
				{
					throw new ArgumentNullException(nameof(value), "Material instance cannot be null.");
				}

				SetIMaterial_Instance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the object that handles facial animations for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Face Face
		{
			get
			{
				this.AssertInstance();

				return GetFacialInstance(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the scale of animation playback speed.
		/// </summary>
		/// <value>
		/// Some values and their effect on the animations:
		/// <list type="number">
		/// <item>0 - animations are still (stuck on one frame).</item>
		/// <item>1 - animations are played normally.</item>
		/// <item>2 - animations are played at double speed.</item>
		/// <item>0.5 - animations are played at half speed.</item>
		/// </list>
		/// </value>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackScale
		{
			get
			{
				this.AssertInstance();

				return GetPlaybackScale(this.handle);
			}
			set
			{
				this.AssertInstance();

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
		/// Creates a new character instance by loading a model file along with any available animation
		/// files.
		/// </summary>
		/// <param name="file"> Path to the model file to load.</param>
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

			EnableStartAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables calls to methods that start this character's animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableStartAnimation()
		{
			this.AssertInstance();

			EnableStartAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables calls to methods that start this character's animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableStartAnimation()
		{
			this.AssertInstance();

			EnableStartAnimationInternal(this.handle, false);
		}
		/// <summary>
		/// Forces underlying object to update the animation. Needs to be updated every frame unless this is
		/// handled by something else (e.g. when this character is bound to the entity slot).
		/// </summary>
		/// <param name="parameters">
		/// Reference to the object that contains information that is relevant for animation process.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UpdateAnimationProcessing(ref AnimationProcessParameters parameters)
		{
			this.AssertInstance();

			StartAnimationProcessing(this.handle, ref parameters);
		}
		/// <summary>
		/// Gets the random point on this character's model surface.
		/// </summary>
		/// <param name="aspect">Aspect of chracter's geometry to find the point on.</param>
		/// <param name="random">An object that is used for random generation.</param>
		/// <returns>
		/// An object that contains coordinates of the point and a normal to the surface at that point.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PositionNormal GetRandomPosition(GeometryFormat aspect, LcgRandom random)
		{
			this.AssertInstance();

			PositionNormal positionNormal;
			GetRandomPos(this.handle, out positionNormal, ref random.State, aspect);
			return positionNormal;
		}
		/// <summary>
		/// Enables/disables facial animations on this character.
		/// </summary>
		/// <param name="enable">Indicates whether facial animations must be enabled or disabled.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SwitchFacialAnimation(bool enable)
		{
			this.AssertInstance();

			EnableFacialAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableFacialAnimation()
		{
			this.AssertInstance();

			EnableFacialAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableFacialAnimation()
		{
			this.AssertInstance();

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

			EnableProceduralFacialAnimationInternal(this.handle, enable);
		}
		/// <summary>
		/// Enables procedural facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableProceduralFacialAnimation()
		{
			this.AssertInstance();

			EnableProceduralFacialAnimationInternal(this.handle, true);
		}
		/// <summary>
		/// Disables procedural facial animations on this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DisableProceduralFacialAnimation()
		{
			this.AssertInstance();

			EnableProceduralFacialAnimationInternal(this.handle, false);
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

			CopyPoseFromInternal(this.handle, instance);
		}
		/// <summary>
		/// Makes sure that this character is ready for forward kinematics simulation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void FinishAnimationComputations()
		{
			this.AssertInstance();

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

			HideMaster(this.handle, hide);
		}
		/// <summary>
		/// Disables rendering for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Hide()
		{
			this.AssertInstance();

			HideMaster(this.handle, true);
		}
		/// <summary>
		/// Enables rendering for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Unhide()
		{
			this.AssertInstance();

			HideMaster(this.handle, false);
		}
		/// <summary>
		/// Synchronizes the state of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Synchronize(CrySync sync)
		{
			this.AssertInstance();

			Serialize(this.handle, sync);
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
		private static extern void GetRandomPos(IntPtr handle, out PositionNormal ran, ref ulong seed, GeometryFormat eForm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, CharacterRenderFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CharacterRenderFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationFileFormatIds GetObjectType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFilePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetHasVertexAnimation(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetIMaterial(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIMaterial_Instance(IntPtr handle, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetIMaterial_Instance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Face GetFacialInstance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableFacialAnimationInternal(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableProceduralFacialAnimationInternal(IntPtr handle, bool bEnable);
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