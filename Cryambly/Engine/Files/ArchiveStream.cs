using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using CryCil.Engine.Memory;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Represents a data stream to provides access to the archive file.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe class ArchiveStream : Stream
	{
		#region Fields
		private readonly IntPtr archive;
		private readonly bool canWrite;
		private uint length;
		private long position;
		private readonly IntPtr name;
		private byte* bytes;
		private bool alive;
		#endregion
		#region Properties
		/// <summary>
		/// Returns true if this stream is not closed.
		/// </summary>
		public override bool CanRead
		{
			get { return this.alive; }
		}
		/// <summary>
		/// Returns true if this stream is not closed.
		/// </summary>
		public override bool CanSeek
		{
			get { return alive; }
		}
		/// <summary>
		/// Indicates whether this is possible to write the file.
		/// </summary>
		public override bool CanWrite
		{
			get { return this.canWrite && this.alive; }
		}
		/// <summary>
		/// Gets the length of the file.
		/// </summary>
		public override long Length
		{
			get { return this.length; }
		}
		/// <summary>
		/// Gets or sets position of this stream in the data sequence.
		/// </summary>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentException">Cannot set position of the stream beyond 4GB.</exception>
		/// <exception cref="ArgumentException">Cannot extend the file beyond 4GB when seeking.</exception>
		public override long Position
		{
			get
			{
				if (!this.alive)
				{
					throw new ObjectDisposedException("This stream is closed.");
				}
				return this.position;
			}
			set
			{
				if (!this.alive)
				{
					throw new ObjectDisposedException("This stream is closed.");
				}
				if (value > UInt32.MaxValue)
				{
					throw new ArgumentException("Cannot set position of the stream beyond 4GB.");
				}
				if (value > this.length - 1)
				{
					try
					{
						this.SetLength(value + 1);
					}
					catch (ArgumentOutOfRangeException ex)
					{
						throw new ArgumentException("Cannot extend the file beyond 4GB when seeking.", ex);
					}
				}

				this.position = value;
			}
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		internal ArchiveStream(IntPtr archive, IntPtr fileName, bool canWrite, uint size, byte* bytes)
		{
			this.archive = archive;
			this.name = fileName;
			this.canWrite = canWrite;
			this.length = size;
			this.bytes = bytes;
			this.alive = true;
			this.position = 0;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Updates the file this stream is associated with.
		/// </summary>
		public override void Flush()
		{
			if (!this.alive)
			{
				throw new ObjectDisposedException("This stream is closed.");
			}

			if (this.length == 0)
			{
				CryArchive.RemoveFilePtr(this.archive, this.name);
			}
			else
			{
				UpdateFile(this.name, this.bytes, this.length);
			}
		}
		/// <summary>
		/// Copies the file data to the given buffer.
		/// </summary>
		/// <param name="buffer">Array of bytes to copy data to.</param>
		/// <param name="offset">Zero-based index of first byte inside the array to copy data to.</param>
		/// <param name="count"> Number of bytes to copy.</param>
		/// <returns>Number of bytes read.</returns>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Zero-based index cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of bytes to read cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Reading to the buffer would cause a buffer overrun.
		/// </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.alive)
			{
				throw new ObjectDisposedException("This stream is closed.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Zero-based index cannot be less then 0.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Number of bytes to read cannot be less then 0.");
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("Reading to the buffer would cause a buffer overrun.");
			}

			int i = 0;
			for (int j = offset; i < count && this.position < this.length; i++)
			{
				buffer[j] = this.bytes[this.position++];
			}
			return i;
		}
		/// <summary>
		/// Moves position of the stream to one indicated by the given <paramref name="offset"/> relative
		/// to the specified <paramref name="origin"/>.
		/// </summary>
		/// <param name="offset">Zero-based index of the new position of the stream.</param>
		/// <param name="origin">Origin position relative to which to move the current position.</param>
		/// <returns>New position.</returns>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Unknown origin was specified.</exception>
		/// <exception cref="ArgumentException">Cannot set position of the stream beyond 4GB.</exception>
		/// <exception cref="ArgumentException">Cannot extend the file beyond 4GB when seeking.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.alive)
			{
				throw new ObjectDisposedException("This stream is closed.");
			}

			switch (origin)
			{
				case SeekOrigin.Begin:
					this.Position = offset;
					break;
				case SeekOrigin.Current:
					this.Position = this.position + offset;
					break;
				case SeekOrigin.End:
					this.Position = this.length + offset;
					break;
				default:
					throw new ArgumentOutOfRangeException("origin", "Unknown origin was specified.");
			}

			return this.position;
		}
		/// <summary>
		/// Changes length of the file.
		/// </summary>
		/// <param name="value">New length of the file.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Cannot set the length of the file to the negative value.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Cannot set the length of the file to more then 4GB.
		/// </exception>
		/// <exception cref="OutOfMemoryException">
		/// Unable to expand the file length to the given number of bytes.
		/// </exception>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="NotSupportedException">
		/// Cannot change the size of the file in the read-only archive.
		/// </exception>
		public override void SetLength(long value)
		{
			if (!this.alive)
			{
				throw new ObjectDisposedException("This stream is closed.");
			}
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot change the size of the file in the read-only archive.");
			}
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", "Cannot set the length of the file to the negative value.");
			}
			if (value > UInt32.MaxValue)
			{
				throw new ArgumentOutOfRangeException("value", "Cannot set the length of the file to more then 4GB.");
			}

			ulong memSize = (ulong)value;
			if ((value == 0 && this.bytes != null) || value > 0)
			{
				IntPtr ptr = CryMarshal.Reallocate((IntPtr)this.bytes, memSize);
				if (ptr == IntPtr.Zero)
				{
					throw new OutOfMemoryException
						(string.Format("Unable to expand the file length to {0} bytes.", value));
				}
				this.bytes = (byte*)ptr;
				this.length = (uint)memSize;
			}
		}
		/// <summary>
		/// Writes data from the given array to the stream starting from the current position.
		/// </summary>
		/// <remarks>
		/// The file's size will be automatically extended to incorporate the data if needed.
		/// </remarks>
		/// <param name="buffer">Array of bytes to write.</param>
		/// <param name="offset">Zero-based index of first element inside the array to write.</param>
		/// <param name="count"> Number of bytes to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Zero-based index cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of bytes to write cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Writing to the buffer would cause a buffer overrun.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// Cannot write into the stream for the read-only archive.
		/// </exception>
		/// <exception cref="ArgumentException">Cannot extend the file beyond 4GB when writing.</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Zero-based index cannot be less then 0.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Number of bytes to write cannot be less then 0.");
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("Reading from the buffer would cause a buffer overrun.");
			}
			fixed (byte* bufferBytes = &buffer[offset])
			{
				this.Write(bufferBytes, count);
			}
		}
		/// <summary>
		/// Writes a single byte into the stream.
		/// </summary>
		/// <param name="value">A byte to write into the stream.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="NotSupportedException">
		/// Cannot write into the stream for the read-only archive.
		/// </exception>
		/// <exception cref="ArgumentException">Cannot extend the file beyond 4GB when writing.</exception>
		public override void WriteByte(byte value)
		{
			this.Write(&value, 1);
		}
		/// <summary>
		/// Updates the file in the archive and marks this stream as closed.
		/// </summary>
		/// <param name="disposing">Not used.</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.alive)
			{
				return;
			}

			this.Flush();

			CryMarshal.Free(new IntPtr(this.bytes));
			Marshal.FreeHGlobal(this.name);

			this.alive = false;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateFile(IntPtr path, byte* data, uint length);
		private void Write(byte* data, int count)
		{
			if (!this.alive)
			{
				throw new ObjectDisposedException("This stream is closed.");
			}
			if (!this.canWrite)
			{
				throw new NotSupportedException("Cannot write into the stream for the read-only archive.");
			}
			// Extend the file if overrun is imminent.
			if (this.position > this.length - count)
			{
				try
				{
					this.SetLength(this.position + count);
				}
				catch (ArgumentOutOfRangeException ex)
				{
					throw new ArgumentException("Cannot extend the file beyond 4GB when writing.", ex);
				}
			}
			// Copy stuff.
			for (int i = 0; i < count; i++)
			{
				this.bytes[this.position++] = data[i];
			}
		}
		#endregion
	}
}