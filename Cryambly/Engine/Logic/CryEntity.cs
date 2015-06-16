using System;
using System.Collections.Generic;
using System.Linq;
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
		#endregion
		#region Events

		#endregion
		#region Construction
		internal CryEntity(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities

		#endregion
	}
}