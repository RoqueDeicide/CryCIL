using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Encapsulates information that is used when forcing facial skeleton joints into specific orientation.
	/// </summary>
	public struct FacialAnimationForcedOrientationEntry
	{
		/// <summary>
		/// Identifier of the joint.
		/// </summary>
		public int JointId;
		/// <summary>
		/// Orientation for the joint.
		/// </summary>
		public Quaternion Orientation;
	}
	/// <summary>
	/// Represents a layer in which the sequences are played.
	/// </summary>
	public struct FaceSequenceLayer
	{
		#region Fields
		private readonly IntPtr handle;
		private readonly FacialSequenceLayer layer;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		#endregion
		#region Interface
		/// <summary>
		/// Plays an animation sequence on this layer.
		/// </summary>
		/// <param name="sequence"> An object that represents the sequence that needs to be played.</param>
		/// <param name="exclusive">
		/// Indicates whether the sequence must override all other sequences(?).
		/// </param>
		/// <param name="looping">  Indicates whether the sequence must loop itself.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Play(FacialAnimationSequence sequence, bool exclusive = false, bool looping = false)
		{
			this.AssertInstance();

			Face.PlaySequence(this.handle, sequence, this.layer, exclusive, looping);
		}
		/// <summary>
		/// Stops currently playing sequence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Stop()
		{
			this.AssertInstance();

			Face.StopSequence(this.handle, this.layer);
		}
		/// <summary>
		/// Indicates whether a sequence is playing in this layer.
		/// </summary>
		/// <param name="sequence">Sequence to check.</param>
		/// <returns>True, if given sequence is currently playing(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsPlaying(FacialAnimationSequence sequence)
		{
			this.AssertInstance();

			return Face.IsPlaySequence(this.handle, sequence, this.layer);
		}
		/// <summary>
		/// Changes playback state on this layer.
		/// </summary>
		/// <param name="pause">Indicates whether animation must be paused.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ChangeState(bool pause)
		{
			this.AssertInstance();

			Face.PauseSequence(this.handle, this.layer, pause);
		}
		/// <summary>
		/// Pauses the animation on this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Pause()
		{
			this.AssertInstance();

			Face.PauseSequence(this.handle, this.layer, true);
		}
		/// <summary>
		/// Resumes the animation on this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Resume()
		{
			this.AssertInstance();

			Face.PauseSequence(this.handle, this.layer, false);
		}
		/// <summary>
		/// Moves the position of currently playing sequence to one specified.
		/// </summary>
		/// <param name="time">Time offset since start of the sequence to move to.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Seek(float time)
		{
			this.AssertInstance();

			Face.SeekSequence(this.handle, this.layer, time);
		}
		#endregion
		#region Construction
		internal FaceSequenceLayer(IntPtr handle, FacialSequenceLayer layer)
		{
			this.handle = handle;
			this.layer = layer;
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
	/// Represents a collection of sequences that are organized in layers.
	/// </summary>
	public struct FaceSequences
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
		/// Gets the layer of facial animation that is used to play animation sequences.
		/// </summary>
		/// <param name="layer">Identifier of the layer.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FaceSequenceLayer this[FacialSequenceLayer layer]
		{
			get
			{
				this.AssertInstance();

				return new FaceSequenceLayer(this.handle, layer);
			}
		}
		#endregion
		#region Construction
		internal FaceSequences(IntPtr handle)
		{
			this.handle = handle;
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
	/// Represents an animated face.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct Face
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to this face's layers that are used to play animation sequences.
		/// </summary>
		[FieldOffset(0)] public readonly FaceSequences Sequences;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets the model of this face.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialModel Model
		{
			get
			{
				this.AssertInstance();

				return GetFacialModel(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that provides information about the state of all effectors that are not groups.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FaceState State
		{
			get
			{
				this.AssertInstance();

				return GetFaceState(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether procedural facial animation is enabled for this
		/// instance.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool ProceduralAnimationEnabled
		{
			get
			{
				this.AssertInstance();

				return IsProceduralFacialAnimationEnabled(this.handle);
			}
			set
			{
				this.AssertInstance();

				EnableProceduralFacialAnimation(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the array of objects that provides information that forces specific joints into specific
		/// orientations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationForcedOrientationEntry[] ForcedOrientations
		{
			set
			{
				this.AssertInstance();

				if (value == null)
				{
					return;
				}

				fixed (FacialAnimationForcedOrientationEntry* entries = value)
				{
					try
					{
						SetForcedRotations(this.handle, value.Length, entries);
					}
					catch (OverflowException)
					{
					}
				}
			}
		}
		/// <summary>
		/// Sets the master character instance for this face.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Character MasterCharacter
		{
			set
			{
				this.AssertInstance();

				SetMasterCharacter(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal Face(IntPtr handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Begins animation of the effector in the channel.
		/// </summary>
		/// <param name="effector">        Effector to begin the animation with.</param>
		/// <param name="weight">          Strength of the effector.</param>
		/// <param name="fadeTime">        The time it takes to fade in the effector in seconds(?).</param>
		/// <param name="lifeTime">        
		/// The time the channel will be active for in seconds(?). Default value of zero means infinite
		/// time.
		/// </param>
		/// <param name="repeatitionCount">
		/// Number of times the animation must repeat itself. Default value of zero means infinite
		/// repetitions.
		/// </param>
		/// <returns>Identifier of the channel.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// Facial effector cannot be <see langword="null"/>.
		/// </exception>
		public uint BeginEffectorChannel(FacialEffector effector, float weight, float fadeTime, float lifeTime = 0,
										 int repeatitionCount = 0)
		{
			this.AssertInstance();
			if (!effector.IsValid)
			{
				throw new ArgumentNullException(nameof(effector), "Facial effector cannot be null.");
			}

			return StartEffectorChannel(this.handle, effector, weight, fadeTime, lifeTime, repeatitionCount);
		}
		/// <summary>
		/// Ends animation of the channel.
		/// </summary>
		/// <param name="channelId">  
		/// Identifier of the channel that was returned by <see cref="BeginEffectorChannel"/>.
		/// </param>
		/// <param name="fadeOutTime">
		/// The time it takes for the animation to fade out and stop in seconds(?).
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EndEffectorChannel(uint channelId, float fadeOutTime = 0)
		{
			this.AssertInstance();

			StopEffectorChannel(this.handle, channelId, fadeOutTime);
		}
		/// <summary>
		/// Loads a sequence from the file.
		/// </summary>
		/// <param name="file"> The path to the file to load the sequence from.</param>
		/// <param name="cache">Indicates whether the sequence must be cached.</param>
		/// <returns>
		/// An object that represents the loaded sequence. It won't be valid if loading failed.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationSequence Load(string file, bool cache = true)
		{
			this.AssertInstance();

			return LoadSequence(this.handle, file, cache);
		}
		/// <summary>
		/// Precaches an animation sequence.
		/// </summary>
		/// <param name="file">Path to the file with a sequence.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Precache(string file)
		{
			this.AssertInstance();

			PrecacheFacialExpression(this.handle, file);
		}
		/// <summary>
		/// Starts/stops syncing character's lips with sound.
		/// </summary>
		/// <param name="soundId">Identifier of sound.</param>
		/// <param name="stop">   Indicates whether syncing should be stopped rather then started.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ChangeLipSync(uint soundId, bool stop)
		{
			this.AssertInstance();

			LipSyncWithSound(this.handle, soundId, stop);
		}
		/// <summary>
		/// Starts syncing character's lips with sound.
		/// </summary>
		/// <param name="soundId">Identifier of sound.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StartLipSync(uint soundId)
		{
			this.AssertInstance();

			LipSyncWithSound(this.handle, soundId, false);
		}
		/// <summary>
		/// Stops syncing character's lips with sound.
		/// </summary>
		/// <param name="soundId">Identifier of sound.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StopLipSync(uint soundId)
		{
			this.AssertInstance();

			LipSyncWithSound(this.handle, soundId, true);
		}
		/// <summary>
		/// Stops all animation sequences and channels.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StopAllAnimations()
		{
			this.AssertInstance();

			StopAllSequencesAndChannels(this.handle);
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
		private static extern FacialModel GetFacialModel(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FaceState GetFaceState(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint StartEffectorChannel(IntPtr handle, FacialEffector pEffector, float fWeight,
														float fFadeTime, float fLifeTime, int nRepeatCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopEffectorChannel(IntPtr handle, uint nChannelID, float fFadeOutTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequence LoadSequence(IntPtr handle, string sSequenceName, bool addToCache);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrecacheFacialExpression(IntPtr handle, string sSequenceName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PlaySequence(IntPtr handle, FacialAnimationSequence pSequence, FacialSequenceLayer layer,
												 bool bExclusive, bool bLooping);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopSequence(IntPtr handle, FacialSequenceLayer layer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsPlaySequence(IntPtr handle, FacialAnimationSequence pSequence, FacialSequenceLayer layer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PauseSequence(IntPtr handle, FacialSequenceLayer layer, bool bPaused);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SeekSequence(IntPtr handle, FacialSequenceLayer layer, float fTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LipSyncWithSound(IntPtr handle, uint nSoundId, bool bStop);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableProceduralFacialAnimation(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsProceduralFacialAnimationEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetForcedRotations(IntPtr handle, int numForcedRotations,
													  FacialAnimationForcedOrientationEntry* forcedRotations);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMasterCharacter(IntPtr handle, Character pMasterInstance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopAllSequencesAndChannels(IntPtr handle);
		#endregion
	}
}