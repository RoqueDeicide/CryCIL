using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Geometry.Splines
{
	/// <summary>
	/// Represents a spline that is implemented in CryEngine.
	/// </summary>
	/// <remarks>
	/// This type is a wrapper for pointers to objects that implement ISplineInterpolator interface.
	/// </remarks>
	public struct CryEngineSpline : ISpline<float, Vector4>
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of valid vector components.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int DimensionCount
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("Instance object is invalid.");
				}

				return GetNumDimensionsInternal(this.handle);
			}
		}
		/// <summary>
		/// Gets number of keys this spline consists of.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int KeyCount
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("Instance object is invalid.");
				}

				return GetKeyCountInternal(this.handle);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="handle">A pointer to ISplineInterpolator object.</param>
		public CryEngineSpline(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Inserts a new point into the spline.
		/// </summary>
		/// <param name="time">    Time stamp where to insert the new key.</param>
		/// <param name="position">Location of the point that represents the key.</param>
		/// <returns>Zero-based index of slot that is now occupied by the given key.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int InsertKey(float time, Vector4 position)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return InsertKeyInternal(this.handle, time, position);
		}
		/// <summary>
		/// Removes a key.
		/// </summary>
		/// <param name="index">Zero-based index of the key to remove.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void RemoveKey(int index)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			RemoveKeyInternal(this.handle, index);
		}
		/// <summary>
		/// Searches for keys within given time span.
		/// </summary>
		/// <param name="startTime">    Start of the time interval to look for keys within.</param>
		/// <param name="endTime">      End of the time interval to look for keys within.</param>
		/// <param name="firstFoundKey">
		/// Zero-based index of the first found key. -1 if nothing was found.
		/// </param>
		/// <param name="numFoundKeys"> 
		/// Number of keys that were found within the time span. 0 if nothing was found.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void FindKeys(float startTime, float endTime, out int firstFoundKey, out int numFoundKeys)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			FindKeysInternal(this.handle, startTime, endTime, out firstFoundKey, out numFoundKeys);
		}
		/// <summary>
		/// Removes all keys within the time span.
		/// </summary>
		/// <param name="startTime">Start of the time interval to look for keys within.</param>
		/// <param name="endTime">  End of the time interval to look for keys within.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void RemoveKeys(float startTime, float endTime)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			RemoveKeysInternal(this.handle, startTime, endTime);
		}
		/// <summary>
		/// Relocates the key to the different time stamp.
		/// </summary>
		/// <param name="index">Zero-based index of the key to relocate.</param>
		/// <param name="time"> A new time-stamp for the key.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyTime(int index, float time)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyTimeInternal(this.handle, index, time);
		}
		/// <summary>
		/// Gets the time-stamp of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key which time-stamp to get.</param>
		/// <returns>Time-stamp of the key.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public float GetKeyTime(int index)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyTimeInternal(this.handle, index);
		}
		/// <summary>
		/// Moves the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key to move.</param>
		/// <param name="value">New position of the key.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyPosition(int index, Vector4 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyValueInternal(this.handle, index, value);
		}
		/// <summary>
		/// Gets the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <returns>Vector that represents position of the key.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public Vector4 GetKeyPosition(int index)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			Vector4 v;
			GetKeyValueInternal(this.handle, index, out v);
			return v;
		}
		/// <summary>
		/// Gets the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="value">Vector that represents position of the key.</param>
		/// <returns>
		/// True, if the index was greater then or equal to zero and less then total number of keys in the
		/// spline.
		/// </returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool GetKeyPosition(int index, out Vector4 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyValueInternal(this.handle, index, out value);
		}
		/// <summary>
		/// Sets the in-going tangent for the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tin">  New in-going tangent point of the key.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyInTangent(int index, Vector4 tin)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyInTangentInternal(this.handle, index, tin);
		}
		/// <summary>
		/// Sets the out-going tangent for the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tout"> New out-going tangent point of the key.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyOutTangent(int index, Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyOutTangentInternal(this.handle, index, tout);
		}
		/// <summary>
		/// Sets both tangents of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tin">  New in-going tangent point of the key.</param>
		/// <param name="tout"> New out-going tangent point of the key.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyTangents(int index, Vector4 tin, Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyTangentsInternal(this.handle, index, tin, tout);
		}
		/// <summary>
		/// Gets both tangents of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tin">  In-going tangent point of the key.</param>
		/// <param name="tout"> Out-going tangent point of the key.</param>
		/// <returns>
		/// True, if the index was greater then or equal to zero and less then total number of keys in the
		/// spline.
		/// </returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool GetKeyTangents(int index, out Vector4 tin, out Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyTangentsInternal(this.handle, index, out tin, out tout);
		}
		/// <summary>
		/// Gets the point along the spline at the specified time stamp.
		/// </summary>
		/// <param name="time">Time stamp at which the point is located.</param>
		/// <returns>Interpolated point.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public Vector4 Interpolate(float time)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			Vector4 v;
			InterpolateInternal(this.handle, time, out v);
			return v;
		}
		/// <summary>
		/// Enumerates the keys of this spline
		/// </summary>
		/// <returns>An object that does the enumeration.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public IEnumerator<Vector4> GetEnumerator()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			int count = GetKeyCountInternal(this.handle);
			for (int i = 0; i < count; i++)
			{
				Vector4 v;
				GetKeyValueInternal(this.handle, i, out v);
				yield return v;
			}
		}
		#endregion
		#region Utilities
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetNumDimensionsInternal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InsertKeyInternal(IntPtr handle, float time, Vector4 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveKeyInternal(IntPtr handle, int key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FindKeysInternal(IntPtr handle, float startTime, float endTime, out int firstFoundKey,
													out int numFoundKeys);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveKeysInternal(IntPtr handle, float startTime, float endTime);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetKeyCountInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeyTimeInternal(IntPtr handle, int key, float time);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetKeyTimeInternal(IntPtr handle, int key);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeyValueInternal(IntPtr handle, int key, Vector4 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyValueInternal(IntPtr handle, int key, out Vector4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeyInTangentInternal(IntPtr handle, int key, Vector4 tin);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeyOutTangentInternal(IntPtr handle, int key, Vector4 tout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetKeyTangentsInternal(IntPtr handle, int key, Vector4 tin, Vector4 tout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyTangentsInternal(IntPtr handle, int key, out Vector4 tin, out Vector4 tout);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InterpolateInternal(IntPtr handle, float time, out Vector4 value);
		#endregion
	}
}