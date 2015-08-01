using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a wrapper object for CryEngine entities.
	/// </summary>
	public partial struct CryEntity
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

				Contract.EndContractBlock();

				return (EntityFlags)GetFlags(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetFlags(this.handle, (ulong)value);
			}
		}
		/// <summary>
		/// Indicates whether this entity is scheduled to be deleted on the next frame.
		/// </summary>
		public bool IsGarbage
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetIsGarbage(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the name of this entity.
		/// </summary>
		/// <remarks>The name is not required to be unique.</remarks>
		/// <exception cref="ArgumentNullException">Name of the entity cannot be null.</exception>
		public string Name
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetNameInternal(this.handle);
			}
			set
			{
				this.AssertEntity();
				if (value == null)
				{
					throw new ArgumentNullException("value", "Name of the entity cannot be null.");
				}

				Contract.EndContractBlock();

				SetNameInternal(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this entity was created by the entity load manager using information stored
		/// in the level file.
		/// </summary>
		/// <remarks>Returns <c>false</c> for any entities created dynamically through code.</remarks>
		public bool IsLoadedFromLevelFile
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetIsLoadedFromLevelFile(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this entity is a part of the entity pool.
		/// </summary>
		public bool IsFromPool
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetIsFromPool(this.handle);
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

			Contract.EndContractBlock();

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

			Contract.EndContractBlock();

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

			Contract.EndContractBlock();

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
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsGarbage(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetNameInternal(IntPtr handle, string sName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetNameInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsLoadedFromLevelFile(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsFromPool(IntPtr handle);
		#endregion
	}
}