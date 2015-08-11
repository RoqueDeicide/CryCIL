using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine
{
	/// <summary>
	/// Enumerates the database of loaded particle effects.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class ParticleEffectEnumerator : IEnumerator<ParticleEffect>
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		private ParticleEffect current;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current particle effect.
		/// </summary>
		public ParticleEffect Current
		{
			get { return this.current; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		public ParticleEffectEnumerator()
		{
			this.handle = Create();
		}
		/// <summary>
		/// Releases the underlying object if it wasn't already.
		/// </summary>
		~ParticleEffectEnumerator()
		{
			this.Dispose();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this enumerator.
		/// </summary>
		public void Dispose()
		{
			if (this.handle != IntPtr.Zero)
			{
				Delete(this.handle);
				this.handle = IntPtr.Zero;
			}
		}
		/// <summary>
		/// Moves to the next particle effect in the database.
		/// </summary>
		/// <returns>
		/// False, if this enumerator has already passed the last particle effect in the database.
		/// </returns>
		public bool MoveNext()
		{
			this.current = Next(this.handle);
			return this.current.IsValid;
		}
		/// <summary>
		/// Resets this enumerator so it can enumerate from the start of the database.
		/// </summary>
		public void Reset()
		{
			this.Dispose();
			this.handle = Create();
		}
		#endregion
		#region Utilities
		object IEnumerator.Current
		{
			get { return this.Current; }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Delete(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect Next(IntPtr handle);
		#endregion
	}
}