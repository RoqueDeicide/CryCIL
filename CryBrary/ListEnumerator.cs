using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Represents an object that enumerates collections that implement IList interface.
	/// </summary>
	/// <typeparam name="ElementType">Type of elements in enumerated collection.</typeparam>
	public class ListEnumerator<ElementType> : IEnumerator<ElementType>
	{
		#region Fields
		private readonly IList<ElementType> list;
		private int currentPosition;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current element of the enumerator.
		/// </summary>
		public ElementType Current
		{
			get
			{
#if DEBUG
				if (this.currentPosition < 0 || this.currentPosition == this.list.Count)
				{
					throw new Exception("Enumerator is out of bounds of the list.");
				}
#endif
				return this.list[this.currentPosition];
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new enumerator for given collection.
		/// </summary>
		/// <param name="list">
		/// Collection that implements <see cref="IList{ElementType}"/> interface.
		/// </param>
		public ListEnumerator(IList<ElementType> list)
		{
			this.list = list;
			this.Reset();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Moves the enumerator to the next element.
		/// </summary>
		/// <returns>False, if the end of the collection has been reached, otherwise true.</returns>
		public bool MoveNext()
		{
			return ++this.currentPosition == this.list.Count;
		}
		/// <summary>
		/// Sets enumerator to position before first element of the list.
		/// </summary>
		public void Reset()
		{
			this.currentPosition = -1;
		}
		/// <summary>
		/// </summary>
		public void Dispose()
		{
		}
		#endregion
		#region Utilities

		object System.Collections.IEnumerator.Current
		{
			get { return this.Current; }
		}
		#endregion
	}
}