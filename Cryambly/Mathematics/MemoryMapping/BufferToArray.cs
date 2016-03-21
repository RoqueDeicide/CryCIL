using System;
using System.Linq;

namespace CryCil.MemoryMapping
{
	/// <summary>
	/// Defines methods that convert fixed size buffers to arrays.
	/// </summary>
	public static unsafe class BufferToArray
	{
		/// <summary>
		/// Copies data from fixed buffer to a managed array.
		/// </summary>
		/// <param name="pointer">Pointer to first byte.</param>
		/// <param name="count">  
		/// Number of bytes to copy, better not go beyond any bounds, as this method has no means of
		/// checking them.
		/// </param>
		/// <returns>
		/// A managed array of bytes copied from the buffer. Null is returned if <paramref name="pointer"/>
		/// is null or <paramref name="count"/> is not positive.
		/// </returns>
		public static byte[] ToBytes(byte* pointer, int count)
		{
			if (pointer == null || count <= 0)
			{
				return null;
			}
			byte[] bytes = new byte[count];
			for (int i = 0; i < count; i++)
			{
				bytes[i] = pointer[i];
			}
			return bytes;
		}
	}
}