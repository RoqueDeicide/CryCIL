using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Represents object that provides stream-like access to native memory cluster.
	/// </summary>
	public class NativeMemoryStream : IDisposable
	{
		#region Fields
		private readonly NativeArray underlyingArray;
		private ulong position;
		private IBuffer streamBuffer;
		/// <summary>
		/// Zero-based index of the first byte in native memory cluster that is associated with the
		/// first byte in stream buffer.
		/// </summary>
		private ulong bufferPosition;
		private readonly StreamMode mode;
		/// <summary>
		/// <para>True - last operation was reading.</para><para>False
		/// - last operation was writing.</para><para>Null - nothing
		/// was done since creation of this object.</para>
		/// </summary>
		private bool? wasReading;
		/// <summary>
		/// <para>True - we are busy reading.</para><para>False - we are busy
		/// writing.</para><para>Null - we are not doing anything.</para>
		/// </summary>
		private bool? currentOperationIsReading;
		#endregion
		#region Properties
		/// <summary>
		/// Gets length of the memory cluster which is handle by this stream.
		/// </summary>
		public ulong Length
		{
			get { return this.underlyingArray.Length; }
		}
		/// <summary>
		/// Indicates whether this stream object can read.
		/// </summary>
		public bool CanRead
		{
			get { return this.mode != StreamMode.Write; }
		}
		/// <summary>
		/// True.
		/// </summary>
		public bool CanSeek
		{
			get { return true; }
		}
		/// <summary>
		/// Indicates whether this stream object can write.
		/// </summary>
		public bool CanWrite
		{
			get { return this.mode != StreamMode.Read; }
		}
		/// <summary>
		/// Gets or sets current position.
		/// </summary>
		public ulong Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (value < this.underlyingArray.Length)
				{
					throw new IndexOutOfRangeException("NativeMemoryStream.Position.Set: You cannot set position of the stream beyond its operating range.");
				}
				this.position = value;
				// If new position is beyond current stream buffer's dimensions.
				if (this.position < this.bufferPosition || this.position > this.bufferPosition + this.streamBuffer.Length)
				{
					this.Flush();
					if (this.currentOperationIsReading == true)
					{
						this.PrepareBufferForReading();
					}
					else if (this.currentOperationIsReading == false)
					{
						this.PrepareBufferForWriting();
					}
				}
			}
		}
		/// <summary>
		/// Indicates whether this stream should free native memory when closed.
		/// </summary>
		public bool Managing { get; private set; }
		/// <summary>
		/// Indicates whether this stream is disposed.
		/// </summary>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Gets or sets current stream mode.
		/// </summary>
		public StreamMode Mode
		{
			get { return this.mode; }
			set
			{
				if (this.mode != value)
				{
					this.Flush();
				}
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of type <see cref="NativeMemoryStream" />.
		/// </summary>
		/// <param name="array">Native memory cluster to use for this stream.</param>
		public NativeMemoryStream(NativeArray array, StreamMode mode)
		{
			this.underlyingArray = array;
			this.mode = mode;
			this.currentOperationIsReading = null;
			this.wasReading = null;
			this.streamBuffer = null;
			this.bufferPosition = 0;
			this.position = 0;
		}
		/// <summary>
		/// Allocates a native memory cluster and initializes new instance of type <see
		/// cref="NativeMemoryStream" /> to access that cluster.
		/// </summary>
		/// <remarks>
		/// Allocated data will be released when this stream is closed.
		/// </remarks>
		/// <param name="size">Size of native memory cluster to allocate.</param>
		/// <param name="mode">Access mode for the stream.</param>
		public NativeMemoryStream(ulong size, StreamMode mode)
		{
			if (mode == StreamMode.Read)
			{
				throw new NotSupportedException("NativeMemoryStream.Constructor: There is no point in creating read-only stream for data that can only be accessed by the stream.");
			}

			this.Managing = true;

			this.underlyingArray = new NativeArray(CryModule.AllocateMemory(size), size, true, false);

			this.mode = mode;
			this.currentOperationIsReading = null;
			this.wasReading = null;
			this.streamBuffer = null;
			this.bufferPosition = 0;
			this.position = 0;
		}
		/// <summary>
		/// Allocates a native memory cluster and initializes new instance of type <see
		/// cref="NativeMemoryStream" /> to access that cluster.
		/// </summary>
		/// <remarks>
		/// Allocated data is handled by <see cref="CryMarshal" />.
		/// </remarks>
		/// <param name="size">Size of native memory cluster to allocate.</param>
		/// <param name="mode">Access mode for the stream.</param>
		/// <param name="handle">Pointer to allocated memory cluster.</param>
		public NativeMemoryStream(ulong size, StreamMode mode, out IntPtr handle)
		{
			this.Managing = false;

			handle = CryMarshal.AllocateMemory(size);

			this.underlyingArray = new NativeArray(handle, size, false, true);

			this.mode = mode;
			this.currentOperationIsReading = null;
			this.wasReading = null;
			this.streamBuffer = null;
			this.bufferPosition = 0;
			this.position = 0;
		}
		~NativeMemoryStream()
		{
			this.Close();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Flushes the internal buffer.
		/// </summary>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		public void Flush()
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeMemoryStream.Seek: This stream is closed.");
			}
			if (this.streamBuffer == null)
			{
				return;
			}
			switch (this.mode)
			{
				case StreamMode.Read:
					break;
				case StreamMode.Write:
					this.streamBuffer.Set(this.underlyingArray.Handle, this.bufferPosition);
					break;
				case StreamMode.ReadWrite:
					if (this.wasReading == false)
					{
						this.streamBuffer.Set(this.underlyingArray.Handle, this.bufferPosition);
					}
					break;
				default:
					throw new NotSupportedException("NativeMemoryStream.Flush: Invalid streaming mode.");
			}
			this.streamBuffer = null;
		}
		/// <summary>
		/// Sets position of the stream.
		/// </summary>
		/// <param name="offset">Offset to add to value defined by next parameter.</param>
		/// <param name="origin">Value relative to which to set the position.</param>
		/// <returns>The new position within the stream.</returns>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Attempt to set the position of the stream outside of bounds.
		/// </exception>
		/// <exception cref="ArgumentException">Invalid origin of seeking.</exception>
		public ulong Seek(long offset, SeekOrigin origin)
		{
			this.ValidateThisInstance();
			if (offset == 0) return this.position;
			switch (origin)
			{
				case SeekOrigin.Begin:
					if (offset < 0 || (ulong)offset >= this.Length)
					{
						throw new ArgumentOutOfRangeException("offset", "NativeMemoryStream.Seek: Attempt to set the position of the stream outside of bounds.");
					}
					break;
				case SeekOrigin.Current:
					if (offset < 0 && (ulong)(-offset) > this.position)
					{
						throw new ArgumentOutOfRangeException("offset", "NativeMemoryStream.Seek: Attempt to set the position of the stream outside of bounds.");
					}
					if ((ulong)offset + this.position >= this.Length)
					{
						throw new ArgumentOutOfRangeException("offset", "NativeMemoryStream.Seek: Attempt to set the position of the stream outside of bounds.");
					}
					if (offset > 0)
					{
						this.position += (ulong)offset;
					}
					else
					{
						this.position -= (ulong)(-offset);
					}
					break;
				case SeekOrigin.End:
					if (offset > 0 || (ulong)(-offset) > this.Length)
					{
						throw new ArgumentOutOfRangeException("offset", "NativeMemoryStream.Seek: Attempt to set the position of the stream outside of bounds.");
					}
					this.position -= (ulong)(-offset);
					break;
				default:
					throw new ArgumentException("NativeMemoryStream.Seek: Invalid origin of seeking.", "origin");
			}
			return this.position;
		}
		/// <summary>
		/// Reads bytes from this stream to given array.
		/// </summary>
		/// <param name="buffer">Given array.</param>
		/// <param name="offset">
		/// Zero-based index of the first byte in the array to which to put read bytes.
		/// </param>
		/// <param name="count">Number of bytes to read.</param>
		/// <returns></returns>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentNullException">Given buffer is not initialized.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// offset + count is greater then length of given array.
		/// </exception>
		/// <exception cref="OverflowException">Too many bytes are requested to be read.</exception>
		public long Read(byte[] buffer, long offset, long count)
		{
			// Check everything.
			this.ValidateThisInstance();
			CheckIfBufferHasEnoughData(buffer, offset, count);
			ValidateBuffer(buffer);
			this.CheckIfDataCanFit((ulong)count);
			// Prepare for reading.
			this.PrepareToRead();
			// Start reading.

			long elementsCountTillSelectionEnd = offset + count;
			for (long i = offset; i < elementsCountTillSelectionEnd; i++)
			{
				buffer[i] = this.streamBuffer[this.position - this.bufferPosition];
				this.Position++;
			}
			CompleteReadingSession();
			return count;
		}
		/// <summary>
		/// Reads bytes from this stream to given array.
		/// </summary>
		/// <param name="buffer">Given array.</param>
		/// <returns></returns>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentNullException">Given buffer is not initialized.</exception>
		/// <exception cref="OverflowException">Too many bytes are requested to be read.</exception>
		public long Read(byte[] buffer)
		{
			return this.Read(buffer, 0, buffer.LongLength);
		}
		/// <summary>
		/// Reads 1 byte from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Byte1 Read1()
		{
			this.ValidateThisInstance();

			Byte1 data = new Byte1();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			data.UnsignedByte = this.CurrentByte;

			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 2 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Bytes2 Read2()
		{
			this.ValidateThisInstance();

			var data = new Bytes2();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			data[0] = this.CurrentByte;
			data[1] = this.CurrentByte;

			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 4 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Bytes4 Read4()
		{
			this.ValidateThisInstance();

			var data = new Bytes4();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 8 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Bytes8 Read8()
		{
			this.ValidateThisInstance();

			var data = new Bytes8();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 32 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Buffer32 Read32()
		{
			this.ValidateThisInstance();

			var data = new Buffer32();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 64 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Buffer64 Read64()
		{
			this.ValidateThisInstance();

			var data = new Buffer64();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 128 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Buffer128 Read128()
		{
			this.ValidateThisInstance();

			var data = new Buffer128();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 256 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Buffer256 Read256()
		{
			this.ValidateThisInstance();

			var data = new Buffer256();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Reads 512 bytes from native memory.
		/// </summary>
		/// <returns>Read data.</returns>
		public Buffer512 Read512()
		{
			this.ValidateThisInstance();

			var data = new Buffer512();

			this.CheckIfDataCanFit(data.Length);
			this.PrepareToRead();

			for (ulong i = 0; i < data.Length; i++)
			{
				data[i] = this.CurrentByte;
			}
			this.CompleteReadingSession();
			return data;
		}
		/// <summary>
		/// Writes data from given array to native memory.
		/// </summary>
		/// <param name="buffer">Array from which to write the bytes.</param>
		/// <param name="offset">
		/// Zero-based index of the first byte within <paramref name="buffer" /> to write.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentNullException">Given buffer is empty.</exception>
		/// <exception cref="ArgumentException">
		/// offset + count is greater then length of given array.
		/// </exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(byte[] buffer, long offset, long count)
		{
			// Check everything.
			this.ValidateThisInstance();
			ValidateBuffer(buffer);
			CheckIfBufferHasEnoughData(buffer, offset, count);
			this.CheckIfDataCanFit((ulong)count);
			this.PrepareToWrite();
			// Start writing.

			long elementsCountTillSelectionEnd = offset + count;
			for (long i = offset; i < elementsCountTillSelectionEnd; i++)
			{
				this.CurrentByte = buffer[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes data from given array to native memory.
		/// </summary>
		/// <param name="buffer">Array from which to write the bytes.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="ArgumentNullException">Given buffer is empty.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(byte[] buffer)
		{
			this.Write(buffer, 0, buffer.LongLength);
		}
		/// <summary>
		/// Writes an 1-byte long integer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Byte1 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(1);
			this.PrepareToWrite();

			this.CurrentByte = value.UnsignedByte;
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 2-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Bytes2 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(2);
			this.PrepareToWrite();

			this.CurrentByte = value.Bytes[0];
			this.CurrentByte = value.Bytes[1];

			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 4-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Bytes4 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 8-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Bytes8 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 32-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Buffer32 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 64-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Buffer64 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 128-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Buffer128 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 256-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Buffer256 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Writes an 512-byte long buffer to stream.
		/// </summary>
		/// <param name="value">Value to write.</param>
		/// <exception cref="ObjectDisposedException">This stream is closed.</exception>
		/// <exception cref="OverflowException">
		/// Too many bytes are requested to be written.
		/// </exception>
		public void Write(Buffer512 value)
		{
			this.ValidateThisInstance();
			this.CheckIfDataCanFit(4);
			this.PrepareToWrite();

			for (ulong i = 0; i < value.Length; i++)
			{
				this.CurrentByte = value.Bytes[i];
			}
			this.CompleteWritingSession();
		}
		/// <summary>
		/// Disposes of this stream.
		/// </summary>
		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Close();
		}
		/// <summary>
		/// Closes this stream.
		/// </summary>
		public void Close()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Flush();
			if (this.Managing)
			{
				this.underlyingArray.Dispose();
			}
			this.Disposed = true;
			GC.SuppressFinalize(this);
		}
		#endregion
		#region Utilities
		/// <summary>
		/// Transfers a bunch of bytes from native memory into buffer.
		/// </summary>
		private void PrepareBufferForReading()
		{
			ulong remainingBytes = this.Length - this.position;
			if (remainingBytes > 512)
			{
				this.streamBuffer = CryMarshal.Get512Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 256)
			{
				this.streamBuffer = CryMarshal.Get256Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 128)
			{
				this.streamBuffer = CryMarshal.Get128Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 64)
			{
				this.streamBuffer = CryMarshal.Get64Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 32)
			{
				this.streamBuffer = CryMarshal.Get32Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 8)
			{
				this.streamBuffer = CryMarshal.Get8Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 4)
			{
				this.streamBuffer = CryMarshal.Get4Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes > 2)
			{
				this.streamBuffer = CryMarshal.Get2Bytes(this.underlyingArray.Handle, this.position);
			}
			if (remainingBytes == 1)
			{
				this.streamBuffer = CryMarshal.GetByte(this.underlyingArray.Handle, this.position);
			}
			this.bufferPosition = this.position;
		}
		/// <summary>
		/// Assign buffer long enough to fit into remaining portion of native memory cluster to streamBuffer.
		/// </summary>
		private void PrepareBufferForWriting()
		{
			ulong remainingBytes = this.Length - this.position;
			if (remainingBytes > 512)
			{
				this.streamBuffer = new Buffer512();
			}
			if (remainingBytes > 256)
			{
				this.streamBuffer = new Buffer256();
			}
			if (remainingBytes > 128)
			{
				this.streamBuffer = new Buffer128();
			}
			if (remainingBytes > 64)
			{
				this.streamBuffer = new Buffer64();
			}
			if (remainingBytes > 32)
			{
				this.streamBuffer = new Buffer32();
			}
			if (remainingBytes > 8)
			{
				this.streamBuffer = new Bytes8();
			}
			if (remainingBytes > 4)
			{
				this.streamBuffer = new Bytes4();
			}
			if (remainingBytes > 2)
			{
				this.streamBuffer = new Bytes2();
			}
			if (remainingBytes == 1)
			{
				this.streamBuffer = new Byte1();
			}
			this.bufferPosition = this.position;
		}

		// ReSharper disable UnusedParameter.Local
		private static void CheckIfBufferHasEnoughData(byte[] buffer, long offset, long count)
		// ReSharper restore UnusedParameter.Local
		{
			if (offset + count > buffer.Length)
			{
				throw new ArgumentException(String.Format("NativeMemoryStream.{0}: offset + count is greater then length of given array.", new StackTrace().GetFrame(1).GetMethod().Name));
			}
		}

		private static void ValidateBuffer(byte[] buffer)
		{
			if (buffer == null || buffer.Length == 0)
			{
				throw new ArgumentNullException("buffer", String.Format("NativeMemoryStream.{0}: Given buffer is empty.", new StackTrace().GetFrame(1).GetMethod().Name));
			}
		}

		// ReSharper disable UnusedParameter.Local
		private void CheckIfDataCanFit(ulong count)
		// ReSharper restore UnusedParameter.Local
		{
			if (this.position + count > this.Length)
			{
				throw new OverflowException(String.Format("NativeMemoryStream.{0}: This stream doesn't have access to enough data for operation.", new StackTrace().GetFrame(1).GetMethod().Name));
			}
		}

		private void PrepareToWrite()
		{
			switch (this.mode)
			{
				case StreamMode.Read:
					throw new NotSupportedException(String.Format("NativeMemoryStream.{0}: This stream object does not support writing.", new StackTrace().GetFrame(1).GetMethod().Name));
				case StreamMode.Write:
					break;
				case StreamMode.ReadWrite:
					if (this.wasReading == true)
					{
						// End reading session.
						this.Flush();
					}
					if (this.wasReading == null || this.wasReading == true)
					{
						// Prepare buffer to write.
						this.PrepareBufferForWriting();
					}
					break;
				default:
					throw new NotSupportedException(String.Format(
						"NativeMemoryStream.{0}: This stream object is configured to use unknown stream mode.", new StackTrace().GetFrame(1).GetMethod().Name));
			}
			this.currentOperationIsReading = false;
		}

		private void PrepareToRead()
		{
			switch (this.mode)
			{
				case StreamMode.Read:
					break;
				case StreamMode.Write:
					throw new NotSupportedException(String.Format("NativeMemoryStream.{0}: This stream object does not support reading.", new StackTrace().GetFrame(1).GetMethod().Name));
				case StreamMode.ReadWrite:
					if (this.wasReading == false)
					{
						// End writing session.
						this.Flush();
					}
					if (this.wasReading == null || this.wasReading == false)
					{
						// Prepare buffer to read.
						this.PrepareBufferForReading();
					}
					break;
				default:
					throw new NotSupportedException(String.Format(
						"NativeMemoryStream.{0}: This stream object is configured to use unknown stream mode.", new StackTrace().GetFrame(1).GetMethod().Name));
			}
			this.currentOperationIsReading = true;
		}

		private void ValidateThisInstance()
		{
			if (!this.Disposed) return;

			StackTrace stackTrace = new StackTrace();
			throw new ObjectDisposedException(String.Format("NativeMemoryStream.{0}: This stream is closed.", stackTrace.GetFrame(1).GetMethod().Name));
		}

		private void CompleteWritingSession()
		{
			this.currentOperationIsReading = null;
			this.wasReading = false;
		}

		private void CompleteReadingSession()
		{
			this.currentOperationIsReading = null;
			this.wasReading = true;
		}

		private byte CurrentByte
		{
			get
			{
				byte b = this.streamBuffer[this.position - this.bufferPosition];
				this.Position++;
				return b;
			}
			set
			{
				this.streamBuffer[this.position - this.bufferPosition] = value;
				this.Position++;
			}
		}
		#endregion
	}
	/// <summary>
	/// Enumeration of usage mode for <see cref="NativeMemoryStream" />.
	/// </summary>
	public enum StreamMode
	{
		/// <summary>
		/// Stream is used for reading.
		/// </summary>
		Read,
		/// <summary>
		/// Stream is used for writing.
		/// </summary>
		Write,
		/// <summary>
		/// Stream is used for both reading and writing.
		/// </summary>
		ReadWrite
	}
}