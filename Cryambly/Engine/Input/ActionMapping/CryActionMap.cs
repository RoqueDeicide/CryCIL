using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Represents a CryEngine input action map.
	/// </summary>
	public struct CryActionMap
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the action of specified name.
		/// </summary>
		/// <param name="name">Name of the action to get.</param>
		/// <returns>A valid object of type <see cref="CryInputAction"/> if action was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public CryInputAction this[string name]
		{
			get
			{
				this.AssertInstance();

				return GetAction(this.handle, name);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this action map should be active.
		/// </summary>
		public bool Enabled
		{
			get
			{
				this.AssertInstance();

				return IsEnabled(this.handle);
			}
			set
			{
				this.AssertInstance();

				Enable(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the name of this action map.
		/// </summary>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
		}
		/// <summary>
		/// Gets the CRC32 hash code of the lower-case version of the name of this action map.
		/// </summary>
		public uint NameHash
		{
			get
			{
				this.AssertInstance();

				return GetNameHash(this.handle);
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		internal CryActionMap(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates an input action.
		/// </summary>
		/// <param name="name">Name of the action.</param>
		/// <returns>
		/// A valid object of type <see cref="CryInputAction"/> if the action was created successfully.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public CryInputAction CreateAction(string name)
		{
			this.AssertInstance();

			return CreateActionInternal(this.handle, name);
		}
		/// <summary>
		/// Removes an input action.
		/// </summary>
		/// <param name="name">Name of the action.</param>
		/// <returns>True, if action was found and removed.</returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public bool RemoveAction(string name)
		{
			this.AssertInstance();

			return RemoveActionInternal(this.handle, name);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not usable.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryInputAction GetAction(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryInputAction CreateActionInternal(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveActionInternal(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Enable(IntPtr handle, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetNameHash(IntPtr handle);
		#endregion
	}
}