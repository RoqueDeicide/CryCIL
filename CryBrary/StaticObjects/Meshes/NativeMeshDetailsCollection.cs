using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using CryEngine.Native;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Base class for native mesh detail collections.
	/// </summary>
	/// <typeparam name="ElementType">Type of elements in the collection.</typeparam>
	public abstract class NativeMeshDetailsCollection<ElementType> : IMeshDetailsCollection<ElementType>
	{
		#region Fields

		#endregion
		#region Properties
		/// <summary>
		/// False.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		/// <summary>
		/// Pointer to CMesh object in unmanaged memory.
		/// </summary>
		public IntPtr MeshHandle { get; protected set; }
		/// <summary>
		/// Pointer to collection of mesh details in unmanaged memory.
		/// </summary>
		public IntPtr CollectionHandle { get; protected set; }
		/// <summary>
		/// Number of objects in the collection.
		/// </summary>
		public int Count { get; protected set; }
		/// <summary>
		/// Gets number of objects in the collection or reallocates it to fit new number of objects.
		/// </summary>
		public virtual int Capacity
		{
			get { return this.Count; }
			set
			{
				if (value >= 0 && value != this.Count && this.Reallocatable)
				{
					NativeMeshMethods.ReallocateStream(this.MeshHandle, this.MemoryRegionIdentifier, value);
				}
			}
		}
		#endregion
		#region Events

		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Updates collection handle and number of elements in it.
		/// </summary>
		public virtual void UpdateCollection()
		{
			this.CollectionHandle =
				NativeMeshMethods.GetStreamHandle(this.MeshHandle, this.MemoryRegionIdentifier);
			this.Count =
				NativeMeshMethods.GetNumberOfElements(this.MeshHandle, this.MemoryRegionIdentifier);
		}
		/// <summary>
		/// Creates enumerator for this collection.
		/// </summary>
		/// <returns>Object that enumerates through elements of this collection.</returns>
		public virtual IEnumerator<ElementType> GetEnumerator()
		{
			return new ListEnumerator<ElementType>(this);
		}
		/// <summary>
		/// Zeros elements of this collection.
		/// </summary>
		public virtual void Clear()
		{
			NativeMeshMethods.ReallocateStream(this.MeshHandle, this.MemoryRegionIdentifier, 0);
			NativeMeshMethods.ReallocateStream(this.MeshHandle, this.MemoryRegionIdentifier, this.Count);
			this.UpdateCollection();
		}
		/// <summary>
		/// Linearly searches through elements of this collection.
		/// </summary>
		/// <param name="item">Element to look for.</param>
		/// <returns>
		/// True, if at least element equal to given is located within this collection.
		/// </returns>
		public virtual bool Contains(ElementType item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Equals(item))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Linearly searches through elements of this collection.
		/// </summary>
		/// <param name="item">Element to look for.</param>
		/// <returns>
		/// Zero-based index of the first element within this collection that equals given, -1 if no
		/// such element was found.
		/// </returns>
		public virtual int IndexOf(ElementType item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Copies all elements of this collection to given array.
		/// </summary>
		/// <param name="array">Array to copy elements to.</param>
		/// <param name="arrayIndex">
		/// Zero-based index of the first element in the array where to start copying.
		/// </param>
		public virtual void CopyTo(ElementType[] array, int arrayIndex)
		{
			if (this.Count == 0)
			{
				return;
			}
			if (arrayIndex < 0)
			{
				arrayIndex = 0;
			}
			if (array.Length == 0)
			{
				array = new ElementType[this.Count + arrayIndex];
			}
			for (int i = 0, j = arrayIndex; i < this.Count && j < array.Length; i++)
			{
				array[j] = this[i];
			}
		}
		/// <summary>
		/// Copies all elements of this collection to an array.
		/// </summary>
		/// <returns>
		/// Array that contains a copy of all elements of this collection. Null is returned if
		/// collection is empty.
		/// </returns>
		public virtual ElementType[] ToArray()
		{
			if (this.Count == 0)
			{
				return null;
			}
			ElementType[] array = new ElementType[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = this[i];
			}
			return array;
		}
		/// <summary>
		/// Copies data from given list to this collection.
		/// </summary>
		/// <param name="details">
		/// Object that implements <see cref="IList{ElementType}" /> that provides the data.
		/// </param>
		public virtual void CopyFrom(IList<ElementType> details)
		{
#if DEBUG
			this.CopyFrom(details, 0, 0, Math.Max(this.Count, details.Count), false);
#else
			this.CopyFrom(details, 0, 0, Math.Max(this.Count, details.Count), true);
#endif
		}
		/// <summary>
		/// Copies data from given list to this collection.
		/// </summary>
		/// <param name="details">
		/// Object that implements <see cref="IList{ElementType}" /> that provides the data.
		/// </param>
		/// <param name="sourceStart">
		/// Zero-based index of the first element from <paramref name="details" /> to copy elements
		/// from (inclusively).
		/// </param>
		/// <param name="destStart">
		/// Zero-based index of the first element from this collection to copy elements to (inclusively).
		/// </param>
		/// <param name="count">Number of elements to copy.</param>
		/// <param name="ignoreOverflow">
		/// Indicates whether exception must thrown when this collection cannot fit elements. If
		/// true, all extra data will be ignored, otherwise exception is thrown.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the first source element is out of bounds.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the first destination element is out of bounds.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Given list does not have enough data. This exception can be thrown only if <paramref
		/// name="ignoreOverflow" /> is set to false.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// This collection cannot fit given data. This exception can be thrown only if <paramref
		/// name="ignoreOverflow" /> is set to false.
		/// </exception>
		public virtual void CopyFrom(IList<ElementType> details, int sourceStart, int destStart, int count,
									 bool ignoreOverflow)
		{
			if (details == null || details.Count == 0)
			{
#if DEBUG
				throw new ArgumentNullException("details", "Cannot copy data from empty list.");
#else
				return;
#endif
			}
			if (sourceStart >= details.Count)
			{
				throw new ArgumentOutOfRangeException("sourceStart", "Index of the first source element is out of bounds.");
			}
			if (destStart >= this.Count)
			{
				throw new ArgumentOutOfRangeException("destStart", "Index of the first destination element is out of bounds.");
			}
			if (sourceStart + count >= details.Count && !ignoreOverflow)
			{
#if DEBUG
				throw new ArgumentException("Given list does not have enough data.");
#endif
			}
			if (destStart + count >= this.Count && !ignoreOverflow)
			{
				throw new ArgumentException("This collection cannot fit given data.");
			}
			if (!(this.Count >= count || ignoreOverflow || this.Reallocatable))
			{
				throw new MeshConsistencyException("Cannot resize this collection to fit the data.");
			}
			Contract.EndContractBlock();
			// Resize collection.
			this.Capacity = details.Count;
			// Copy data.
			for
			(
				int i = destStart, j = sourceStart;
				i < this.Count && j < details.Count && i - destStart < count && j - sourceStart < count;
				i++
			)
			{
				this[i] = details[j];
			}
		}
		#endregion
		#region Utilities
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#region Unsupported operations.
		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <exception cref="NotSupportedException"></exception>
		public void Insert(int index, ElementType item)
		{
			throw new NotSupportedException();
		}
		/// <summary></summary>
		/// <param name="index"></param>
		/// <exception cref="NotSupportedException"></exception>
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}
		/// <summary></summary>
		/// <param name="item"></param>
		/// <exception cref="NotSupportedException"></exception>
		public void Add(ElementType item)
		{
			throw new NotSupportedException();
		}
		/// <summary></summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException"></exception>
		public bool Remove(ElementType item)
		{
			throw new NotSupportedException();
		}
		#endregion
		#endregion
		#region To be Implemented in Derived Classes
		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public abstract ElementType this[int index] { get; set; }
		/// <summary>
		/// When implemented in derived class, gets identifier of native memory region that contains
		/// this collection.
		/// </summary>
		public abstract NativeMeshMemoryRegion MemoryRegionIdentifier { get; }
		/// <summary>
		/// When implemented in derived class, indicates whether collection is allowed to be
		/// reallocated on its own.
		/// </summary>
		public abstract bool Reallocatable { get; }
		#endregion
	}
}