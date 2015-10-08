using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CryCil.Engine.Physics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a collection of static sub-objects of a compound static object.
	/// </summary>
	public struct StaticSubObjects : IList<StaticSubObject>
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets the static sub-object.
		/// </summary>
		/// <param name="index">Zero-based index of the sub-object to get.</param>
		/// <returns>An object that wraps the sub-object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="NotSupportedException">
		/// Setting static sub-objects is not supported.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">Index cannot be less then 0.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be greater then or equal to number of sub-objects. Not thrown in non-debug builds.
		/// </exception>
		public StaticSubObject this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0.");
				}
#if DEBUG
				if (index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index cannot be greater then or equal to number of sub-objects.");
				}
#endif
				Contract.EndContractBlock();

				return StaticObject.GetSubObject(this.handle, index);
			}
			set { throw new NotSupportedException(); }
		}
		/// <summary>
		/// Gets or sets the number of slots for sub-objects.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return StaticObject.GetSubObjectCount(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				StaticObject.SetSubObjectCount(this.handle, value);
			}
		}
		/// <summary>
		/// Returns <c>false</c>.
		/// </summary>
		bool ICollection<StaticSubObject>.IsReadOnly
		{
			get { return false; }
		}
		#endregion
		#region Construction
		internal StaticSubObjects(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>An object that handles enumeration.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public IEnumerator<StaticSubObject> GetEnumerator()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				yield return this[i];
			}
		}
		/// <summary>
		/// Removes all slots for sub-objects.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.Count = 0;
		}
		/// <summary>
		/// Adds a sub-object to this collection.
		/// </summary>
		/// <param name="item">A sub-object to add.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Add(StaticObject item)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			StaticObject.AddSubObject(this.handle, item);
		}
		/// <summary>
		/// Removes a sub-object for specified slot.
		/// </summary>
		/// <param name="index">Zero-based index of the slot to clear.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveAt(int index)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			StaticObject.RemoveSubObject(this.handle, index);
		}
		/// <summary>
		/// Looks for a static sub-object with specified name.
		/// </summary>
		/// <param name="name">Name of the sub-object to find.</param>
		/// <returns>Usable wrapper of sub-object, if found. Invalid one otherwise.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticSubObject Find(string name)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StaticObject.FindSubObject(this.handle, name);
		}
		/// <summary>
		/// Looks for a static sub-object that was loaded from CGA file (?) with specified name.
		/// </summary>
		/// <param name="name">Name of the sub-object to find.</param>
		/// <returns>Usable wrapper of sub-object, if found. Invalid one otherwise.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticSubObject FindCga(string name)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StaticObject.FindSubObject_CGA(this.handle, name);
		}
		/// <summary>
		/// Looks for a static sub-object with specified name.
		/// </summary>
		/// <param name="name">Full name of the sub-object to find.</param>
		/// <returns>Usable wrapper of sub-object, if found. Invalid one otherwise.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticSubObject FindFullName(string name)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StaticObject.FindSubObject_StrStr(this.handle, name);
		}
		/// <summary>
		/// Copies a sub-object into this collection.
		/// </summary>
		/// <param name="source">          A static object to copy the sub-object from.</param>
		/// <param name="sourceIndex">     Zero-based index of the sub-object slot to copy from.</param>
		/// <param name="destinationIndex">Zero-based index of the sub-object slot to copy to.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// Static object to copy a sub-object from cannot be null.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Index of the sub-object to copy cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Index of the sub-object to copy cannot be greater then or equal to number of sub-objects.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Index of the sub-object slot to copy to cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// Index of the sub-object slot to copy to cannot be greater then or equal to number of
		/// sub-objects.
		/// </exception>
		public bool Copy(StaticObject source, int sourceIndex, int destinationIndex)
		{
			this.AssertInstance();
			if (!source.IsValid)
			{
				throw new ArgumentNullException("source", "Static object to copy a sub-object from cannot be null.");
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex",
													  "Index of the sub-object to copy cannot be less then 0.");
			}
			if (sourceIndex >= source.SubObjects.Count)
			{
				throw new ArgumentOutOfRangeException
					("sourceIndex",
					 "Index of the sub-object to copy cannot be greater then or equal to number of sub-objects.");
			}
			if (destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex",
													  "Index of the sub-object slot to copy to cannot be less then 0.");
			}
			if (destinationIndex >= this.Count)
			{
				throw new ArgumentOutOfRangeException
					("destinationIndex",
					 "Index of the sub-object slot to copy to cannot be greater then or equal to number of sub-objects.");
			}
			Contract.EndContractBlock();

			return StaticObject.CopySubObject(this.handle, destinationIndex, source, sourceIndex);
		}
		/// <summary>
		/// Adds all sub-objects in this collection to the physical entity as parts.
		/// </summary>
		/// <param name="entity">         Physical entity to add sub-objects as parts to.</param>
		/// <param name="matrix">         
		/// Reference to transformation matrix that represents the scale of the hosting entity and local
		/// transformation of the static object.
		/// </param>
		/// <param name="mass">           Total mass of all sub-objects(?).</param>
		/// <param name="density">        
		/// Optional value that can specify density of each sub-object, if mass is 0.
		/// </param>
		/// <param name="id0">            
		/// When adding sub-objects as parts start the part identifiers from this value.
		/// </param>
		/// <param name="szPropsOverride">Unknown.</param>
		/// <returns>Identifier of last part that was created from sub-objects(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Physicalize(PhysicalEntity entity, ref Matrix34 matrix, float mass, float density = 0.0f, int id0 = 0,
							   string szPropsOverride = null)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StaticObject.PhysicalizeSubobjects(this.handle, entity, ref matrix, mass, density, id0,
													  szPropsOverride);
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		void ICollection<StaticSubObject>.Add(StaticSubObject item)
		{
			throw new NotSupportedException();
		}
		bool ICollection<StaticSubObject>.Contains(StaticSubObject item)
		{
			throw new NotSupportedException();
		}
		void ICollection<StaticSubObject>.CopyTo(StaticSubObject[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}
		bool ICollection<StaticSubObject>.Remove(StaticSubObject item)
		{
			throw new NotSupportedException();
		}
		int IList<StaticSubObject>.IndexOf(StaticSubObject item)
		{
			throw new NotSupportedException();
		}
		void IList<StaticSubObject>.Insert(int index, StaticSubObject item)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}