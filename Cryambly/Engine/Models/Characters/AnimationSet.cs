using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of possible results of animation sampling.
	/// </summary>
	public enum AnimationSampleResult
	{
		/// <summary>
		/// Sampling was successful.
		/// </summary>
		Success,
		/// <summary>
		/// Sampling wasn't successful due to invalidity of provided animation index.
		/// </summary>
		InvalidAnimationId,
		/// <summary>
		/// Sampling wasn't successful due to animation asset type not supporting sampling.
		/// </summary>
		UnsupportedAssetType,
		/// <summary>
		/// Sampling wasn't successful due to animation not being loaded into memory.
		/// </summary>
		NotInMemory,
		/// <summary>
		/// Sampling wasn't successful due to animation controller not being found.
		/// </summary>
		ControllerNotFound
	}
	/// <summary>
	/// Provides information about the animation asset.
	/// </summary>
	public struct AnimationAsset
	{
		#region Fields
		private readonly IntPtr handle;
		private readonly int index;
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
		/// Gets the name of this animation asset.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetNameByAnimID(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the CRC32 hash code of the lower-case version of the name of this animation asset.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint NameHash
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetCRCByAnimID(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the path to the file this animation asset was loaded from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string File
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetFilePathByID(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the CRC32 hash code of the lower-case version of the file name of this animation asset.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint FileHash
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetFilePathCRCByAnimID(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the duration of this animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public TimeSpan Duration
		{
			get
			{
				this.AssertInstance();

				try
				{
					return TimeSpan.FromSeconds(AnimationSet.GetDuration_sec(this.handle, this.index));
				}
				catch (OverflowException)
				{
				}
				catch (ArgumentException)
				{
				}
				return new TimeSpan();
			}
		}
		/// <summary>
		/// Gets a set of flags that specify this animation asset.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AnimationAssetFlags Flags
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetAnimationFlags(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the size of memory block this animation asset occupies in bytes.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint Size
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.GetAnimationSize(this.handle, (uint)this.index);
			}
		}
		/// <summary>
		/// Indicates whether this animation asset is loaded into memory.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsLoaded
		{
			get
			{
				this.AssertInstance();

				return AnimationSet.IsAnimLoaded(this.handle, this.index);
			}
		}
		#endregion
		#region Construction
		internal AnimationAsset(IntPtr handle, int index)
		{
			this.handle = handle;
			this.index = index;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increments the reference count of this animation asset. Use it if you are going to keep an
		/// extra reference to it for some reason.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AnimationSet.AddRef(this.handle, this.index);
		}
		/// <summary>
		/// Decrements the reference count of this animation asset. Use it if you no longer need an extra
		/// reference.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();

			AnimationSet.Release(this.handle, this.index);
		}
		/// <summary>
		/// Gets the "DCC world space" location of the first frame of the animation asset.
		/// </summary>
		/// <param name="startLocation">
		/// Resultant start location. Will be equal to <see cref="Quatvec.Identity"/> on failure.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAnimationStartLocation(out Quatvec startLocation)
		{
			this.AssertInstance();

			return AnimationSet.GetAnimationDCCWorldSpaceLocationId(this.handle, this.index, out startLocation);
		}
		/// <summary>
		/// Samples the location of a controller at a specific time in a non-parametric animation.
		/// </summary>
		/// <param name="animationNormalizedTime">
		/// A value between 0 and 1 that specifies the point in time to get the location at.
		/// </param>
		/// <param name="controllerId">           Identifier of a controller to use.</param>
		/// <param name="relativeLocationOutput"> 
		/// Resultant location of a controller in local space. Unless return result is
		/// <see cref="AnimationSampleResult.Success"/> this value will be equal to
		/// <see cref="Quatvec.Identity"/>.
		/// </param>
		/// <returns>A result code.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AnimationSampleResult Sample(float animationNormalizedTime, uint controllerId,
											out Quatvec relativeLocationOutput)
		{
			this.AssertInstance();

			return AnimationSet.SampleAnimation(this.handle, this.index, animationNormalizedTime, controllerId,
												out relativeLocationOutput);
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
		#endregion
	}
	/// <summary>
	/// Represents a set of animations in the character instance.
	/// </summary>
	public struct AnimationSet
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
		/// Gets information about the animation asset.
		/// </summary>
		/// <param name="index">Zero-based index of the animation asset.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index of must not be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of animation must be less then total number of animations. Only thrown in Debug builds.
		/// </exception>
		public AnimationAsset this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index of must not be less then 0.");
				}
#if DEBUG
				if (index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index of animation must be less then total number of animations.");
				}
#endif

				return new AnimationAsset(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the number of animation assets in this set.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint Count
		{
			get
			{
				this.AssertInstance();

				return GetAnimationCount(this.handle);
			}
		}
		#endregion
		#region Construction
		internal AnimationSet(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the index of animation asset that has specified name.
		/// </summary>
		/// <param name="name">Name of animation asset to look for.</param>
		/// <returns>Zero-based index of animation asset if found, otherwise returns -1.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(string name)
		{
			this.AssertInstance();

			if (name == null)
			{
				return -1;
			}

			return GetAnimIDByName(this.handle, name);
		}
		/// <summary>
		/// Gets the index of animation asset that has specified name.
		/// </summary>
		/// <param name="nameHash">
		/// CRC32 hash code of the lower-case version of the name of animation asset to look for.
		/// </param>
		/// <returns>Zero-based index of animation asset if found, otherwise returns -1.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(LowerCaseCrc32 nameHash)
		{
			this.AssertInstance();

			return GetAnimIDByCRC(this.handle, nameHash);
		}
		/// <summary>
		/// Gets the "DCC world space" location of the first frame of the animation.
		/// </summary>
		/// <param name="animationName">Name of animation asset to check.</param>
		/// <param name="startLocation">
		/// Resultant start location. Will be equal to <see cref="Quatvec.Identity"/> on failure.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAnimationStartLocation(string animationName, out Quatvec startLocation)
		{
			this.AssertInstance();

			return GetAnimationDCCWorldSpaceLocationName(this.handle, animationName, out startLocation);
		}
		/// <summary>
		/// Gets the "DCC world space" location of the first frame of the animation.
		/// </summary>
		/// <param name="animation">    An object that represents the animation to check.</param>
		/// <param name="startLocation">
		/// Resultant start location. Will be equal to <see cref="Quatvec.Identity"/> on failure.
		/// </param>
		/// <param name="controllerId"> Identifier of the controller to use with the animation.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetAnimationStartLocation(CharacterAnimation animation, out Quatvec startLocation, uint controllerId)
		{
			this.AssertInstance();

			return GetAnimationDCCWorldSpaceLocationObject(this.handle, animation, out startLocation, controllerId);
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
		private static extern uint GetAnimationCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAnimIDByName(IntPtr handle, string szAnimationName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetNameByAnimID(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAnimIDByCRC(IntPtr handle, LowerCaseCrc32 animationCRC);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetCRCByAnimID(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetFilePathCRCByAnimID(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetFilePathByID(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetDuration_sec(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AnimationAssetFlags GetAnimationFlags(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetAnimationSize(IntPtr handle, uint nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsAnimLoaded(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddRef(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Release(IntPtr handle, int nAnimationId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAnimationDCCWorldSpaceLocationName(IntPtr handle, string szAnimationName,
																		 out Quatvec startLocation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetAnimationDCCWorldSpaceLocationId(IntPtr handle, int animId, out Quatvec startLocation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAnimationDCCWorldSpaceLocationObject(IntPtr handle, CharacterAnimation pAnim,
																		   out Quatvec startLocation, uint controllerId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AnimationSampleResult SampleAnimation(IntPtr handle, int animationId,
																	 float animationNormalizedTime, uint controllerId, out Quatvec relativeLocationOutput);
		#endregion
	}
}