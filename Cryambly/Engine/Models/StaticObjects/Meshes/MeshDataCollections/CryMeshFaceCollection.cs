using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a data stream that contains the faces of triangular mesh.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct CryMeshFaceCollection : IEnumerable<CryMeshFace>
	{
		#region Fields
		private const GeneralMeshStreamId MainStreamId = GeneralMeshStreamId.Faces;

		[FieldOffset(0)] private readonly GeneralCryMesh mesh;
		[FieldOffset(0)] private readonly CMeshInternals* meshHandle;
		[FieldOffset(VariousConstants.PointerSize)] private CryMeshFace* facesPtr;
		#endregion
		#region Properties
		private CryMeshFace* DataPointer
		{
			get
			{
				int c;
				return (CryMeshFace*)GeneralCryMesh.GetStreamPtr(this.meshHandle, MainStreamId, out c);
			}
		}
		/// <summary>
		/// Gets or sets the number of elements in this collection. Setting this to negative value or 0
		/// deallocates the array.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return this.meshHandle->streamSize[(int)MainStreamId];
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				GeneralCryMesh.ReallocateStream(this.meshHandle, MainStreamId, value);
				this.facesPtr = this.DataPointer;
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
		public CryMeshFace this[int index]
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
				Contract.EndContractBlock();

				return this.facesPtr[index];
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
				Contract.EndContractBlock();

				this.facesPtr[index] = value;
			}
		}
		#endregion
		#region Construction
		internal CryMeshFaceCollection(GeneralCryMesh mesh, CMeshInternals* meshHandle)
			: this()
		{
			this.mesh = mesh;
			this.meshHandle = meshHandle;
			if (this.mesh.IsValid)
			{
				this.facesPtr = this.DataPointer;
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes a range of elements from this collection.
		/// </summary>
		/// <remarks>The range is clamped when it exceeds the bounds of this collection.</remarks>
		/// <param name="first">Zero-based index of the first element to remove.</param>
		/// <param name="count">Number of elements to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveRange(int first, int count)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			first = first < 0 ? 0 : first;
			GeneralCryMesh.RemoveRangeFromStreamInternal(this.meshHandle, MainStreamId, first, count);
		}
		/// <summary>
		/// Removes an element from this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the element to remove.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAt(int index)
		{
			this.RemoveRange(index, 1);
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.mesh.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>An object that handles enumeration.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public IEnumerator<CryMeshFace> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
		}
		#endregion
	}
}