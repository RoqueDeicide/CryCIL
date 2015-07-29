using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a wrapper object for CryEngine entities.
	/// </summary>
	public struct CryEntity
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the pointer to the underlying object.
		/// </summary>
		public IntPtr Handle
		{
			get { return this.handle; }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets or sets flags that describe this entity.
		/// </summary>
		public EntityFlags Flags
		{
			get
			{
				this.AssertEntity();
				return (EntityFlags)GetFlags(this.handle);
			}
			set
			{
				this.AssertEntity();
				SetFlags(this.handle, (ulong)value);
			}
		}
		#endregion
		#region Construction
		internal CryEntity(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Marks a specified set of flags as set for this entity.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		/// entity.Flags |= someFlags;
		/// </code>
		/// This function is faster because it involves only one internal call and only one validation
		/// check.
		/// </remarks>
		/// <param name="flagsToAdd">Flags to set.</param>
		public void AddFlags(EntityFlags flagsToAdd)
		{
			this.AssertEntity();

			if (flagsToAdd == 0)
			{
				return;
			}

			AddFlagsInternal(this.handle, (ulong)flagsToAdd);
		}
		/// <summary>
		/// Removes flags from the current set of entity flags.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		///  entity.Flags &amp;= ~someFlags;
		/// </code>
		/// This function is faster because it involves only one internal call and only one validation
		/// check.
		/// </remarks>
		/// <param name="flagsToClear">Combination of bit flags to remove.</param>
		public void ClearFlags(EntityFlags flagsToClear)
		{
			this.AssertEntity();

			if (flagsToClear == 0)
			{
				return;
			}

			ClearFlagsInternal(this.handle, (ulong)flagsToClear);
		}
		/// <summary>
		/// Checks if specified set of flags is enabled.
		/// </summary>
		/// <remarks>
		/// This function is a faster equivalent of the following code:
		/// <code>
		/// entity.Flags.HasFlag(someFlags);
		/// </code>This function is faster because it doesn't involve boxing.
		/// </remarks>
		/// <param name="flagsToCheck">A combination of flags to check.</param>
		/// <param name="all">         
		/// Indicates whether all specified flags must be check in order for this method to return
		/// <c>true</c>.
		/// </param>
		/// <returns>True, if all specified flags are checked.</returns>
		public bool CheckFlags(EntityFlags flagsToCheck, bool all = true)
		{
			this.AssertEntity();

			return flagsToCheck == 0 || CheckFlagsInternal(this.handle, (ulong)flagsToCheck, all);
		}

		#endregion
		#region Utilities
		// Assertion method.
		private void AssertEntity()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("This entity is not usable.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, ulong flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ulong GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddFlagsInternal(IntPtr handle, ulong flagsToAdd);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearFlagsInternal(IntPtr handle, ulong flagsToClear);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CheckFlagsInternal(IntPtr handle, ulong flagsToCheck, bool all);
		#endregion
	}
}