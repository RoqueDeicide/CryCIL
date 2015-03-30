using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Enumeration of flags that can be used as masks for checking modifier keys.
	/// </summary>
	[Flags]
	public enum ModifierMask
	{
		/// <summary>
		/// No modifier masking.
		/// </summary>
		None = 0,
		/// <summary>
		/// Mask for checking whether left control key has been toggled.
		/// </summary>
		LeftControl = 1,
		/// <summary>
		/// Mask for checking whether left shift key has been toggled.
		/// </summary>
		LeftShift = 2,
		/// <summary>
		/// Mask for checking whether left alt key has been toggled.
		/// </summary>
		LeftAlt = 4,
		/// <summary>
		/// Mask for checking whether left windows key has been toggled.
		/// </summary>
		LeftWindows = 8,
		/// <summary>
		/// Mask for checking whether right control key has been toggled.
		/// </summary>
		RightControl = 16,
		/// <summary>
		/// Mask for checking whether right shift key has been toggled.
		/// </summary>
		RightShift = 32,
		/// <summary>
		/// Mask for checking whether right alt key has been toggled.
		/// </summary>
		RightAlt = 64,
		/// <summary>
		/// Mask for checking whether right windows key has been toggled.
		/// </summary>
		RightWindows = 128,
		/// <summary>
		/// Mask for checking whether num lock key has been toggled.
		/// </summary>
		NumLock = 256,
		/// <summary>
		/// Mask for checking whether caps lock key has been toggled.
		/// </summary>
		CapsLock = 512,
		/// <summary>
		/// Mask for checking whether scroll lock key has been toggled.
		/// </summary>
		ScrollLock = 1024,

		/// <summary>
		/// Mask for checking whether any of control keys have been toggled.
		/// </summary>
		Control = (LeftControl | RightControl),
		/// <summary>
		/// Mask for checking whether any of shift keys have been toggled.
		/// </summary>
		Shift = (LeftShift | RightShift),
		/// <summary>
		/// Mask for checking whether any of alt keys have been toggled.
		/// </summary>
		Alt = (LeftAlt | RightAlt),
		/// <summary>
		/// Mask for checking whether any of Windows keys have been toggled.
		/// </summary>
		Windows = (LeftWindows | RightWindows),
		/// <summary>
		/// Mask for checking whether any of modifier keys have been toggled.
		/// </summary>
		Modifiers = (Control | Shift | Alt | Windows),
		/// <summary>
		/// Mask for checking whether any of lock keys have been toggled.
		/// </summary>
		LockKeys = (CapsLock | NumLock | ScrollLock)
	}
}
