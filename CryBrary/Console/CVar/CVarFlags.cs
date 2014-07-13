using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[Flags]
	public enum CVarFlags
	{
		/// <summary>
		/// just to have one recognizable spot where the flags are located in the Register call
		/// </summary>
		None = 0x00000000,

		/// <summary>
		/// stays in the default state when cheats are disabled
		/// </summary>
		Cheat = 0x00000002,
		DumpToDisk = 0x00000100,

		/// <summary>
		/// can not be changed by the user
		/// </summary>
		ReadOnly = 0x00000800,
		RequireLevelReload = 0x00001000,
		RequireAppRestart = 0x00002000,

		/// <summary>
		/// shows warning that this var was not used in config file
		/// </summary>
		WarningNotUsed = 0x00004000,

		/// <summary>
		/// otherwise the const char * to the name will be stored without copying the memory
		/// </summary>
		CopyName = 0x00008000,

		/// <summary>
		/// Set when variable value modified.
		/// </summary>
		Modified = 0x00010000,

		/// <summary>
		/// Set when variable was present in config file.
		/// </summary>
		WasInConfig = 0x00020000,

		/// <summary>
		/// Allow bitfield setting syntax.
		/// </summary>
		Bitfield = 0x00040000,

		/// <summary>
		/// is visible and usable in restricted (normal user) console mode
		/// </summary>
		RestrictedMode = 0x00080000,

		/// <summary>
		/// Invisible to the user in console
		/// </summary>
		Invisible = 0x00100000,

		/// <summary>
		/// Always accept variable value and call on change callback even if variable value didnt change
		/// </summary>
		AlwaysOnChange = 0x00200000,

		/// <summary>
		/// Blocks the execution of console commands for one frame
		/// </summary>
		BlockFrame = 0x00400000,

		/// <summary>
		/// Set if it is a const cvar not to be set inside cfg-files
		/// </summary>
		ConstCVar = 0x00800000,

		/// <summary>
		/// This variable is critical to check in every hash, since it's extremely vulnerable
		/// </summary>
		CheatAlwaysCheck = 0x01000000,

		/// <summary>
		/// This variable is set as VF_CHEAT but doesn't have to be checked/hashed since it's
		/// harmless to workaround
		/// </summary>
		CheatNoCheck = 0x02000000,

		// These flags should never be set during cvar creation, and probably never set manually.
		InternalFlagsStart = 0x00000080,

		/// <summary>
		/// can be changed on client and when connecting the var not sent to the client (is set for
		/// all vars in Game/scripts/Network/cvars.txt)
		/// </summary>
		NotNetSyncedInternal = InternalFlagsStart,
		InternalFlagsEnd = NotNetSyncedInternal
	}
}