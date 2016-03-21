using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a collection of mesh subsets.
	/// </summary>
	public unsafe struct CryMeshSubsetCollection : IEnumerable<CryMeshSubset>
	{
		#region Fields
		private readonly CMeshInternals* meshHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the number of elements in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return CryMesh.GetSubsetCount(this.meshHandle);
			}
			set
			{
				this.AssertInstance();

				CryMesh.SetSubsetCount(this.meshHandle, value);
			}
		}
		/// <summary>
		/// Gets or sets the element within this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the element to get or set.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater or equal to the size of this collection.
		/// </exception>
		public CryMeshSubset this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				return this.meshHandle->subsets[index];
			}
			set
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				this.meshHandle->subsets[index] = value;
			}
		}
		#endregion
		#region Construction
		internal CryMeshSubsetCollection(CMeshInternals* meshHandle)
		{
			this.meshHandle = meshHandle;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (this.meshHandle == null)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>An object that handles enumeration.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public IEnumerator<CryMeshSubset> GetEnumerator()
		{
			this.AssertInstance();
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
}