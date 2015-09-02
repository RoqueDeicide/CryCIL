using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Utilities
{
	/// <summary>
	/// A special object that can be used in certain operations to lock access to certain data. Use
	/// <c>using</c> statement to make sure that the lock is released as soon as possible.
	/// </summary>
	/// <example>
	/// <code>
	/// using (var @lock = new WriteLockCond())	// Don't use default constructor.
	/// {
	///     SomeOperationThatRequiresLock(@lock);
	/// }	// The lock is released here.
	/// </code>
	/// </example>
	[StructLayout(LayoutKind.Sequential)]
	public class WriteLockCond : IDisposable
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the internal handle of this object.
		/// </summary>
		public IntPtr Handle
		{
			get { return this.handle; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new lock.
		/// </summary>
		public WriteLockCond()
		{
			this.handle = CreateLock();
		}
		~WriteLockCond()
		{
			this.Dispose(true);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this lock.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(false);
		}
		private void Dispose(bool calledFromDestructor)
		{
			if (!calledFromDestructor)
			{
				GC.SuppressFinalize(this);
			}
			if (this.handle != IntPtr.Zero)
			{
				ReleaseLock(this.handle);
			}
			this.handle = IntPtr.Zero;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateLock();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReleaseLock(IntPtr handle);
		#endregion
	}
}