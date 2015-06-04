using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumerates the surface types.
	/// </summary>
	public struct SurfaceTypeEnumerator : IEnumerator<SurfaceType>
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		private SurfaceType current;

		private static readonly SurfaceType zero = new SurfaceType();
		#endregion
		#region Properties
		/// <summary>
		/// Gets the current surface type.
		/// </summary>
		public SurfaceType Current
		{
			get { return this.current; }
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
				Release(this.handle);
				this.handle = IntPtr.Zero;
				this.current = new SurfaceType();
			}
		}
		/// <summary>
		/// Moves to the next surface type in the collection.
		/// </summary>
		/// <returns>False, if we passed the end of the collection.</returns>
		public bool MoveNext()
		{
			if (this.handle == IntPtr.Zero)
			{
				this.Reset();
				this.current = GetFirst(this.handle);
			}
			else
			{
				this.current = GetNext(this.handle);
			}
			return this.current != zero;
		}
		/// <summary>
		/// Resets this enumerator.
		/// </summary>
		public void Reset()
		{
			this.handle = Init();
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Init();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceType GetFirst(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceType GetNext(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceType Release(IntPtr handle);
		object IEnumerator.Current
		{
			get { return Current; }
		}
		#endregion
	}
}