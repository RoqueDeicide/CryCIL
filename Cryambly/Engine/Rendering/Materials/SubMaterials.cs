using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Provides access to sub-materials held by the material object.
	/// </summary>
	public struct SubMaterials : IList<Material>
	{
		#region Fields
		[UsedImplicitly] private IntPtr matHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Provides read/write access to a sub-material slot.
		/// </summary>
		/// <param name="index">Zero-based index of the slot to access.</param>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the sub-material to access cannot be less then 0.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the sub-material to access cannot be greater or equal to number of sub-material slots.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot assign null material to a sub-material slot, try assigning default material instead.
		/// </exception>
		public Material this[int index]
		{
			get { return this.GetItem(index); }
			set { this.SetItem(index, value); }
		}
		/// <summary>
		/// Gets or sets number of slots for sub-materials.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// Unable to access number of sub-material slots of an invalid material object.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of sub-material slots cannot be less then 0.
		/// </exception>
		public int Count
		{
			get { return this.GetCount(); }
			set { this.SetCount(value); }
		}
		/// <summary>
		/// Returns false.
		/// </summary>
		public bool IsReadOnly => false;
		#endregion
		#region Construction
		internal SubMaterials(IntPtr handle)
		{
			this.matHandle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a sub-material to the end of this collection.
		/// </summary>
		/// <param name="item">Material to add.</param>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot add null material to a sub-material slot, try using default material instead.
		/// </exception>
		public void Add(Material item)
		{
			try
			{
				int index = this.Count;
				this.Count = index + 1;
				this[index] = item;
			}
			catch (ArgumentNullException ex)
			{
				throw new ArgumentNullException("Cannot add null material to a sub-material slot, try using " +
												"default material instead.", ex);
			}
		}
		/// <summary>
		/// Clears this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		public void Clear()
		{
			this.Count = 0;
		}
		/// <summary>
		/// Inserts a sub-material at specified position.
		/// </summary>
		/// <param name="index">Zero-based index of the position of insertion.</param>
		/// <param name="item"> Material to insert.</param>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot insert invalid material into the collection.
		/// </exception>
		/// <exception cref="Exception">Given list is not big enough.</exception>
		public void Insert(int index, Material item)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Index cannot be less then 0.");
			}
			if (!item.IsValid)
			{
				throw new ArgumentNullException(nameof(item), "Cannot insert invalid material into the collection.");
			}

			int initialCount = this.Count;

			if (index == initialCount)
			{
				this.Add(item);
				return;
			}

			if (index > initialCount)
			{
				this.Count = index + 1;
			}
			else
			{
				this.Count = this.Count + 1;

				this.Shift(index, index + 1, initialCount - index - 1);
			}

			this[index] = item;
		}
		/// <summary>
		/// Removes a sub-material at specified index.
		/// </summary>
		/// <param name="index">Zero-based index of the sub-material to remove.</param>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="Exception">Given list is not big enough.</exception>
		public void RemoveAt(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Index cannot be less then 0.");
			}

			int count = this.Count;
			if (index != count - 1)
			{
				this.Shift(index + 1, index, count - index - 1);
			}

			this.Count = count - 1;
		}
		/// <summary>
		/// Copies sub-materials from this collection to the array.
		/// </summary>
		/// <param name="array">     An array to copy sub-materials to.</param>
		/// <param name="arrayIndex">
		/// Zero-based index of the first slot in the array to start copying to.
		/// </param>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		/// <exception cref="ArgumentNullException">Cannot copy sub-material to the null array.</exception>
		/// <exception cref="IndexOutOfRangeException">Array index cannot be less then 0.</exception>
		/// <exception cref="ArgumentException">
		/// Cannot fit the sub-materials into the given array.
		/// </exception>
		public void CopyTo(Material[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array), "Cannot copy sub-material to the null array.");
			}
			if (arrayIndex < 0)
			{
				throw new IndexOutOfRangeException("Array index cannot be less then 0.");
			}
			if (arrayIndex > array.Length - this.Count)
			{
				throw new ArgumentException("Cannot fit the sub-materials into the given array.");
			}

			int count = this.Count;
			for (int i = 0, j = arrayIndex; i < count; i++, j++)
			{
				array[j] = this[i];
			}
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public bool Contains(Material item)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public int IndexOf(Material item)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing.</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public bool Remove(Material item)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Object that does the enumeration.</returns>
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		public IEnumerator<Material> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">
		/// Unable to access the sub-material of an invalid material object.
		/// </exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetItem(int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetItem(int index, Material mat);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetCount();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCount(int newCount);
		#endregion
	}
}