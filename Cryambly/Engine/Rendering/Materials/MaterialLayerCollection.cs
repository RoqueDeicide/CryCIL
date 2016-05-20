using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents a collection of material layers.
	/// </summary>
	/// <remarks>This collection does not support search-related functions.</remarks>
	public struct MaterialLayerCollection : IList<MaterialLayer>
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of material layers in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of objects in the collection cannot be less then 0.
		/// </exception>
		public int Count
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return (int)GetLayerCount(this.handle);
			}
			set
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Number of objects in the collection cannot be less then 0.");
				}
				SetLayerCount(this.handle, (uint)value);
			}
		}
		/// <summary>
		/// Returns false.
		/// </summary>
		/// <exception cref="NullReferenceException" accessor="get">
		/// Instance object is not valid.
		/// </exception>
		public bool IsReadOnly
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return false;
			}
		}
		/// <summary>
		/// Gets or sets the material layer in the collection.
		/// </summary>
		/// <param name="index">Zero-based index of the layer.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Material layer object is not initialized.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then or equal to number of layers in the collection.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">Index cannot be less then zero.</exception>
		public MaterialLayer this[int index]
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be less then zero.");
				}
				bool error;
				var result = GetLayerChecked(this.handle, (uint)index, out error);
				if (error)
				{
					throw new IndexOutOfRangeException("Index cannot be greater then or equal to number of layers in the collection.");
				}
				return result;
			}
			set
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				if (!value.IsValid)
				{
					throw new ArgumentNullException(nameof(value), "Material layer object is not initialized.");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be less then zero.");
				}
				bool error;
				SetLayerChecked(this.handle, (uint)index, value, out error);
				if (error)
				{
					throw new IndexOutOfRangeException("Index cannot be greater then or equal to number of layers in the collection.");
				}
			}
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		#endregion
		#region Construction
		internal MaterialLayerCollection(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>An object that does the enumeration.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public IEnumerator<MaterialLayer> GetEnumerator()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			uint count = GetLayerCount(this.handle);
			for (uint i = 0; i < count; i++)
			{
				yield return GetLayer(this.handle, i);
			}
		}
		/// <summary>
		/// Adds a layer to the collection.
		/// </summary>
		/// <param name="item">A layer to add.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Material layer object is not initialized.</exception>
		public void Add(MaterialLayer item)
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			if (!item.IsValid)
			{
				throw new ArgumentNullException(nameof(item), "Material layer object is not initialized.");
			}
			uint oldCount = GetLayerCount(this.handle);
			SetLayerCount(this.handle, oldCount + 1);
			SetLayer(this.handle, oldCount, item);
		}
		/// <summary>
		/// Clears the entire collection.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void Clear()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			SetLayerCount(this.handle, 0);
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public bool Contains(MaterialLayer item)
		{
			throw new NotSupportedException("This operation is not supported.");
		}
		/// <summary>
		/// Copies the material layers to the array.
		/// </summary>
		/// <param name="array">     An array of material layers.</param>
		/// <param name="arrayIndex">
		/// Zero-based index of the first position in the array to start copying to.
		/// </param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Given array cannot be of 0 length.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="BufferOverflowException">Given array is too small.</exception>
		public void CopyTo(MaterialLayer[] array, int arrayIndex)
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			if (array.Length == 0)
			{
				throw new ArgumentNullException(nameof(array), "Given array cannot be of 0 length.");
			}
			if (arrayIndex < 0)
			{
				throw new IndexOutOfRangeException("Index cannot be less then 0.");
			}
			uint count = GetLayerCount(this.handle);
			if (arrayIndex + count > array.LongLength)
			{
				throw new BufferOverflowException("Given array is too small.");
			}

			for (int i = 0; i < array.Length; i++)
			{
				array[arrayIndex + i] = GetLayer(this.handle, (uint)i);
			}
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public bool Remove(MaterialLayer item)
		{
			throw new NotSupportedException("This operation is not supported.");
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="item">Ignored.</param>
		/// <returns>Nothing</returns>
		/// <exception cref="NotSupportedException">This operation is not supported.</exception>
		public int IndexOf(MaterialLayer item)
		{
			throw new NotSupportedException("This operation is not supported.");
		}
		/// <summary>
		/// Inserts the material layer at the specified slot.
		/// </summary>
		/// <param name="index">Zero-based index of the slot to put the layer in.</param>
		/// <param name="item"> Material layer to insert.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Material layer object is not initialized.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then number of layers in the collection.
		/// </exception>
		/// <exception cref="Exception">Given list is not big enough.</exception>
		public void Insert(int index, MaterialLayer item)
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			if (!item.IsValid)
			{
				throw new ArgumentNullException(nameof(item), "Material layer object is not initialized.");
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Index cannot be less then 0.");
			}
			uint count = GetLayerCount(this.handle);
			if (index > count)
			{
				throw new IndexOutOfRangeException("Index cannot be greater then number of layers in the collection.");
			}

			if (index == count)
			{
				this.Add(item);
			}
			else
			{
				SetLayerCount(this.handle, count + 1);
				this.Shift(index, index + 1, (int)(count - index));
				SetLayer(this.handle, (uint)index, item);
			}
		}
		/// <summary>
		/// Removes the material layer from the slot.
		/// </summary>
		/// <param name="index">Zero-based index of material layer slot.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then or equal to number of layers in the collection.
		/// </exception>
		/// <exception cref="Exception">Given list is not big enough.</exception>
		public void RemoveAt(int index)
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException("Index cannot be less then 0.");
			}
			uint count = GetLayerCount(this.handle);
			if (index >= count)
			{
				throw new IndexOutOfRangeException("Index cannot be greater then or equal to number of layers in the collection.");
			}

			this.Shift(index + 1, index, (int)(count - index - 1));
			SetLayerCount(this.handle, count - 1);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLayerCount(IntPtr handle, uint nCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetLayerCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLayer(IntPtr handle, uint nSlot, MaterialLayer pLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLayerChecked(IntPtr handle, uint nSlot, MaterialLayer pLayer, out bool error);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MaterialLayer GetLayer(IntPtr handle, uint nSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MaterialLayer GetLayerChecked(IntPtr handle, uint nSlot, out bool error);
		#endregion
	}
}