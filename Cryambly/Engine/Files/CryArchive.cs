using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.RunTime;
using CryCil.Utilities;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of options that can be selected when creating objects of type <see cref="CryArchive"/>.
	/// </summary>
	[Flags]
	public enum ArchiveOptions : uint
	{
		/// <summary>
		/// When set, indicates that the archive object will support complex path specifications (e.g.
		/// absolute paths). All non-absolute paths are treated as relative to the CryEngine work directory.
		/// </summary>
		/// <remarks>This flag and <see cref="SimplePaths"/> are mutually exclusive.</remarks>
		ComplexPaths = 1,
		/// <summary>
		/// When set, indicates that the archive object will only support simple relative file path types,
		/// that are also treated as relative to the zip archive file itself.
		/// </summary>
		/// <remarks>
		/// <para>Working with files in this mode is faster, which useful for frequent accesses.</para>
		/// <para>This flag and <see cref="ComplexPaths"/> are mutually exclusive.</para>
		/// </remarks>
		/// <example>
		/// From CryEngine documentation: <c>Examples/ExampleText.txt</c> in Examples.pak file.
		/// </example>
		SimplePaths = 2,
		/// <summary>
		/// When set, disables update/remove operations for this archive.
		/// </summary>
		/// <remarks>
		/// When this flag and <see cref="SimplePaths"/> are both set, then the archive object will have
		/// extra optimizations applied to it.
		/// </remarks>
		ReadOnly = 4,
		/// <summary>
		/// When set, disables update/remove operations for this archive and forces a number of quick access
		/// and memory footprint optimizations.
		/// </summary>
		OptimizedReadOnly = 8,
		/// <summary>
		/// When set, instructs the system to wipe the file if it already existed before opening.
		/// </summary>
		CreateNew = 16,
		/// <summary>
		/// When set, instructs the underlying system to not compact the archive file when it's closed.
		/// </summary>
		/// <remarks>
		/// Useful when the archive file is being opened for writing and closed multiple times.
		/// </remarks>
		DontCompact = 32,
		/// <summary>
		/// When set, indicates that the archive is loaded into memory.
		/// </summary>
		InMemory = 64,
		/// <summary>
		/// Unknown.
		/// </summary>
		InMemoryCpu = 128,
		/// <summary>
		/// Mask for detection of <see cref="InMemory"/> and <see cref="InMemoryCpu"/> flags.
		/// </summary>
		InMemoryMask = InMemory | InMemoryCpu,
		/// <summary>
		/// When set, indicates that names of files in the archive are stored as crc32 in a flat directory
		/// structure.
		/// </summary>
		FilenamesAsCrc32 = 256,
		/// <summary>
		/// When set, indicates that the archive is stored on HDD.
		/// </summary>
		OnHdd = 512,
		/// <summary>
		/// Paks opened with this flag go at the end of the list and contents will be found before other
		/// paks.
		/// </summary>
		/// <remarks>Used for patching.</remarks>
		OverridePak = 1024,
		/// <summary>
		/// When set, renders the archive inaccessible, while keeping it loaded.
		/// </summary>
		/// <remarks>Used primarily for multiplayer.</remarks>
		DisablePak = 2048
	}
	/// <summary>
	/// Represents a .pak file.
	/// </summary>
	public unsafe class CryArchive : IDisposable
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		private readonly string fullPath;
		private bool disposed;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets flags assigned to the archive object.
		/// </summary>
		public extern ArchiveOptions Flags { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets full path to the archive file.
		/// </summary>
		public string FullPath => this.fullPath;
		/// <summary>
		/// Indicates whether this archive is opened in read-only mode.
		/// </summary>
		public bool ReadOnly => (this.Flags & (ArchiveOptions.ReadOnly | ArchiveOptions.OptimizedReadOnly)) != 0;
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Opens an archive.
		/// </summary>
		/// <remarks>
		/// It's important to note that .pak files inside the <see cref="DirectoryStructure.ContentFolder"/>
		/// folder are loaded at the start with <see cref="ArchiveOptions.ReadOnly"/> flag set.
		/// </remarks>
		/// <param name="path">   Path to the archive file.</param>
		/// <param name="options">A set of options that specify how the archive will be loaded.</param>
		/// <exception cref="FileAccessException">Unable to open the archive file.</exception>
		/// <exception cref="FileNotFoundException">Unable to find the archive file.</exception>
		/// <exception cref="DirectoryNotFoundException">
		/// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
		/// </exception>
		/// <exception cref="IOException"><paramref name="path"/> is a file name.</exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// The caller does not have the required permission.
		/// </exception>
		/// <exception cref="OutOfMemoryException">There is insufficient memory available.</exception>
		public CryArchive(string path, ArchiveOptions options)
		{
			this.handle = OpenArchive(StringPool.Get(path), options);
			if (this.handle == IntPtr.Zero)
			{
				if (File.Exists(path))
				{
					throw new FileAccessException(
						$"Unable to open the archive file.{(Directory.EnumerateFiles(DirectoryStructure.ContentFolder).Contains(path) ? "The archives located inside the game content folder can only be opened in read-only mode." : string.Empty)}");
				}
				throw new FileNotFoundException("Unable to find the archive file.");
			}
			this.fullPath = GetFullPath(this.handle);
			this.disposed = false;
		}
		/// <summary>
		/// Closes this archive
		/// </summary>
		~CryArchive()
		{
			this.Dispose();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Opens a file from the archive for reading.
		/// </summary>
		/// <param name="path">Path to the file within the archive.</param>
		/// <returns>
		/// A new object of type <see cref="ArchiveStream"/> that can read contents of the file, or null, if
		/// file wasn't found.
		/// </returns>
		/// <exception cref="ObjectDisposedException">The archive is closed.</exception>
		/// <exception cref="OutOfMemoryException">
		/// Cannot allocate enough memory for the file data.
		/// </exception>
		public ArchiveStream OpenRead(string path)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The archive is closed.");
			}
			return this.Open(path, false);
		}
		/// <summary>
		/// Opens a file from the archive for writing.
		/// </summary>
		/// <param name="path">Path to the file within the archive.</param>
		/// <returns>A new object of type <see cref="ArchiveStream"/> that update the file.</returns>
		/// <exception cref="ObjectDisposedException">The archive is closed.</exception>
		/// <exception cref="OutOfMemoryException">
		/// Cannot allocate enough memory for the file data.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// This operation is not supported in read-only archives.
		/// </exception>
		public ArchiveStream OpenWrite(string path)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The archive is closed.");
			}
			if (this.ReadOnly)
			{
				throw new NotSupportedException("Cannot open a file within the read-only archive for writing.");
			}
			return this.Open(path, true);
		}
		/// <summary>
		/// Deletes the file from the archive.
		/// </summary>
		/// <param name="name">Name of the file to delete.</param>
		/// <exception cref="ObjectDisposedException">The archive is closed.</exception>
		/// <exception cref="NotSupportedException">
		/// This operation is not supported in read-only archives.
		/// </exception>
		public void DeleteFile(string name)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The archive is closed.");
			}
			if (this.ReadOnly)
			{
				throw new NotSupportedException("Cannot delete a file from the read-only archive.");
			}

			RemoveFile(this.handle, name);
		}
		/// <summary>
		/// Deletes the directory from the archive.
		/// </summary>
		/// <param name="name">Name of the directory to delete.</param>
		/// <exception cref="ObjectDisposedException">The archive is closed.</exception>
		/// <exception cref="NotSupportedException">
		/// This operation is not supported in read-only archives.
		/// </exception>
		public void DeleteDirectory(string name)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The archive is closed.");
			}
			if (this.ReadOnly)
			{
				throw new NotSupportedException("Cannot delete a directory from the read-only archive.");
			}

			RemoveDirectory(this.handle, name);
		}
		/// <summary>
		/// Empties this archive.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The archive is closed.</exception>
		/// <exception cref="NotSupportedException">
		/// This operation is not supported in read-only archives.
		/// </exception>
		public void Clear()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("The archive is closed.");
			}
			if (this.ReadOnly)
			{
				throw new NotSupportedException("Cannot empty a read-only archive.");
			}

			ClearAll(this.handle);
		}
		/// <summary>
		/// Closes this archive.
		/// </summary>
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;

			CloseArchive(this.handle);

			this.handle = IntPtr.Zero;

			GC.SuppressFinalize(this);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr OpenArchive(IntPtr name, ArchiveOptions options);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CloseArchive(IntPtr archive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetFullPath(IntPtr archive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr FindFile(IntPtr archive, IntPtr szPath);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetFileSize(IntPtr archive, IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ReadFile(IntPtr archive, IntPtr handle, IntPtr pBuffer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveFile(IntPtr archive, string path);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveFilePtr(IntPtr archive, IntPtr path);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveDirectory(IntPtr archive, string path);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearAll(IntPtr archive);
		/// <exception cref="OutOfMemoryException">There is insufficient memory available.</exception>
		/// <exception cref="OutOfMemoryException">
		/// Cannot allocate enough memory for the file data.
		/// </exception>
		private ArchiveStream Open(string name, bool writing)
		{
			IntPtr pathPtr = Marshal.StringToHGlobalAnsi(name);

			IntPtr fileHandle = FindFile(this.handle, pathPtr);

			if (fileHandle == IntPtr.Zero)
			{
				return writing ? new ArchiveStream(this.handle, pathPtr, true, 0, (byte*)0) : null;
			}

			uint size = GetFileSize(this.handle, fileHandle);

			IntPtr fileBytes = CryMarshal.Allocate(size);

			if (fileBytes == IntPtr.Zero)
			{
				throw new OutOfMemoryException("Cannot allocate enough memory for the file data.");
			}

			return new ArchiveStream(this.handle, pathPtr, writing, size, (byte*)fileBytes);
		}
		#endregion
	}
}