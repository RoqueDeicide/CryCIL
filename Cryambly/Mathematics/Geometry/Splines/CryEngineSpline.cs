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
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int InsertKey(float time, Vector4 position)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return InsertKeyInternal(this.handle, time, position);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void RemoveKey(int index)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			RemoveKeyInternal(this.handle, index);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void FindKeys(float startTime, float endTime, out int firstFoundKey, out int numFoundKeys)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			FindKeysInternal(this.handle, startTime, endTime, out firstFoundKey, out numFoundKeys);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void RemoveKeys(float startTime, float endTime)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			RemoveKeysInternal(this.handle, startTime, endTime);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyTime(int index, float time)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyTimeInternal(this.handle, index, time);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public float GetKeyTime(int index)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyTimeInternal(this.handle, index);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyPosition(int index, Vector4 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyValueInternal(this.handle, index, value);
		}
		/// <inheritdoc/>
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
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool GetKeyPosition(int index, out Vector4 value)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyValueInternal(this.handle, index, out value);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyInTangent(int index, Vector4 tin)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyInTangentInternal(this.handle, index, tin);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyOutTangent(int index, Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyOutTangentInternal(this.handle, index, tout);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void SetKeyTangents(int index, Vector4 tin, Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			SetKeyTangentsInternal(this.handle, index, tin, tout);
		}
		/// <inheritdoc/>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool GetKeyTangents(int index, out Vector4 tin, out Vector4 tout)
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("Instance object is invalid.");
			}

			return GetKeyTangentsInternal(this.handle, index, out tin, out tout);
		}
		/// <inheritdoc/>
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