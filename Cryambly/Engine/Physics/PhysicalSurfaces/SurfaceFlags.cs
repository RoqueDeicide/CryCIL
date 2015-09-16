using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	//manually_breakable = 0x400,
	//matbreakable_bit = 16

	/// <summary>
	/// Creates objects of type <see cref="SurfaceFlags"/> that store pierceability value inside.
	/// </summary>
	public struct SurfacePierceabilityValueFlag
	{
		/// <summary>
		/// Creates an object of type <see cref="SurfaceFlags"/> that has provided pierceability value
		/// stored inside.
		/// </summary>
		/// <param name="value">
		/// A pierceablity to store inside the new object. It will be clamped into range from 0 to 15 by
		/// using bitwise AND operation with 0x0F as second operand.
		/// </param>
		/// <returns>
		/// A new object of type <see cref="SurfaceFlags"/> that can be combine with other instances of
		/// that type.
		/// </returns>
		public SurfaceFlags this[uint value]
		{
			get { return new SurfaceFlags(value & SurfaceFlags.pierceableMask); }
		}
	}
	/// <summary>
	/// Represents a quasi-enumeration that is used to specify flags that are set for physical surfaces.
	/// </summary>
	/// <example>
	/// The following code creates a set of flags that describes a surface that has pierceability of 6 and
	/// is 'important'.
	/// <code>
	/// SurfaceFlags flags = SurfaceFlags.Important | SurfaceFlags.Pierceability[6];
	/// </code>
	/// </example>
	[StructLayout(LayoutKind.Sequential)]
	public struct SurfaceFlags
	{
		#region Constants
		internal const uint important = 0x200;
		internal const uint pierceableMask = 0x0F;
		/// <summary>
		/// When set, specifies that this surface is 'important'. Pierceable ray hits through surfaces with
		/// this flag set a placed before other pierceable hits when casting a ray with a flag
		/// <see cref="RayCastFlags.SeparateImportantHits"/> set.
		/// </summary>
		public static readonly SurfaceFlags Important = new SurfaceFlags(important);
		/// <summary>
		/// Creates objects of type <see cref="SurfaceFlags"/> that store pierceability value inside.
		/// </summary>
		public static readonly SurfacePierceabilityValueFlag Pierceability;
		/// <summary>
		/// An object that can be combined with other surface flags to specify that the surface has maximal
		/// pierceability.
		/// </summary>
		public static readonly SurfaceFlags MaxPierceability = new SurfaceFlags(Pierceability[pierceableMask]);
		#endregion
		#region Fields
		private readonly uint value;
		#endregion
		#region Construction
		internal SurfaceFlags(uint value)
		{
			this.value = value;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Converts an object of this type to unsigned 32-bit number.
		/// </summary>
		/// <param name="value">A value to convert to unsigned 32-bit number.</param>
		/// <returns>Resultant number.</returns>
		public static implicit operator uint(SurfaceFlags value)
		{
			return value.value;
		}
		/// <summary>
		/// Combines 2 sets of flags.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>A combination of 2 sets of flags.</returns>
		public static SurfaceFlags operator |(SurfaceFlags left, SurfaceFlags right)
		{
			return new SurfaceFlags(left.value | right.value);
		}
		#endregion
	}
}