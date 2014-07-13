using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines common functionality of objects that assist in
	/// transferring strongly-typed objects to and from native memory.
	/// </summary>
	/// <typeparam name="ObjectType">
	/// Type of objects to handle.
	/// </typeparam>
	public interface ITransferAgent<ObjectType>
	{
		/// <summary>
		/// When implemented in derived class, calculates number of
		/// bytes that need to be allocated in native memory to fit
		/// given collection of objects.
		/// </summary>
		/// <param name="objects">
		/// A collection of objects that need to be transfered to
		/// native memory.
		/// </param>
		/// <returns>
		/// A number of bytes to allocate in native memory to fit a
		/// collection of objects.
		/// </returns>
		ulong GetBytesNumber(IList<ObjectType> objects);
		/// <summary>
		/// When implemented in derived class, calculates number of
		/// bytes that need to be allocated in native memory to fit
		/// given number of objects.
		/// </summary>
		/// <remarks>
		/// This method is primarily used when dealing with objects
		/// that have wither fixed size or their size can be
		/// calculated without knowing their data.
		/// </remarks>
		/// <param name="objectsCount">
		/// Number of objects to fit into native memory.
		/// </param>
		/// <returns>
		/// Size of smallest native memory cluster that can fit given
		/// number of objects.
		/// </returns>
		ulong GetBytesNumber(ulong objectsCount);
		/// <summary>
		/// When implemented in derived class, determines number of
		/// objects of type <typeparamref name="ObjectType" /> stored
		/// in native memory.
		/// </summary>
		/// <param name="handle">
		/// Address of native memory cluster.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of first byte in a sequence that contains objects.
		/// </param>
		/// <param name="size">
		/// Number of bytes in a sequence within native memory cluster.
		/// </param>
		/// <returns>Number of objects in given native array.</returns>
		ulong GetObjectsNumber(IntPtr handle, ulong offset, ulong size);
		/// <summary>
		/// When implemented in derived class, writes a collection of
		/// objects into the stream.
		/// </summary>
		/// <param name="stream">
		/// <see cref="NativeMemoryStream" /> object that provides
		/// stream access to native memory.
		/// </param>
		/// <param name="objects">
		/// A collection of objects to write to native memory.
		/// </param>
		/// <returns>Number of bytes written.</returns>
		ulong Write(NativeMemoryStream stream, IList<ObjectType> objects);
		/// <summary>
		/// When implemented in derived class, reads a number of
		/// objects from native memory.
		/// </summary>
		/// <param name="stream">
		/// <see cref="NativeMemoryStream" /> object that provides
		/// stream access to native memory.
		/// </param>
		/// <param name="objects">
		/// A collection of objects into which to write the objects
		/// from native memory.
		/// </param>
		/// <param name="index">
		/// Zero-based index of the first object to write objects to.
		/// - 1 to write objects to the and of the collection.
		/// </param>
		/// <param name="count">
		/// Number of objects to write. 0 to write all available.
		/// </param>
		/// <returns>Number of read objects.</returns>
		int Read(NativeMemoryStream stream, IList<ObjectType> objects, int index = -1, int count = 0);
	}
}