using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Provides an incredibly quick access to columns in the object
	/// of type <see cref="Matrix34" />.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct Column34 : IConvertibleTo<Vector3>
	{
		/// <summary>
		/// First element in the column from the top.
		/// </summary>
		[FieldOffset(0)]
		public float ElementAtRow0;
		/// <summary>
		/// Second element in the column from the top.
		/// </summary>
		[FieldOffset(16)]
		public float ElementAtRow1;
		/// <summary>
		/// Third element in the column from the top.
		/// </summary>
		[FieldOffset(32)]
		public float ElementAtRow2;
		/// <summary>
		/// Converts this vector to a compact vector.
		/// </summary>
		/// <param name="output">
		/// Vector that contains all 3 elements of this column.
		/// </param>
		public void ConvertTo(out Vector3 output)
		{
			output = new Vector3(this.ElementAtRow0, this.ElementAtRow1, this.ElementAtRow2);
		}
	}
}