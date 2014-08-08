using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CryEngine.Mathematics
{
	/// <summary>
	/// Encapsulates 3 32-bit signed integer numbers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Int32Vector3 : IEnumerable<int>
	{
		#region Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public int X;
		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public int Y;
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public int Z;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the X, Y or Z component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the X component, 1 for the Y component,
		/// 2 for the Z component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Attempt to access vector component other then X, Y or Z.
		/// </exception>
		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.X;
					case 1:
						return this.Y;
					case 2:
						return this.Z;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector" +
																	   " component other then X, Y or Z.");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						this.X = value;
						break;
					case 1:
						this.Y = value;
						break;
					case 2:
						this.Z = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access vector component" +
																	   " other then X, Y or Z.");
				}
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Creates new <see cref="Int32Vector3" /> with specified components.
		/// </summary>
		/// <param name="vx">X-component of new vector.</param>
		/// <param name="vy">Y-component of new vector.</param>
		/// <param name="vz">Z-component of new vector.</param>
		public Int32Vector3(int vx, int vy, int vz)
			: this()
		{
			X = vx;
			Y = vy;
			Z = vz;
		}
		/// <summary>
		/// Creates new <see cref="Int32Vector3" />.
		/// </summary>
		/// <param name="f">
		/// <see cref="Int32" /> value to assign to all components of new vector.
		/// </param>
		public Int32Vector3(int f)
			: this()
		{
			X = Y = Z = f;
		}
		/// <summary>
		/// Creates new <see cref="Int32Vector3" />.
		/// </summary>
		/// <param name="values">A list of integer numbers which specify new vector.</param>
		public Int32Vector3(IList<int> values)
			: this()
		{
			if (values == null) return;
			for (int i = 0; i < 3 || i < values.Count; i++)
			{
				this[i] = values[i];
			}
		}
		/// <summary>
		/// Creates new <see cref="Vector3" />.
		/// </summary>
		/// <param name="values">
		/// A <see cref="Dictionary{TKey,TValue}" /> which is used to initialize new vector.
		/// </param>
		public Int32Vector3(IDictionary<string, int> values)
			: this()
		{
			if (values == null || values.Count == 0) return;
			if (values.ContainsKey("X"))
			{
				this.X = values["X"];
			}
			if (values.ContainsKey("Y"))
			{
				this.X = values["Y"];
			}
			if (values.ContainsKey("Z"))
			{
				this.X = values["Z"];
			}
		}
		#endregion
		/// <summary>
		/// Enumerates components of this vector.
		/// </summary>
		public IEnumerator<int> GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}