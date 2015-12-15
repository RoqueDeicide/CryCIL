using System;
using System.Collections;
using System.Collections.Generic;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a collection of positions of vertices in 3D space.
	/// </summary>
	public unsafe struct VertexPositionCollection : IEnumerable<Vector3>
	{
		#region Fields
		private readonly Vector3* ptr;
		private readonly int count;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.ptr != null; }
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
		public Vector3 this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				return this.ptr[index];
			}
			set
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				this.ptr[index] = value;
			}
		}
		#endregion
		#region Construction
		internal VertexPositionCollection(Vector3* ptr, int count)
		{
			this.ptr = ptr;
			this.count = count;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (this.ptr == null)
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
		public IEnumerator<Vector3> GetEnumerator()
		{
			this.AssertInstance();

			for (int i = 0; i < this.count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
	/// <summary>
	/// Represents a collection of normals of vertices.
	/// </summary>
	public unsafe struct VertexNormalCollection : IEnumerable<CryMeshNormal>
	{
		#region Fields
		private readonly CryMeshNormal* ptr;
		private readonly int count;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.ptr != null; }
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
		public CryMeshNormal this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				return this.ptr[index];
			}
			set
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				this.ptr[index] = value;
			}
		}
		#endregion
		#region Construction
		internal VertexNormalCollection(CryMeshNormal* ptr, int count)
		{
			this.ptr = ptr;
			this.count = count;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (this.ptr == null)
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
		public IEnumerator<CryMeshNormal> GetEnumerator()
		{
			this.AssertInstance();

			for (int i = 0; i < this.count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
	/// <summary>
	/// Represents a collection of colors of vertices.
	/// </summary>
	public unsafe struct VertexColorCollection : IEnumerable<CryMeshColor>
	{
		#region Fields
		private readonly CryMeshColor* ptr;
		private readonly int count;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.ptr != null; }
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
		public CryMeshColor this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				return this.ptr[index];
			}
			set
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				this.ptr[index] = value;
			}
		}
		#endregion
		#region Construction
		internal VertexColorCollection(CryMeshColor* ptr, int count)
		{
			this.ptr = ptr;
			this.count = count;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (this.ptr == null)
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
		public IEnumerator<CryMeshColor> GetEnumerator()
		{
			this.AssertInstance();

			for (int i = 0; i < this.count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
	/// <summary>
	/// Represents a collection of indexes of materials of vertices.
	/// </summary>
	public unsafe struct VertexMaterialCollection : IEnumerable<int>
	{
		#region Fields
		private readonly int* ptr;
		private readonly int count;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.ptr != null; }
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
		public int this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				return this.ptr[index];
			}
			set
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
				if (index >= this.count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater or equal to the size of this collection.");
				}

				this.ptr[index] = value;
			}
		}
		#endregion
		#region Construction
		internal VertexMaterialCollection(int* ptr, int count)
		{
			this.ptr = ptr;
			this.count = count;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (this.ptr == null)
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
		public IEnumerator<int> GetEnumerator()
		{
			this.AssertInstance();

			for (int i = 0; i < this.count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
}