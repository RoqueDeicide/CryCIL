using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;
using CryEngine.StaticObjects.Native;

namespace CryEngine.StaticObjects
{
	/// <summary>
	/// Represents a static object in CryEngine world.
	/// </summary>
	public class StaticObject : IDisposable
	{
		internal readonly IntPtr handle;
		/// <summary>
		/// Indicates whether this static object is usable.
		/// </summary>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Creates a new empty static object.
		/// </summary>
		/// <remarks>
		/// This constructor invokes I3DEngine-&gt;CreateStatObj() method that initializes empty
		/// instance of IStatObj, that does not contain any mesh data.
		///
		/// This means that you should use this constructor when you need to create a mesh object
		/// from scratch.
		///
		/// Failure to create new object will mark this wrapper as disposed.
		/// </remarks>
		public StaticObject()
		{
			this.handle = NativeStaticObjectMethods.CreateStaticObject();
			this.Disposed = this.handle == IntPtr.Zero;
		}
		// Used by EntityBase.GetStaticObject(int).
		internal StaticObject(IntPtr handle)
		{
			this.handle = handle;
			this.Disposed = this.handle == IntPtr.Zero;
		}
		~StaticObject()
		{
			this.Dispose(false);
		}
		/// <summary>
		/// Signals CryEngine run-time that this static object is no longer needed in Mono environment.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		/// <summary>
		/// Performs disposal of this instance.
		/// </summary>
		/// <param name="disposeManagedResources">
		/// Indicates whether all resources allocated in managed environment must be freed.
		/// </param>
		protected void Dispose(bool disposeManagedResources)
		{
			if (this.Disposed)
			{
				return;
			}
			NativeStaticObjectMethods.ReleaseStaticObject(this.handle);
			this.Disposed = true;
		}
	}
}