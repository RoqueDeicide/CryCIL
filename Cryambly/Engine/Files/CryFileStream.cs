using System;
using System.IO;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Represents a stream object that grants access to the file in the virtual CryEngine file system.
	/// </summary>
	public class CryFileStream : Stream
	{
		#region Fields
		private bool opened;
		private readonly CryFileMode mode;
		private IntPtr fileHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this object can read from the file.
		/// </summary>
		public override bool CanRead
		{
			get { return this.opened && this.mode == CryFileMode.Read; }
		}
		/// <summary>
		/// Indicates whether this stream can change its position.
		/// </summary>
		public override bool CanSeek
		{
			get { return this.opened; }
		}
		/// <summary>
		/// Indicates whether it's possibly to write into this stream.
		/// </summary>
		public override bool CanWrite
		{
			get { return this.opened && this.mode == CryFileMode.Append || this.mode == CryFileMode.Write; }
		}
		/// <summary>
		/// Gets the length of the file.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		public override long Length
		{
			get
			{
				if (!this.opened)
				{
					throw new ObjectDisposedException("Stream is closed.");
				}
				return CryFiles.GetSize(this.fileHandle);
			}
		}
		/// <summary>
		/// Gets or sets current file position.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		public override long Position
		{
			get
			{
				if (!this.opened)
				{
					throw new ObjectDisposedException("Stream is closed.");
				}
				return CryFiles.GetCurrentPosition(this.fileHandle);
			}
			set
			{
				if (!this.opened)
				{
					throw new ObjectDisposedException("Stream is closed.");
				}
				CryFiles.Seek(this.fileHandle, (int)value, SeekOrigin.Begin);
			}
		}
		/// <summary>
		/// Gets the path to the file.
		/// </summary>
		public string Path { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Opens a stream that will access a file inside the CryEngine virtual file system.
		/// </summary>
		/// <param name="path">        Path to the file.</param>
		/// <param name="mode">        Mode in which the file will be opened.</param>
		/// <param name="type">        Type as which the file will be recognized.</param>
		/// <param name="directAccess">Indicates if low-level access to the file should be used.</param>
		/// <param name="flags">       A set of flags that further specify the opening process.</param>
		/// <exception cref="ArgumentOutOfRangeException">Invalid file opening mode specified.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Invalid file recognition type specified.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		/// File could not be found in the virtual file system.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Text file type recognition is not supported for files in .pak archives.
		/// </exception>
		/// <exception cref="FileAccessException">Unable to access a file.</exception>
		public CryFileStream(string path, CryFileMode mode, CryFileType type, bool directAccess = false,
							 FileOpenFlags flags = FileOpenFlags.Nothing)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", "Cannot open a file using a null name.");
			}

			uint encodedFlags = EncodeFlags(mode, type, directAccess);

#if DEBUG
			if (!CryFiles.Exists(path))
			{
				throw new FileNotFoundException(
					string.Format("File could not be found in the virtual file system using a path = \"{0}\".",
								  path));
			}

			if (type == CryFileType.Text && CryFiles.Exists(path, SearchLocation.Pak))
			{
				throw new ArgumentException("Text file type recognition is not supported for files in .pak archives.");
			}
#endif

			this.fileHandle = CryFiles.Open(path, ref encodedFlags, flags);

			if (this.fileHandle == IntPtr.Zero)
			{
				throw new FileAccessException("Unable to access a file.");
			}

			this.Path = path;
			this.mode = mode;
			this.opened = true;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Flushes internal buffers.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		public override void Flush()
		{
			if (!this.opened)
			{
				throw new ObjectDisposedException("Stream is closed.");
			}
			CryFiles.Flush(this.fileHandle);
		}
		/// <summary>
		/// Reads file data into the given buffer.
		/// </summary>
		/// <param name="buffer">Array of bytes to write data into.</param>
		/// <param name="offset">
		/// Zero-based index of first byte inside the array to write data into.
		/// </param>
		/// <param name="count"> Max number of bytes to read.</param>
		/// <returns>Number of bytes read.</returns>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Zero-based index cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of bytes to read cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Reading to the buffer would cause a buffer overrun.
		/// </exception>
		/// <exception cref="NotSupportedException">Reading is not supported for this stream.</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.opened)
			{
				throw new ObjectDisposedException("Stream is closed.");
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
			if (!this.CanRead)
			{
				throw new NotSupportedException("Reading is not supported for this stream.");
			}

			return CryFiles.ReadBytes(this.fileHandle, buffer, offset, count);
		}
		/// <summary>
		/// Moves position of the stream to one indicated by the given <paramref name="offset"/> relative
		/// to the specified <paramref name="origin"/>.
		/// </summary>
		/// <param name="offset">Zero-based index of the new position of the stream.</param>
		/// <param name="origin">Origin position relative to which to move the current position.</param>
		/// <returns>New position.</returns>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Unknown origin was specified.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Cannot change position of the stream by value greater then <see cref="Int32.MaxValue"/>.
		/// </exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.opened)
			{
				throw new ObjectDisposedException("Stream is closed.");
			}
			switch (origin)
			{
				case SeekOrigin.Begin:
				case SeekOrigin.Current:
				case SeekOrigin.End:
					break;
				default:
					throw new ArgumentOutOfRangeException("origin", "Unknown origin was specified.");
			}
			if (offset > Int32.MaxValue)
			{
				throw new ArgumentOutOfRangeException();
			}
			return CryFiles.Seek(this.fileHandle, (int)offset, origin);
		}
		/// <summary>
		/// Throws <see cref="NotSupportedException"/> error.
		/// </summary>
		/// <param name="value">Ignored.</param>
		/// <exception cref="NotSupportedException">
		/// Cannot set length of the file in a virtual file system.
		/// </exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException("Cannot set length of the file in a virtual file system.");
		}
		/// <summary>
		/// Writes data into the file
		/// </summary>
		/// <param name="buffer">Array of bytes to write.</param>
		/// <param name="offset">Zero-based index of first element inside the array to write.</param>
		/// <param name="count"> Number of bytes to write.</param>
		/// <exception cref="ObjectDisposedException">Stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Zero-based index cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of bytes to write cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Writing to the buffer would cause a buffer overrun.
		/// </exception>
		/// <exception cref="NotSupportedException">Writing is not supported for this stream.</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.opened)
			{
				throw new ObjectDisposedException("Stream is closed.");
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
				throw new ArgumentException("Writing from the buffer would cause a buffer overrun.");
			}
			if (!this.CanWrite)
			{
				throw new NotSupportedException("Writing is not supported for this stream.");
			}

			CryFiles.WriteBytes(this.fileHandle, buffer, offset, count);
		}
		#endregion
		#region Utilities
		private static uint EncodeFlags(CryFileMode mode, CryFileType type, bool directAccess)
		{
			uint flags = 0;

			switch (mode)
			{
				case CryFileMode.Read:
					flags |= (byte)'r' << 24;
					break;
				case CryFileMode.Write:
					flags |= (byte)'w' << 24;
					break;
				case CryFileMode.Append:
					flags |= (byte)'a' << 24;
					break;
				default:
					throw new ArgumentOutOfRangeException("mode", "Invalid file opening mode specified.");
			}

			switch (type)
			{
				case CryFileType.Binary:
					flags |= (byte)'b' << 16;
					break;
				case CryFileType.Text:
					flags |= (byte)'t' << 16;
					break;
				default:
					throw new ArgumentOutOfRangeException("type", "Invalid file recognition type specified.");
			}

			if (directAccess)
			{
				flags |= (byte)'x' << 8;
			}

			return flags;
		}
		/// <summary>
		/// Closes the stream.
		/// </summary>
		/// <param name="disposing">Ignored.</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.opened)
			{
				return;
			}
			this.opened = false;
			CryFiles.Close(this.fileHandle);
			this.fileHandle = IntPtr.Zero;
		}
		#endregion
	}
}