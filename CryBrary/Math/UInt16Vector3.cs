using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace CryEngine
{
	/// <summary>
	/// Represents 3 16-bit unsigned integer numbers.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct UInt16Vector3 : IEquatable<UInt16Vector3>, IComparable<UInt16Vector3>, IEnumerable<ushort>
	{
		#region Fields
		/// <summary>
		/// First number.
		/// </summary>
		public ushort X;
		/// <summary>
		/// Second number.
		/// </summary>
		public ushort Y;
		/// <summary>
		/// Third number.
		/// </summary>
		public ushort Z;
		#endregion
		#region Properties
		/// <summary>
		/// Provides access to component of the vector specified by
		/// given index.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of the component of the vector.
		/// </param>
		/// <returns></returns>
		public ushort this[int index]
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
						throw new ArgumentOutOfRangeException
							("UInt16Vector3.Indexer: Invalid index - it should be within range 0..2");
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
						throw new ArgumentOutOfRangeException
							("UInt16Vector3.Indexer: Invalid index - it should be within range 0..2");
				}
			}
		}
		/// <summary>
		/// Provides access to component of the vector specified by
		/// given index.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of the component of the vector.
		/// </param>
		/// <returns></returns>
		public ushort this[char index]
		{
			get
			{
				switch (index)
				{
					case 'X':
					case 'x':
						return this.X;
					case 'Y':
					case 'y':
						return this.Y;
					case 'Z':
					case 'z':
						return this.Z;
					default:
						throw new ArgumentOutOfRangeException
							("UInt16Vector3.Indexer: Invalid index - use a letter that designates component to access it. (Case is ignored).");
				}
			}
			set
			{
				switch (index)
				{
					case 'X':
					case 'x':
						this.X = value;
						break;
					case 'Y':
					case 'y':
						this.Y = value;
						break;
					case 'Z':
					case 'z':
						this.Z = value;
						break;
					default:
						throw new ArgumentOutOfRangeException
							("UInt16Vector3.Indexer: Invalid index - use a letter that designates component to access it. (Case is ignored).");
				}
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of <see cref="UInt16Vector3" />.
		/// </summary>
		/// <param name="x">X-component.</param>
		/// <param name="y">Y-component.</param>
		/// <param name="z">Z-component.</param>
		public UInt16Vector3(ushort x, ushort y, ushort z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		/// <summary>
		/// Initializes new instance of <see cref="UInt16Vector3" />
		/// using values from given array.
		/// </summary>
		/// <param name="array">
		/// Array that contains values to use to initialize components
		/// of this vector.
		/// </param>
		/// <param name="startingIndex">
		/// Index of the first element in the array to use for initialization.
		/// </param>
		/// <param name="startingComponent">
		/// Index of the first component to initialize.
		/// </param>
		/// <param name="count">
		/// Number of components to initialize.
		/// </param>
		public UInt16Vector3(ushort[] array, int startingIndex, int startingComponent, int count)
		{
			Contract.Requires(array != null && array.Length >= count, "Array must be initialized and have number of elements to be at least equal to number of initialized components.");
			Contract.Requires(count > 0 && count < 4, "Number of initialized components can only be 1, 2 or 3.");
			Contract.Requires(startingIndex > -1 && startingIndex <= array.Length - count, "Index of first element of the array to copy must be more or equal to 0 and less then or equal to number of elements in the array minus number of components that will be initialized.");
			Contract.Requires(startingComponent == 3 - count, "Index of first component must be equal to 3 - number of initialized components.");

			this.X = 0;
			this.Y = 0;
			this.Z = 0;

			switch (count)
			{
				case 1:
					this[startingComponent] = array[startingIndex];
					break;
				case 2:
					this[startingComponent] = array[startingIndex];
					this[startingComponent + 1] = array[startingIndex + 1];
					break;
				case 3:
					this.X = array[startingIndex];
					this.Y = array[startingIndex + 1];
					this.Z = array[startingIndex + 2];
					break;
				default:
					break;
			}
		}
		#endregion
		#region Interface
		#region Comparisons
		/// <summary>
		/// Determines whether this vector is equal to another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>
		/// True, if this vector is equal to another one, otherwise false.
		/// </returns>
		public bool Equals(UInt16Vector3 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}
		/// <summary>
		/// Determines whether this vector is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if given object is a vector equal to this one,
		/// otherwise false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is UInt16Vector3)
			{
				return this.Equals((UInt16Vector3)obj);
			}
			return false;
		}
		/// <summary>
		/// Determines relative position of given vector and this one
		/// in a sequence sorted in ascending order.
		/// </summary>
		/// <remarks>
		/// Comparison is done by mapping each vector into 64-bit
		/// integer and comparing them.
		/// </remarks>
		/// <param name="other">Another vector.</param>
		/// <returns>
		/// <para>-1 - other vector should precede this
		/// one.</para><para>0 - other vector is equal to this
		/// one.</para><para>1 - other vector should follow this one.</para>
		/// </returns>
		public int CompareTo(UInt16Vector3 other)
		{
			us3tol1 map1 = new us3tol1();
			map1.x = this.X;
			map1.y = this.Y;
			map1.z = this.Z;
			us3tol1 map2 = new us3tol1();
			map2.x = other.X;
			map2.y = other.Y;
			map2.z = other.Z;
			return map1.l.CompareTo(map2.l);
		}
		#region Struct Used in comparison
		[StructLayout(LayoutKind.Explicit)]
		private struct us3tol1
		{
			[FieldOffset(2)]
			public ushort x;
			[FieldOffset(4)]
			public ushort y;
			[FieldOffset(6)]
			public ushort z;
			[FieldOffset(0)]
			public ulong l;
		}
		#endregion
		#endregion
		#region Enumeration
		/// <summary></summary>
		/// <returns></returns>
		public IEnumerator<ushort> GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
		}
		/// <summary></summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
		}
		#endregion
		#region Operators
		/// <summary>
		/// Determines whether two instances of <see
		/// cref="UInt16Vector3" /> struct are equal.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>
		/// True, if objects are equal, otherwise false.
		/// </returns>
		public static bool operator ==(UInt16Vector3 l, UInt16Vector3 r)
		{
			return l.X == r.X && l.Y == r.Y && l.Z == r.Z;
		}
		/// <summary>
		/// Determines whether two instances of <see
		/// cref="UInt16Vector3" /> struct are not equal.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>
		/// True, if objects are not equal, otherwise false.
		/// </returns>
		public static bool operator !=(UInt16Vector3 l, UInt16Vector3 r)
		{
			return l.X != r.X && l.Y != r.Y && l.Z != r.Z;
		}
		#endregion
		#region Simple Operations
		/// <summary>
		/// Calculates hash code of this vector.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return 17 * this.X + 83 * this.Y + 53 * this.Z;
		}
		/// <summary>
		/// Converts this vector to string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("({0};{1};{2})", this.X, this.Y, this.Z);
		}
		#endregion
		#endregion
	}
}