using System;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Represents a randomized single-precision floating point number.
	/// </summary>
	public struct RandomizedSingle
	{
		#region Fields
		private float @base;
		private float range;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets base value.
		/// </summary>
		public float Base
		{
			get { return this.@base; }
			set { this.@base = value; }
		}
		/// <summary>
		/// Gets or sets a value between 0 and 1 that represents maximal variation of the base value.
		/// </summary>
		public float Range
		{
			get { return this.range; }
			set { this.range = MathHelpers.Clamp(value, 0.0f, 1.0f); }
		}
		/// <summary>
		/// Gets next random floating-point number.
		/// </summary>
		public float Value => (float)(new Random().NextDouble() * this.range + this.@base);
		#endregion
		#region Interface
		/// <summary>
		/// Creates a value of type <see cref="RandomizedSingle"/> without randomization.
		/// </summary>
		/// <param name="base">A base value.</param>
		/// <returns>
		/// A new object of type <see cref="RandomizedSingle"/> with randomization range set to 0.
		/// </returns>
		public static implicit operator RandomizedSingle(float @base)
		{
			return new RandomizedSingle {Base = @base};
		}
		#endregion
	}
	/// <summary>
	/// Represents a randomized unsigned single-precision floating point number that cannot be bigger then
	/// <see cref="int.MaxValue"/>.
	/// </summary>
	public struct RandomizedUSingle
	{
		#region Fields
		private float @base;
		private float range;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets base value.
		/// </summary>
		public float Base
		{
			get { return this.@base; }
			set { this.@base = MathHelpers.Clamp(value, 0.0f, int.MaxValue); }
		}
		/// <summary>
		/// Gets or sets a value between 0 and 1 that represents maximal variation of the base value.
		/// </summary>
		public float Range
		{
			get { return this.range; }
			set { this.range = MathHelpers.Clamp(value, 0.0f, 1.0f); }
		}
		/// <summary>
		/// Gets next random floating-point number.
		/// </summary>
		public float Value => (float)(new Random().NextDouble() * this.range + this.@base);
		#endregion
		#region Interface
		/// <summary>
		/// Creates a value of type <see cref="RandomizedUSingle"/> without randomization.
		/// </summary>
		/// <param name="base">A base value.</param>
		/// <returns>
		/// A new object of type <see cref="RandomizedUSingle"/> with randomization range set to 0.
		/// </returns>
		public static implicit operator RandomizedUSingle(float @base)
		{
			return new RandomizedUSingle {Base = @base};
		}
		#endregion
	}
}