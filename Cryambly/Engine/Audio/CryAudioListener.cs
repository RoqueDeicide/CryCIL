using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Logic;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Represents a pointer to the object that represents the listener to sounds in the game.
	/// </summary>
	public struct CryAudioListener
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
		/// Gets the identifier of the entity that is associated with this listener.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityId Id
		{
			get
			{
				this.AssertInstance();

				return GetId(this.handle);
			}
		}
		/// <summary>
		/// Gets of sets the value that indicates whether this listener is active.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Active
		{
			get
			{
				this.AssertInstance();

				return GetActive(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetActive(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the value that indicates whether this listener had been relocated during last frame(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Moved
		{
			get
			{
				this.AssertInstance();

				return GetMoved(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this listener is inside an indoor area.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Inside
		{
			get
			{
				this.AssertInstance();

				return GetInside(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetInside(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the sound recording level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float RecordLevel
		{
			get
			{
				this.AssertInstance();

				return GetRecordLevel(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetRecordLevel(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the position of this listener.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Position
		{
			get
			{
				this.AssertInstance();

				return GetPosition(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetPosition(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets the forward direction of this listener.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Forward
		{
			get
			{
				this.AssertInstance();

				Vector3 result;
				GetForward(this.handle, out result);
				return result;
			}
		}
		/// <summary>
		/// Gets the up direction of this listener(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Top
		{
			get
			{
				this.AssertInstance();

				return GetTop(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the velocity of this listener.
		/// </summary>
		/// <remarks>Used for Doppler effect(?).</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Velocity
		{
			get
			{
				this.AssertInstance();

				Vector3 velocity;
				GetVelocity(this.handle, out velocity);
				return velocity;
			}
			set
			{
				this.AssertInstance();

				SetVelocity(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets the 3x4 matrix that represents transformation of this listener.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 Matrix
		{
			get
			{
				this.AssertInstance();

				Matrix34 matrix;
				GetMatrix(this.handle, out matrix);
				return matrix;
			}
			set
			{
				this.AssertInstance();

				SetMatrix(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets the depth of the water the listener is in(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float UnderWater
		{
			get
			{
				this.AssertInstance();

				return GetUnderwater(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetUnderwater(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CryAudioListener(IntPtr handle)
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
		private static extern EntityId GetId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetActive(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetActive(IntPtr handle, bool bActive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetMoved(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetInside(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInside(IntPtr handle, bool bInside);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRecordLevel(IntPtr handle, float fRecordLevel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetRecordLevel(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetPosition(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPosition(IntPtr handle, ref Vector3 rPosition);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetForward(IntPtr handle, out Vector3 forward);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetTop(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetVelocity(IntPtr handle, out Vector3 velocity);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVelocity(IntPtr handle, ref Vector3 vVel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMatrix(IntPtr handle, ref Matrix34 newTransformation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMatrix(IntPtr handle, out Matrix34 matrix);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetUnderwater(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetUnderwater(IntPtr handle, float fUnder);
		#endregion
	}
}