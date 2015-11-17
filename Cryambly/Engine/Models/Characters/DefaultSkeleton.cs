using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents a general model that is used by the character.
	/// </summary>
	public struct DefaultSkeleton
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
		/// Gets the number of joints in this skeleton.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint JointCount
		{
			get
			{
				this.AssertInstance();

				return GetJointCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the file this model was loaded from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string ModelFile
		{
			get
			{
				this.AssertInstance();

				return GetModelFilePath(this.handle);
			}
		}
		#endregion
		#region Construction
		internal DefaultSkeleton(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new object of this type and loads information about a skeleton into it.
		/// </summary>
		/// <param name="file"> Path to the file.</param>
		/// <param name="flags">A set of flags that specify how to load the model.</param>
		public DefaultSkeleton(string file, CharacterLoadingFlags flags)
		{
			this.handle = Character.LoadModelSKEL(file, flags);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the parent joint to another.
		/// </summary>
		/// <param name="index">Zero-based index of the joint which parent to get.</param>
		/// <returns>Zero-based index of the joint that is a parent to another.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetJointParent(int index)
		{
			this.AssertInstance();

			return GetJointParentIDByID(this.handle, index);
		}
		/// <summary>
		/// Gets the identifier of the animation controller that is associated with a joint.
		/// </summary>
		/// <param name="index">Zero-based index of the joint which controller to get.</param>
		/// <returns>Identifier of the animation controller.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetJointController(int index)
		{
			this.AssertInstance();

			return GetControllerIDByID(this.handle, index);
		}
		/// <summary>
		/// Gets the index of joint that is identifiable by given CRC32 hash.
		/// </summary>
		/// <param name="crc32">CRC32 hash of the name of the joint.</param>
		/// <returns>Zero-based index of the joint which name's CRC32 hash is equal to given one.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOfJoint(LowerCaseCrc32 crc32)
		{
			this.AssertInstance();

			return GetJointIDByCRC32(this.handle, crc32);
		}
		/// <summary>
		/// Gets the index of joint that is identifiable by given name.
		/// </summary>
		/// <param name="name">Name of the joint.</param>
		/// <returns>Zero-based index of the joint which name is equal to given one.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOfJoint(string name)
		{
			this.AssertInstance();

			return GetJointIDByName(this.handle, name);
		}
		/// <summary>
		/// Gets the hash value of the name of the joint.
		/// </summary>
		/// <param name="index">Zero-based index of the joint which name's hash value to get.</param>
		/// <returns>CRC32 value calculated from lowercase version of the name of the joint.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint GetJointHash(int index)
		{
			this.AssertInstance();

			return GetJointCRC32ByID(this.handle, index);
		}
		/// <summary>
		/// Gets the name of the joint.
		/// </summary>
		/// <param name="index">Zero-based index of the joint which name to get.</param>
		/// <returns>Name of the joint.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string GetJointName(int index)
		{
			this.AssertInstance();

			return GetJointNameByID(this.handle, index);
		}
		/// <summary>
		/// Gets default location of the joint.
		/// </summary>
		/// <param name="index">Zero-based index of the joint.</param>
		/// <returns>Position and orientation of the joint in model-space (?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec GetJointDefaultAbsoluteLocation(uint index)
		{
			this.AssertInstance();

			Quatvec location;
			GetDefaultAbsJointByID(this.handle, index, out location);
			return location;
		}
		/// <summary>
		/// Gets default location of the joint.
		/// </summary>
		/// <param name="index">Zero-based index of the joint.</param>
		/// <returns>Position and orientation of the joint relative to its parent joint (?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec GetJointDefaultRelativeLocation(uint index)
		{
			this.AssertInstance();

			Quatvec location;
			GetDefaultRelJointByID(this.handle, index, out location);
			return location;
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
		private static extern uint GetJointCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetJointParentIDByID(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetControllerIDByID(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetJointIDByCRC32(IntPtr handle, LowerCaseCrc32 crc32);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetJointCRC32ByID(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetJointNameByID(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetJointIDByName(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDefaultAbsJointByID(IntPtr handle, uint nJointIdx, out Quatvec jointLocation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDefaultRelJointByID(IntPtr handle, uint nJointIdx, out Quatvec jointLocation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetModelFilePath(IntPtr handle);
		#endregion
	}
}