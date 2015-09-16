using System;
using System.Diagnostics.Contracts;
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
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets the action of specified name.
		/// </summary>
		/// <param name="name">Name of the action to get.</param>
		/// <returns>A valid object of type <see cref="CryInputAction"/> if action was found.</returns>
		public CryInputAction this[string name]
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetAction(this.handle, name);
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
		public CryInputAction CreateAction(string name)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return CreateActionInternal(this.handle, name);
		}
		/// <summary>
		/// Removes an input action.
		/// </summary>
		/// <param name="name">Name of the action.</param>
		/// <returns>True, if action was found and removed.</returns>
		public bool RemoveAction(string name)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return RemoveActionInternal(this.handle, name);
		}
		#endregion
		#region Utilities
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
		#endregion
	}
}