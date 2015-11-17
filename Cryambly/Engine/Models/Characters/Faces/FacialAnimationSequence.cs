using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry.Splines;
using CryCil.Utilities;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a collection of facial animation channels in the sequence.
	/// </summary>
	public struct FacialAnimationSequenceChannels
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
		/// Gets the number of channels in the facial animation sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialAnimationSequence.GetChannelCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the facial animation channel.
		/// </summary>
		/// <param name="index">Zero-based index of the channel to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialAnimationChannel this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialAnimationSequence.GetChannel(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSequenceChannels(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates a new facial animation channel and adds it to this sequence.
		/// </summary>
		/// <returns>A new facial animation channel.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationChannel Create()
		{
			this.AssertInstance();

			return FacialAnimationSequence.CreateChannel(this.handle);
		}
		/// <summary>
		/// Creates a new group of facial animation channels and adds it to this sequence.
		/// </summary>
		/// <returns>A new facial animation channel that represents a root of the group.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationChannel CreateGroup()
		{
			this.AssertInstance();

			return FacialAnimationSequence.CreateChannelGroup(this.handle);
		}
		/// <summary>
		/// Removes a channel from this sequence.
		/// </summary>
		/// <param name="channel">A facial animation channel to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Remove(FacialAnimationChannel channel)
		{
			this.AssertInstance();

			FacialAnimationSequence.RemoveChannel(this.handle, channel);
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
	/// Represents a collection of sounds in the sequence.
	/// </summary>
	public struct FacialAnimationSequenceSounds
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
		/// Gets the number of sounds in the facial animation sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialAnimationSequence.GetSoundEntryCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the sound entry.
		/// </summary>
		/// <param name="index">Zero-based index of the sound entry to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialAnimationSoundEntry this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialAnimationSequence.GetSoundEntry(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSequenceSounds(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates and inserts a sound entry into the sequence.
		/// </summary>
		/// <param name="index">Zero-based index of the slot to insert the entry into.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Insert(int index)
		{
			this.AssertInstance();

			FacialAnimationSequence.InsertSoundEntry(this.handle, index);
		}
		/// <summary>
		/// Removes a sound entry into the sequence.
		/// </summary>
		/// <param name="index">Zero-based index of the the entry to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAt(int index)
		{
			this.AssertInstance();

			FacialAnimationSequence.DeleteSoundEntry(this.handle, index);
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
	/// Represents a collection of skeleton animations in the sequence.
	/// </summary>
	public struct FacialAnimationSequenceSkeletonAnimations
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
		/// Gets the number of skeleton animations in the facial animation sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialAnimationSequence.GetSkeletonAnimationEntryCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the skeleton animation entry.
		/// </summary>
		/// <param name="index">Zero-based index of the skeleton animation entry to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public FacialAnimationSkeletonAnimationEntry this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialAnimationSequence.GetSkeletonAnimationEntry(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSequenceSkeletonAnimations(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates and inserts a skeleton animation entry into the sequence.
		/// </summary>
		/// <param name="index">Zero-based index of the slot to insert the entry into.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Insert(int index)
		{
			this.AssertInstance();

			FacialAnimationSequence.InsertSkeletonAnimationEntry(this.handle, index);
		}
		/// <summary>
		/// Removes a skeleton animation entry into the sequence.
		/// </summary>
		/// <param name="index">Zero-based index of the the entry to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAt(int index)
		{
			this.AssertInstance();

			FacialAnimationSequence.DeleteSkeletonAnimationEntry(this.handle, index);
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
	/// Represents a sequence of facial animations.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FacialAnimationSequence
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the list of channels in this sequence.
		/// </summary>
		[FieldOffset(0)] public readonly FacialAnimationSequenceChannels Channels;
		/// <summary>
		/// Provides access to the list of sounds in this sequence.
		/// </summary>
		[FieldOffset(0)] public readonly FacialAnimationSequenceSounds Sounds;
		/// <summary>
		/// Provides access to the list of skeleton animations in this sequence.
		/// </summary>
		[FieldOffset(0)] public readonly FacialAnimationSequenceSkeletonAnimations SkeletonAnimations;
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
		/// Gets or sets the name of the sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the name of the file that provides the joystick data.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string JoystickFile
		{
			get
			{
				this.AssertInstance();

				return GetJoystickFile(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetJoystickFile(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationSequenceFlags Flags
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
		/// Gets or sets a range of time when this sequence plays.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public RangeSingle TimeRange
		{
			get
			{
				this.AssertInstance();

				return GetTimeRange(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetTimeRange(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a value that indicates whether this sequence should be loaded into memory.
		/// </summary>
		/// <remarks>Changing this value causes asynchronous process of streaming in and out.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool InMemory
		{
			get
			{
				this.AssertInstance();

				return IsInMemory(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetInMemory(this.handle, value);
			}
		}
		/// <summary>
		/// Gets a spline that specifies position of the camera during the sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEngineSpline CameraPosition
		{
			get
			{
				this.AssertInstance();

				return GetCameraPathPosition(this.handle);
			}
		}
		/// <summary>
		/// Gets a spline that specifies orientation of the camera during the sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEngineSpline CameraOrientation
		{
			get
			{
				this.AssertInstance();

				return GetCameraPathOrientation(this.handle);
			}
		}
		/// <summary>
		/// Gets a spline that specifies field-of-view of the camera during the sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEngineSpline CameraFieldOfView
		{
			get
			{
				this.AssertInstance();

				return GetCameraPathFOV(this.handle);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSequence(IntPtr handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increments the reference count of this sequence. Call this when you get an extra reference to
		/// it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decrements the reference count of this sequence. Call this when you no longer have an extra
		/// reference to it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			this.AssertInstance();

			Release(this.handle);
		}
		/// <summary>
		/// Initiates streaming of this animation sequence(?).
		/// </summary>
		/// <param name="file">File to use for streaming(?).</param>
		/// <returns>True, if successful(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StartStreaming(string file)
		{
			this.AssertInstance();

			return StartStreamingInternal(this.handle, file);
		}
		/// <summary>
		/// Saves this sequence to the Xml node.
		/// </summary>
		/// <param name="node"> 
		/// An Xml node to use as a root for Xml data that represents this sequence.
		/// </param>
		/// <param name="flags">A set of flags that specify which parts of this sequence to save.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Save(CryXmlNode node,
						 FacialAnimationSequenceSerializationFlags flags = FacialAnimationSequenceSerializationFlags.All)
		{
			this.AssertInstance();

			Serialize(this.handle, node.Handle, false, flags);
		}
		/// <summary>
		/// Merges another sequence into this one.
		/// </summary>
		/// <param name="other">    Another sequence.</param>
		/// <param name="overwrite">
		/// Indicates whether data from another sequence should take precendence in case of conflicts.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Merge(FacialAnimationSequence other, bool overwrite)
		{
			this.AssertInstance();

			MergeSequence(this.handle, other, overwrite);
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
		private static extern bool StartStreamingInternal(IntPtr handle, string sFilename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetName(IntPtr handle, string sNewName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, FacialAnimationSequenceFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequenceFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RangeSingle GetTimeRange(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTimeRange(IntPtr handle, RangeSingle range);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetChannelCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialAnimationChannel GetChannel(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialAnimationChannel CreateChannel(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialAnimationChannel CreateChannelGroup(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveChannel(IntPtr handle, FacialAnimationChannel pChannel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSoundEntryCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InsertSoundEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DeleteSoundEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialAnimationSoundEntry GetSoundEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSkeletonAnimationEntryCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InsertSkeletonAnimationEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DeleteSkeletonAnimationEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern FacialAnimationSkeletonAnimationEntry GetSkeletonAnimationEntry(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetJoystickFile(IntPtr handle, string joystickFile);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetJoystickFile(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Serialize(IntPtr handle, IntPtr xmlNode, bool bLoading,
											 FacialAnimationSequenceSerializationFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MergeSequence(IntPtr handle, FacialAnimationSequence pMergeSequence, bool overwrite);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEngineSpline GetCameraPathPosition(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEngineSpline GetCameraPathOrientation(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEngineSpline GetCameraPathFOV(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInMemory(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInMemory(IntPtr handle, bool bInMemory);
		#endregion
	}
}