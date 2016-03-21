using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a sound that plays during facial animation.
	/// </summary>
	public struct FacialAnimationSoundEntry
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
		/// Gets or sets the name of the file to use the sound from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string File
		{
			get
			{
				this.AssertInstance();

				return GetSoundFile(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetSoundFile(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the sentence this entry represents.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialSentence Sentence
		{
			get
			{
				this.AssertInstance();

				return GetSentence(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the moment when this sound starts playing.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float StartTime
		{
			get
			{
				this.AssertInstance();

				return GetStartTime(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetStartTime(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSoundEntry(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
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
		private static extern void SetSoundFile(IntPtr handle, string sSoundFile);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetSoundFile(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialSentence GetSentence(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetStartTime(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStartTime(IntPtr handle, float time);
		#endregion
	}
}