using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Geometry;
using CryCil.RunTime;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Defines signature of methods that can be registered as callbacks that will be called by the
	/// animation sub-system as soon as positions and orientations of all bones in the character's skeletons
	/// are updated.
	/// </summary>
	/// <param name="character">Character which bone have just been updated.</param>
	public delegate void CharacterBonesUpdateEventHandler(Character character);
	/// <summary>
	/// Represents a run-time status of the character animation skeleton.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct SkeletonPose
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// A collection of joints in this skeleton.
		/// </summary>
		[FieldOffset(0)] public readonly SkeletonJoints Joints;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets or sets the physical entity that hosts physical representation of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity CharacterPhysics
		{
			get
			{
				this.AssertInstance();

				return GetCharacterPhysics(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetCharacterPhysics(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal SkeletonPose(IntPtr handle)
		{
			this.Joints = new SkeletonJoints();
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Builds the physical representation of this character skeleton and assigns it to the physical
		/// entity.
		/// </summary>
		/// <param name="entity">      
		/// Entity that will host the physical representation of this character.
		/// </param>
		/// <param name="mass">        Mass of this character in kilograms.</param>
		/// <param name="location">    
		/// Reference to the matrix that represents the location of this character in world-space(?).
		/// </param>
		/// <param name="surfaceIndex">
		/// Index of the <see cref="PhysicalSurface"/> that represents the surface of this character.
		/// </param>
		/// <param name="stiffness">   Factor to apply to stiffness of this's character's joints.</param>
		/// <param name="lod">         
		/// Zero-based index of the LOD model of this character to physicalize.
		/// </param>
		/// <param name="firstPartId"> Identifier to assign to the first part of the entity.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void BuildPhysicalEntity(PhysicalEntity entity, float mass, ref Matrix34 location, int surfaceIndex = -1,
										float stiffness = 1.0f, int lod = 0, int firstPartId = 0)
		{
			this.AssertInstance();

			BuildPhysicalEntityInternal(this.handle, entity, mass, ref location, surfaceIndex, stiffness, lod,
										firstPartId);
		}
		/// <summary>
		/// Builds the physical representation of this character skeleton and assigns it to the physical
		/// entity.
		/// </summary>
		/// <param name="entity">      
		/// Entity that will host the physical representation of this character.
		/// </param>
		/// <param name="mass">        Mass of this character in kilograms.</param>
		/// <param name="surfaceIndex">
		/// Index of the <see cref="PhysicalSurface"/> that represents the surface of this character.
		/// </param>
		/// <param name="stiffness">   Factor to apply to stiffness of this's character's joints.</param>
		/// <param name="lod">         
		/// Zero-based index of the LOD model of this character to physicalize.
		/// </param>
		/// <param name="firstPartId"> Identifier to assign to the first part of the entity.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void BuildPhysicalEntity(PhysicalEntity entity, float mass, int surfaceIndex = -1, float stiffness = 1.0f,
										int lod = 0, int firstPartId = 0)
		{
			this.AssertInstance();

			BuildPhysicalEntityInternal(this.handle, entity, mass, ref Matrix34.SecretIdentity, surfaceIndex, stiffness,
										lod, firstPartId);
		}
		/// <summary>
		/// Creates a physical entity that will host the physical representation of this character.
		/// </summary>
		/// <param name="entity">      
		/// An optional host for the physical representation of the character(?).
		/// </param>
		/// <param name="mass">        Mass of the character in kilograms.</param>
		/// <param name="location">    
		/// Reference to the matrix that represents location of the character in the world.
		/// </param>
		/// <param name="surfaceIndex">
		/// Index of the <see cref="PhysicalSurface"/> that represents the surface of this character.
		/// </param>
		/// <param name="stiffness">   Factor to apply to stiffness of this's character's joints.</param>
		/// <param name="lod">         
		/// Zero-based index of the LOD model of this character to physicalize.
		/// </param>
		/// <returns>
		/// If <paramref name="entity"/> was invalid, then returned value will probably be a brand new
		/// entity, otherwise the same entity will probably be returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity CreateCharacterPhysics(PhysicalEntity entity, float mass, ref Matrix34 location,
													 int surfaceIndex = -1, float stiffness = 1.0f, int lod = 0)
		{
			this.AssertInstance();

			return CreateCharacterPhysicsInternal(this.handle, entity, mass, ref location, surfaceIndex, stiffness, lod);
		}
		/// <summary>
		/// Creates a physical entity that will host the physical representation of this character.
		/// </summary>
		/// <param name="entity">      
		/// An optional host for the physical representation of the character(?).
		/// </param>
		/// <param name="mass">        Mass of the character in kilograms.</param>
		/// <param name="surfaceIndex">
		/// Index of the <see cref="PhysicalSurface"/> that represents the surface of this character.
		/// </param>
		/// <param name="stiffness">   Factor to apply to stiffness of this's character's joints.</param>
		/// <param name="lod">         
		/// Zero-based index of the LOD model of this character to physicalize.
		/// </param>
		/// <returns>
		/// If <paramref name="entity"/> was invalid, then returned value will probably be a brand new
		/// entity, otherwise the same entity will probably be returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity CreateCharacterPhysics(PhysicalEntity entity, float mass, int surfaceIndex = -1,
													 float stiffness = 1.0f, int lod = 0)
		{
			this.AssertInstance();

			return CreateCharacterPhysicsInternal(this.handle, entity, mass, ref Matrix34.SecretIdentity, surfaceIndex,
												  stiffness, lod);
		}
		/// <summary>
		/// Creates an auxiliary physical representation of this character.
		/// </summary>
		/// <param name="entity">  A host for the physical representation of the character(?).</param>
		/// <param name="location">
		/// Reference to the matrix that represents location of the character in the world.
		/// </param>
		/// <param name="lod">     
		/// Zero-based index of the LOD model of this character to physicalize.
		/// </param>
		/// <returns>
		/// Identifier that can be used to acquire the physical entity that was created during this call.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int CreateAuxilaryPhysics(PhysicalEntity entity, ref Matrix34 location, int lod = 0)
		{
			this.AssertInstance();

			return CreateAuxilaryPhysicsInternal(this.handle, entity, ref location, lod);
		}
		/// <summary>
		/// Gets the physical entity that hosts the physical representation of the bone.
		/// </summary>
		/// <param name="boneName">Name of the bone.</param>
		/// <returns>Physical entity that hosts the physical representation of the bone.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity GetBonePhysics(string boneName)
		{
			this.AssertInstance();

			return GetCharacterPhysicsBone(this.handle, boneName);
		}
		/// <summary>
		/// Gets the physical entity that hosts the auxiliary physical representation of the bone.
		/// </summary>
		/// <param name="id">Identifier that was returned by <see cref="CreateAuxilaryPhysics"/>.</param>
		/// <returns>Physical entity that hosts the auxiliary physical representation of the bone.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity GetAuxiliaryPhysics(int id)
		{
			this.AssertInstance();

			return GetCharacterPhysicsAux(this.handle, id);
		}
		/// <summary>
		/// Synchronizes the state of this skeleton with provided physical entity.
		/// </summary>
		/// <param name="entity">           
		/// Physical entity that contains the changes that were made to it by the physical world.
		/// </param>
		/// <param name="masterPosition">   
		/// Reference to the position of the entity that contains this character.
		/// </param>
		/// <param name="masterOrientation">
		/// Reference to the orientation of the entity that contains this character.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SyncWithPhysics(PhysicalEntity entity, ref Vector3 masterPosition, ref Quaternion masterOrientation)
		{
			this.AssertInstance();

			SynchronizeWithPhysicalEntity(this.handle, entity, ref masterPosition, ref masterOrientation);
		}
		/// <summary>
		/// Synchronizes the state of this skeleton with provided physical entity.
		/// </summary>
		/// <param name="entity">        
		/// Physical entity that contains the changes that were made to it by the physical world.
		/// </param>
		/// <param name="masterPosition">
		/// Reference to the position of the entity that contains this character.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SyncWithPhysics(PhysicalEntity entity, ref Vector3 masterPosition)
		{
			this.AssertInstance();

			SynchronizeWithPhysicalEntity(this.handle, entity, ref masterPosition, ref Quaternion.SecretIdentity);
		}
		/// <summary>
		/// Synchronizes the state of this skeleton with provided physical entity.
		/// </summary>
		/// <param name="entity">           
		/// Physical entity that contains the changes that were made to it by the physical world.
		/// </param>
		/// <param name="masterOrientation">
		/// Reference to the orientation of the entity that contains this character.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SyncWithPhysics(PhysicalEntity entity, ref Quaternion masterOrientation)
		{
			this.AssertInstance();

			SynchronizeWithPhysicalEntity(this.handle, entity, ref Vector3.SecretZero, ref masterOrientation);
		}
		/// <summary>
		/// Synchronizes the state of this skeleton with provided physical entity.
		/// </summary>
		/// <param name="entity">
		/// Physical entity that contains the changes that were made to it by the physical world.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SyncWithPhysics(PhysicalEntity entity)
		{
			this.AssertInstance();

			SynchronizeWithPhysicalEntity(this.handle, entity, ref Vector3.SecretZero, ref Quaternion.SecretIdentity);
		}
		/// <summary>
		/// Relinquishes the physical representation of this character.
		/// </summary>
		/// <param name="location">           
		/// Reference to the matrix that represents location of the entity that contains the character.
		/// </param>
		/// <param name="initialVelocity">    Reference to the velocity of the entity.</param>
		/// <param name="stiffness">          
		/// A factor to apply to the stiffness of joints in resultant rag-doll.
		/// </param>
		/// <param name="copyJointVelocities">
		/// Indicates whether velocities of individual joints of the skeleton must be applied to the
		/// respective joints in the resultant rag-doll.
		/// </param>
		/// <returns>Physical entity that hosts the resultant rag-doll.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity RelinquishPhysics(ref Matrix34 location, ref Vector3 initialVelocity, float stiffness = 0,
												bool copyJointVelocities = false)
		{
			this.AssertInstance();

			return RelinquishCharacterPhysics(this.handle, ref location, ref initialVelocity, stiffness,
											  copyJointVelocities);
		}
		/// <summary>
		/// Relinquishes the physical representation of this character.
		/// </summary>
		/// <param name="location">           
		/// Reference to the matrix that represents location of the entity that contains the character.
		/// </param>
		/// <param name="stiffness">          
		/// A factor to apply to the stiffness of joints in resultant rag-doll.
		/// </param>
		/// <param name="copyJointVelocities">
		/// Indicates whether velocities of individual joints of the skeleton must be applied to the
		/// respective joints in the resultant rag-doll.
		/// </param>
		/// <returns>Physical entity that hosts the resultant rag-doll.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity RelinquishPhysics(ref Matrix34 location, float stiffness = 0, bool copyJointVelocities = false)
		{
			this.AssertInstance();

			return RelinquishCharacterPhysics(this.handle, ref location, ref Vector3.SecretZero, stiffness,
											  copyJointVelocities);
		}
		/// <summary>
		/// Destroys the physical representation of this character.
		/// </summary>
		/// <param name="mode">A value that specifies how to delete the entity.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DestroyPhysics(PhysicalEntityRemovalMode mode)
		{
			this.AssertInstance();

			DestroyCharacterPhysics(this.handle, mode);
		}
		/// <summary>
		/// Informs the character about an impulse.
		/// </summary>
		/// <param name="partId"> Identifier of the part to affect with an impulse.</param>
		/// <param name="point">  Point of application of impulse.</param>
		/// <param name="impulse">Direction and force of the impulse.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RegisterImpulse(int partId, Vector3 point, Vector3 impulse)
		{
			this.AssertInstance();

			AddImpact(this.handle, partId, point, impulse);
		}
		/// <summary>
		/// Gets the identifier of the bone in auxiliary physical representation of this character.
		/// </summary>
		/// <param name="auxiliaryId">
		/// Identifier of auxiliary physical representation of this character.
		/// </param>
		/// <param name="partId">     Identifier of the physical part that represents the bone.</param>
		/// <returns>Identifier of the bone(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetAuxiliaryBone(int auxiliaryId, int partId)
		{
			this.AssertInstance();

			return GetAuxPhysicsBoneId(this.handle, auxiliaryId, partId);
		}
		/// <summary>
		/// Initiates procedural "stand up" animation.
		/// </summary>
		/// <param name="location">             
		/// Reference to the object that contains position and orientation of the entity.
		/// </param>
		/// <param name="physicalEntity">       
		/// Reference to the object that will contain the physical representation of this character.
		/// </param>
		/// <param name="threeDegreesOfFreedom">Unknown.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool BlendFromRagDoll(ref Quatvecale location, out PhysicalEntity physicalEntity,
									 bool threeDegreesOfFreedom = false)
		{
			this.AssertInstance();

			return BlendFromRagdoll(this.handle, ref location, out physicalEntity, threeDegreesOfFreedom);
		}
		/// <summary>
		/// Gets identifier of the joint of the bone.
		/// </summary>
		/// <param name="boneIndex">Identifier of the bone(?).</param>
		/// <param name="lod">      Zero-based index of the LOD model.</param>
		/// <returns>
		/// Identifier that can be used to check the bone in <see cref="DefaultSkeleton"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetBoneJointId(int boneIndex, int lod = 0)
		{
			this.AssertInstance();

			return GetBonePhysParentOrSelfIndex(this.handle, boneIndex, lod);
		}
		/// <summary>
		/// Assigns a function that should handle the event of bones positions getting updated.
		/// </summary>
		/// <param name="handler">
		/// Reference to the static field (Important!) that contains the delegate that should be called when
		/// the event occurs.
		/// </param>
		/// <example>
		/// Here is the only valid way of using this method:
		/// <code source="SkeletonPostProcessing.cs"/>
		/// </example>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetBoneUpdateHandler(ref CharacterBonesUpdateEventHandler handler)
		{
			this.AssertInstance();

			SetPostProcessCallback(this.handle, ref handler);
		}
		/// <summary>
		/// Removes the previously set bone update handler. This function will remove any handlers including
		/// native ones.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ClearBoneUpdateHandler()
		{
			this.AssertInstance();

			ClearPostProcessCallback(this.handle);
		}
		/// <summary>
		/// Forces a skeleton to be updated during next specified frames.
		/// </summary>
		/// <param name="frames">
		/// Number of next frames to update the skeleton on. If <c>0x8000</c> is passed then skeleton
		/// updates will be done indefinitely.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ForceSkeletonUpdate(int frames)
		{
			this.AssertInstance();

			SetForceSkeletonUpdate(this.handle, frames);
		}
		/// <summary>
		/// Forces this skeleton to switch to default pose.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ForceDefaultPose()
		{
			this.AssertInstance();

			SetDefaultPose(this.handle);
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
		private static extern void BuildPhysicalEntityInternal(IntPtr handle, PhysicalEntity pent, float mass,
															   ref Matrix34 mtxloc, int surfaceIdx, float stiffnessScale, int nLod, int partid0);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity CreateCharacterPhysicsInternal(IntPtr handle, PhysicalEntity pHost, float mass,
																			ref Matrix34 mtxloc, int surfaceIdx, float stiffnessScale, int nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int CreateAuxilaryPhysicsInternal(IntPtr handle, PhysicalEntity pHost, ref Matrix34 mtx,
																int nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetCharacterPhysics(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetCharacterPhysicsBone(IntPtr handle, string pRootBoneName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetCharacterPhysicsAux(IntPtr handle, int iAuxPhys);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCharacterPhysics(IntPtr handle, PhysicalEntity pent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SynchronizeWithPhysicalEntity(IntPtr handle, PhysicalEntity pent, ref Vector3 posMaster,
																 ref Quaternion qMaster);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity RelinquishCharacterPhysics(IntPtr handle, ref Matrix34 mtx, ref Vector3 velHost,
																		float stiffness, bool bCopyJointVelocities);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyCharacterPhysics(IntPtr handle, PhysicalEntityRemovalMode iMode);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool AddImpact(IntPtr handle, int partid, Vector3 point, Vector3 impact);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAuxPhysicsBoneId(IntPtr handle, int iAuxPhys, int iBone);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool BlendFromRagdoll(IntPtr handle, ref Quatvecale location, out PhysicalEntity pPhysicalEntity,
													bool b3Dof);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetBonePhysParentOrSelfIndex(IntPtr handle, int nBoneIndex, int nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern PhysicalEntity GetPhysEntOnJoint(IntPtr handle, int nId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPhysEntOnJoint(IntPtr handle, int nId, PhysicalEntity pPhysEnt);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPhysIdOnJoint(IntPtr handle, int nId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Quatvec GetAbsJointById(IntPtr handle, int nJointId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Quatvec GetRelJointById(IntPtr handle, int nJointId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPostProcessCallback(IntPtr handle, ref CharacterBonesUpdateEventHandler handler);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearPostProcessCallback(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetForceSkeletonUpdate(IntPtr handle, int i);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDefaultPose(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetStatObjOnJoint(IntPtr handle, int nId, StaticObject pStatObj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticObject GetStatObjOnJoint(IntPtr handle, int nId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetMaterialOnJoint(IntPtr handle, int nId, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Material GetMaterialOnJoint(IntPtr handle, int nId);

		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		[RawThunk("Invoked from underlying framework to invoke the callbacks.")]
		private static void HandleBoneUpdates(ref CharacterBonesUpdateEventHandler handler, Character character)
		{
			try
			{
				// We are basically raising an event here.
				var delegates = handler;

				delegates?.Invoke(character);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}