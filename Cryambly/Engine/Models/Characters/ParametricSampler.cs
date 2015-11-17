using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.Characters
{
	internal unsafe struct ParametricSamplerInternals
	{
		internal void* vtable;
		internal byte m_nParametricType; //Type of Group: i.e. I2M, M2I, MOVE, Idle-Step, Idle-Rot, etc....
		internal byte m_numDimensions; //how many dimensions are used in this Parametric Group
		internal fixed float m_MotionParameter [4]; //we have only 4 dimensions per blend-space
		internal fixed byte m_MotionParameterID [4]; //we have only 4 dimensions per blend-space
		internal fixed byte m_MotionParameterFlags [4]; //we have only 4 dimensions per blend-space
	}
	/// <summary>
	/// Enumeration of flags that specifies dimensions of the parametric group in
	/// <see cref="ParametricSampler"/>.
	/// </summary>
	[Flags]
	public enum DimensionFlags : byte
	{
		/// <summary>
		/// When set, specifies that this dimension is initialized.
		/// </summary>
		Initialized = 0x001,
		/// <summary>
		/// When set, specifies that this dimension is locked.
		/// </summary>
		LockedParameter = 0x002,
		/// <summary>
		/// When set, specifies that this dimension stores a delta value(?).
		/// </summary>
		DeltaExtraction = 0x004
	}
	/// <summary>
	/// Enumeration of identifiers of motion parameters.
	/// </summary>
	public enum MotionParameterId : byte
	{
		/// <summary>
		/// Parameter contains the travel speed.
		/// </summary>
		TravelSpeed = 0,
		/// <summary>
		/// Parameter contains the turn speed.
		/// </summary>
		TurnSpeed,
		/// <summary>
		/// Parameter contains the travel angle: forward, backwards and sidestepping.
		/// </summary>
		TravelAngle,
		/// <summary>
		/// Parameter contains the travel slope.
		/// </summary>
		TravelSlope,
		/// <summary>
		/// Parameter contains the turn angle that is used with Idle2Move transitions and idle-rotations.
		/// </summary>
		TurnAngle,
		/// <summary>
		/// Parameter contains the travel distance for idle-steps.
		/// </summary>
		TravelDist,
		/// <summary>
		/// Parameter contains the stop leg info for Move2Idle transitions.
		/// </summary>
		StopLeg,

		/// <summary>
		/// Parameter contains the first blend weight.
		/// </summary>
		BlendWeight,
		/// <summary>
		/// Parameter contains the second blend weight.
		/// </summary>
		BlendWeight2,
		/// <summary>
		/// Parameter contains the third blend weight.
		/// </summary>
		BlendWeight3,
		/// <summary>
		/// Parameter contains the fourth blend weight.
		/// </summary>
		BlendWeight4,
		/// <summary>
		/// Parameter contains the fifth blend weight.
		/// </summary>
		BlendWeight5,
		/// <summary>
		/// Parameter contains the sixth blend weight.
		/// </summary>
		BlendWeight6,
		/// <summary>
		/// Parameter contains the seventh blend weight.
		/// </summary>
		BlendWeight7,
		/// <summary>
		/// Identifier of the last parameter that contains a blend weight.
		/// </summary>
		BlendWeightLast = BlendWeight7,

		/// <summary>
		/// Number of parameter identifiers.
		/// </summary>
		Count
	}
	/// <summary>
	/// Represents a parameter sampler that is used to blend animations.
	/// </summary>
	public unsafe struct ParametricSampler
	{
		#region Fields
		private readonly ParametricSamplerInternals* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != null; }
		}
		/// <summary>
		/// Gets the type of this parametric group: i.e. I2M, M2I, MOVE, Idle-Step, Idle-Rot, etc...
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public byte ParametricType
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return this.handle->m_nParametricType;
			}
		}
		/// <summary>
		/// Gets the number of dimensions that are used by this parametric group.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public byte DimensionCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return this.handle->m_numDimensions;
			}
		}
		/// <summary>
		/// Gets the index of the animation segment this sampler is currently on.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public byte CurrentSegmentIndex
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetCurrentSegmentIndexBSpace(this.handle);
			}
		}
		#endregion
		#region Construction
		internal ParametricSampler(IntPtr handle)
		{
			this.handle = (ParametricSamplerInternals*)handle.ToPointer();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the motion parameter value.
		/// </summary>
		/// <param name="index">Zero-based index of the dimension to get the parameter from.</param>
		/// <returns>Current value of the motion parameter.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the dimension must be in range from 0 to <see cref="DimensionCount"/> non-inclusively.
		/// </exception>
		public float GetMotionParameter(int index)
		{
			this.AssertInstance();
			if (index < 0 | index >= this.DimensionCount)
			{
				throw new IndexOutOfRangeException("Index of the dimension must be in range from 0 to 3 inclusively.");
			}
			Contract.EndContractBlock();

			return this.handle->m_MotionParameter[index];
		}
		/// <summary>
		/// Gets the identifier of motion parameter.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of the dimension to get the parameter identifier from.
		/// </param>
		/// <returns>Current identifier of the motion parameter.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the dimension must be in range from 0 to <see cref="DimensionCount"/> non-inclusively.
		/// </exception>
		public MotionParameterId GetMotionParameterId(int index)
		{
			this.AssertInstance();
			if (index < 0 | index >= this.DimensionCount)
			{
				throw new IndexOutOfRangeException("Index of the dimension must be in range from 0 to 3 inclusively.");
			}
			Contract.EndContractBlock();

			return (MotionParameterId)this.handle->m_MotionParameterID[index];
		}
		/// <summary>
		/// Gets a set of flags that are assigned to the motion parameter.
		/// </summary>
		/// <param name="index">Zero-based index of the dimension to get the parameter flags from.</param>
		/// <returns>Current flags that are assigned to the motion parameter.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the dimension must be in range from 0 to <see cref="DimensionCount"/> non-inclusively.
		/// </exception>
		public DimensionFlags GetMotionParameterFlags(int index)
		{
			this.AssertInstance();
			if (index < 0 | index >= this.DimensionCount)
			{
				throw new IndexOutOfRangeException("Index of the dimension must be in range from 0 to 3 inclusively.");
			}
			Contract.EndContractBlock();

			return (DimensionFlags)this.handle->m_MotionParameterFlags[index];
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
		private static extern byte GetCurrentSegmentIndexBSpace(ParametricSamplerInternals* handle);
		#endregion
	}
}