using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Represents a CryEngine input action.
	/// </summary>
	public struct CryInputAction
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
		#endregion
		#region Construction
		internal CryInputAction(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds input to the list of inputs that can activate this action.
		/// </summary>
		/// <param name="input">Identifier of the input that can activate this action.</param>
		/// <param name="spec"> 
		/// Additional information that specify when the action can be activated by this input.
		/// </param>
		/// <returns>True, if input has been successfully added and bound, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public bool AddInput(InputId input, ActionInputSpecification spec)
		{
			this.AssertInstance();

			return AddInputInternal(this.handle, input, spec);
		}
		/// <summary>
		/// Removes input from the list of inputs that can activate this action.
		/// </summary>
		/// <param name="input">Identifier of the input that can activate this action.</param>
		/// <returns>True, if input has been successfully found and removed, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public bool RemoveInput(InputId input)
		{
			this.AssertInstance();

			return RemoveInputInternal(this.handle, input);
		}
		/// <summary>
		/// Rebinds one of the inputs to the different input.
		/// </summary>
		/// <remarks>Input specification remains the same.</remarks>
		/// <param name="oldInput">Input to rebind.</param>
		/// <param name="newInput">
		/// A new input. If equal to <see cref="InputId.Unknown"/> then it will be cleared.
		/// </param>
		/// <returns>True, if successful, otherwise false.</returns>
		/// <exception cref="NullReferenceException">This instance is not usable.</exception>
		public bool RebindInput(InputId oldInput, InputId newInput)
		{
			this.AssertInstance();

			return RebindInputInternal(this.handle, oldInput, newInput);
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
		private static extern bool AddInputInternal(IntPtr handle, InputId input, ActionInputSpecification spec);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveInputInternal(IntPtr handle, InputId input);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RebindInputInternal(IntPtr handle, InputId oldInput, InputId newInput);
		#endregion
	}
}