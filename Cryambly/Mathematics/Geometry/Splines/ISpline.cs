using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CryCil.Geometry.Splines
{
	/// <summary>
	/// Defines common functionality of types that represent splines.
	/// </summary>
	/// <typeparam name="ComponentType">
	/// Type that represents components of vectors that form this spline.
	/// </typeparam>
	/// <typeparam name="VectorType">Type of vectors the spline is defined by.</typeparam>
	internal interface ISpline<in ComponentType, VectorType> : IEnumerable<VectorType>
		where VectorType : IVector<ComponentType, VectorType>
	{
		/// <summary>
		/// Gets number of valid vector components.
		/// </summary>
		int DimensionCount { get; }
		/// <summary>
		/// Gets number of keys that comprise this spline.
		/// </summary>
		int KeyCount { get; }
		/// <summary>
		/// Inserts a new point into the spline.
		/// </summary>
		/// <param name="time">    Time stamp where to insert the new key.</param>
		/// <param name="position">Location of the point that represents the key.</param>
		/// <returns>Zero-based index of slot that is now occupied by the given key.</returns>
		int InsertKey(float time, VectorType position);
		/// <summary>
		/// Removes a key.
		/// </summary>
		/// <param name="index">Zerobased index of the key to remove.</param>
		void RemoveKey(int index);
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
		void FindKeys(float startTime, float endTime, out int firstFoundKey, out int numFoundKeys);
		/// <summary>
		/// Removes all keys within the time span.
		/// </summary>
		/// <param name="startTime">Start of the time interval to look for keys within.</param>
		/// <param name="endTime">  End of the time interval to look for keys within.</param>
		void RemoveKeys(float startTime, float endTime);

		/// <summary>
		/// Relocates the key to the different time stamp.
		/// </summary>
		/// <param name="index">Zero-based index of the key to relocate.</param>
		/// <param name="time"> A new time-stamp for the key.</param>
		void SetKeyTime(int index, float time);
		/// <summary>
		/// Gets the time-stamp of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key which time-stamp to get.</param>
		/// <returns>Time-stamp of the key.</returns>
		float GetKeyTime(int index);
		/// <summary>
		/// Moves the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key to move.</param>
		/// <param name="value">New position of the key.</param>
		void SetKeyPosition(int index, VectorType value);
		/// <summary>
		/// Gets the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <returns>Vector that represents position of the key.</returns>
		VectorType GetKeyPosition(int index);
		/// <summary>
		/// Gets the position of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="value">Vector that represents position of the key.</param>
		/// <returns>
		/// True, if the index was greater then or equal to zero and less then total number of keys in the
		/// spline.
		/// </returns>
		bool GetKeyPosition(int index, out VectorType value);

		/// <summary>
		/// Sets the in-going tangent for the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tin">  New in-going tangent point of the key.</param>
		void SetKeyInTangent(int index, VectorType tin);
		/// <summary>
		/// Sets the out-going tangent for the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tout"> New out-going tangent point of the key.</param>
		void SetKeyOutTangent(int index, VectorType tout);
		/// <summary>
		/// Sets both tangents of the key.
		/// </summary>
		/// <param name="index">Zero-based index of the key.</param>
		/// <param name="tin">  New in-going tangent point of the key.</param>
		/// <param name="tout"> New out-going tangent point of the key.</param>
		void SetKeyTangents(int index, VectorType tin, VectorType tout);
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
		bool GetKeyTangents(int index, out VectorType tin, out VectorType tout);
		/// <summary>
		/// Gets the point along the spline at the specified time stamp.
		/// </summary>
		/// <param name="time">Time stamp at which the point is located.</param>
		/// <returns>Interpolated point.</returns>
		VectorType Interpolate(float time);
	}
	[ContractClassFor(typeof(ISpline<,>))]
	internal abstract class SplineContracts<ComponentType, VectorType> : ISpline<ComponentType, VectorType> where VectorType : IVector<ComponentType, VectorType>
	{
		IEnumerator<VectorType> IEnumerable<VectorType>.GetEnumerator()
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return null;
			// ReSharper restore AssignNullToNotNullAttribute
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			// ReSharper disable AssignNullToNotNullAttribute
			return null;
			// ReSharper restore AssignNullToNotNullAttribute
		}
		int ISpline<ComponentType, VectorType>.DimensionCount
		{
			get
			{
				Contract.Ensures(Contract.Result<int>() > 0);
				return 0;
			}
		}
		int ISpline<ComponentType, VectorType>.KeyCount
		{
			get
			{
				Contract.Ensures(Contract.Result<int>() >= 0);
				return 0;
			}
		}
		int ISpline<ComponentType, VectorType>.InsertKey(float time, VectorType position)
		{
			return 0;
		}
		void ISpline<ComponentType, VectorType>.RemoveKey(int index)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		void ISpline<ComponentType, VectorType>.FindKeys(float startTime, float endTime, out int firstFoundKey, out int numFoundKeys)
		{
			firstFoundKey = 0;
			numFoundKeys = 0;
		}
		void ISpline<ComponentType, VectorType>.RemoveKeys(float startTime, float endTime)
		{
		}
		void ISpline<ComponentType, VectorType>.SetKeyTime(int index, float time)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		float ISpline<ComponentType, VectorType>.GetKeyTime(int index)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
			return 0;
		}
		void ISpline<ComponentType, VectorType>.SetKeyPosition(int index, VectorType value)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		VectorType ISpline<ComponentType, VectorType>.GetKeyPosition(int index)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
			return default(VectorType);
		}
		bool ISpline<ComponentType, VectorType>.GetKeyPosition(int index, out VectorType value)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
			value = default(VectorType);
			return false;
		}
		void ISpline<ComponentType, VectorType>.SetKeyInTangent(int index, VectorType tin)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		void ISpline<ComponentType, VectorType>.SetKeyOutTangent(int index, VectorType tout)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		void ISpline<ComponentType, VectorType>.SetKeyTangents(int index, VectorType tin, VectorType tout)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
		}
		bool ISpline<ComponentType, VectorType>.GetKeyTangents(int index, out VectorType tin, out VectorType tout)
		{
			Contract.Requires(index >= 0 && index < ((ISpline<ComponentType, VectorType>)this).KeyCount);
			tin = default(VectorType);
			tout = default(VectorType);
			return false;
		}
		VectorType ISpline<ComponentType, VectorType>.Interpolate(float time)
		{
			return default(VectorType);
		}
	}
}