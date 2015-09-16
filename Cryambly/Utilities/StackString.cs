using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Utilities
{
	/// <summary>
	/// Represents CryEngine default stack-allocated string.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct StackString
	{
		#region Fields
		[UsedImplicitly] private int length;
		[UsedImplicitly] private int allocSize;
		[UsedImplicitly] private char* m_str;
		[UsedImplicitly] private fixed char m_strBuf [512];
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the text value of this string.
		/// </summary>
		public string Text
		{
			get
			{
				fixed (StackString* ptr = &this)
				{
					return GetString(ptr);
				}
			}
			set
			{
				fixed (StackString* ptr = &this)
				{
					AssignString(ptr, value);
				}
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AssignString(StackString* ptr, string str);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetString(StackString* ptr);
		#endregion
	}
}