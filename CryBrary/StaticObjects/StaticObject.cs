using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics.Geometry.Meshes;
using CryEngine.Native;

namespace CryEngine.StaticObjects
{
	/// <summary>
	/// Represents a static object in CryEngine world.
	/// </summary>
	public class StaticObject : IDisposable
	{
		internal readonly IntPtr Handle;
		/// <summary>
		/// Indicates whether this static object is usable.
		/// </summary>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Gets object that provides access to mesh data in native memory.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// Cannot acquire mesh data from static object that has been disposed of.
		/// </exception>
		/// <exception cref="Exception">
		/// Unable to acquire mesh handles for the static object.
		/// </exception>
		public NativeMesh Mesh
		{
			get { return new NativeMesh(this); }
		}
		/// <summary>
		/// Creates a new empty static object.
		/// </summary>
		/// <remarks>
		/// This constructor invokes I3DEngine-&gt;CreateStatObj() method that initializes
		/// empty instance of IStatObj, that does not contain any mesh data.
		/// 
		/// This means that you should use this constructor when you need to create a mesh
		/// object from scratch.
		/// 
		/// Failure to create new object will mark this wrapper as disposed.
		/// </remarks>
		public StaticObject()
		{
			this.Handle = StaticObjectInterop.CreateStaticObject();
			this.Disposed = this.Handle == IntPtr.Zero;
		}
		// Used by EntityBase.GetStaticObject(int).
		internal StaticObject(IntPtr handle)
		{
			this.Handle = handle;
			this.Disposed = this.Handle == IntPtr.Zero;
		}
		~StaticObject()
		{
			this.Dispose(false);
		}
		/// <summary>
		/// Signals CryEngine run-time environment that this static object is no longer
		/// needed in Mono environment.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		/// <summary>
		/// Performs disposal of this instance.
		/// </summary>
		/// <param name="disposeManagedResources">
		/// Indicates whether all resources allocated in managed environment must be
		/// freed.
		/// </param>
		protected void Dispose(bool disposeManagedResources)
		{
			if (this.Disposed)
			{
				return;
			}
			StaticObjectInterop.ReleaseStaticObject(this.Handle);
			this.Disposed = true;
		}
	}
}