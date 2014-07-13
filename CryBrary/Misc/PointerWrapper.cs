using System;

namespace CryEngine.Utilities
{
	/// <summary>
	/// Wrapper to be able to make sure that we're getting the correct handle in C++. In native
	/// code, interpret as struct MyStruct { void *pHandle; } (See mono::pointer)
	/// </summary>
	public struct PointerWrapper
	{
		public PointerWrapper(IntPtr pointer)
		{
			Handle = pointer;
		}

		public bool IsZero { get { return Handle == IntPtr.Zero; } }

		public IntPtr Handle;
	}
}