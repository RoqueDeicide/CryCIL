using System;
using System.Linq;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents a collection of children of particle effect in hierarchical structure.
	/// </summary>
	public struct ParticleEffectChildren
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets number of children in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return ParticleEffect.GetChildCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the child particle effect.
		/// </summary>
		/// <param name="index">Zero-based index of the child to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public ParticleEffect this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return ParticleEffect.GetChild(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal ParticleEffectChildren(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes all children from this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.AssertInstance();

			ParticleEffect.ClearChilds(this.handle);
		}
		/// <summary>
		/// Adds a child particle effect to the end of this collection.
		/// </summary>
		/// <param name="child">A particle effect to add.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Child particle effect cannot be null.</exception>
		public void Add(ParticleEffect child)
		{
			this.AssertInstance();
			if (!child.IsValid)
			{
				throw new ArgumentNullException(nameof(child), "Child particle effect cannot be null.");
			}

			ParticleEffect.InsertChild(this.handle, this.Count, child);
		}
		/// <summary>
		/// Inserts a particle effect into this collection.
		/// </summary>
		/// <param name="child">A particle effect to insert.</param>
		/// <param name="slot"> Zero-based index of the slot to insert the child at.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Child particle effect cannot be null.</exception>
		public void Insert(ParticleEffect child, int slot)
		{
			this.AssertInstance();
			if (!child.IsValid)
			{
				throw new ArgumentNullException(nameof(child), "Child particle effect cannot be null.");
			}

			ParticleEffect.InsertChild(this.handle, slot, child);
		}
		/// <summary>
		/// Attempts to locate the child particle effect in this collection.
		/// </summary>
		/// <param name="child">Particle effect to find.</param>
		/// <returns>
		/// Positive zero-based index of the slot that is occupied by the child, if it was found, otherwise
		/// a negative value is returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(ParticleEffect child)
		{
			this.AssertInstance();
			if (!child.IsValid)
			{
				return -1;
			}

			return ParticleEffect.FindChild(this.handle, child);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		#endregion
	}
}