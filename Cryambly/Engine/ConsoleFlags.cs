using System;

namespace CryCil.Engine
{
	/// <summary>
	/// A set of flags that can be set console commands and variables.
	/// </summary>
	[Flags]
	public enum ConsoleFlags
	{
		/// <summary>
		/// No flags are set at this value.
		/// </summary>
		Null = 0x00000000,
		/// <summary>
		/// When set for a console variable, indicates that the variable will not change its state when
		/// cheat mode is disabled.
		/// </summary>
		Cheat = 0x00000002,
		/// <summary>
		/// When set for a console variable, indicates that the variable will only get registered in a
		/// non-release build.
		/// </summary>
		DevelopmentBuildOnly = 0x00000004,
		/// <summary>
		/// When set for a console variable, indicates that the variable will only get registered in a
		/// non-release or dedicated server build.
		/// </summary>
		DedicatedServerOnly = 0x00000008,
		/// <summary>
		/// When set for a console variable, indicates that the variable's value stays the same on both
		/// server and clients with server taking precedence.
		/// </summary>
		NetworkSynchronized = 0x00000080,
		/// <summary>
		/// Unknown.
		/// </summary>
		DumpToDisk = 0x00000100,
		/// <summary>
		/// When set for a console variable, indicates that the variable's value can be set but can never
		/// be changed.
		/// </summary>
		ReadOnly = 0x00000800,
		/// <summary>
		/// When set for a console variable, indicates that changes to the variable's value will take
		/// effect after reloading the level.
		/// </summary>
		RequireLevelReload = 0x00001000,
		/// <summary>
		/// When set for a console variable, indicates that changes to the variable's value will take
		/// effect after reloading the game.
		/// </summary>
		RequireAppRestart = 0x00002000,
		/// <summary>
		/// When set for a console variable, indicates that a warning will be printed if the variable's
		/// value is not set in the configuration file.
		/// </summary>
		WarningNotUsed = 0x00004000,
		/// <summary>
		/// Setting this flag has no effect in C# code.
		/// </summary>
		CopyName = 0x00008000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value was modified since
		/// initialization.
		/// </summary>
		Modified = 0x00010000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value was initialized with a value
		/// from a configuration file.
		/// </summary>
		WasInConfig = 0x00020000,
		/// <summary>
		/// When set for an integer console variable, indicates that variable's value can be set using
		/// bit-field syntax.
		/// </summary>
		BitField = 0x00040000,
		/// <summary>
		/// Unknown.
		/// </summary>
		RestrictedMode = 0x00080000,
		/// <summary>
		/// When set, indicates that the variable or command is not visible to the normal user.
		/// </summary>
		Invisible = 0x00100000,
		/// <summary>
		/// When set for a console variable that was registered with a callback, indicates that the
		/// callback should be invoked even when the value of the variable doesn't change.
		/// </summary>
		AlwaysOnChange = 0x00200000,
		/// <summary>
		/// When set for a console command, indicates that its execution should block execution of other
		/// console commands till the next frame.
		/// </summary>
		BlockFrame = 0x00400000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value is always equal to one
		/// specified in the code during registration.
		/// </summary>
		ConstantConsoleVariable = 0x00800000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value is critical for the program,
		/// and extra checks should be used to make sure it doesn't change.
		/// </summary>
		CheatAlwaysCheck = 0x01000000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value is not critical for the
		/// program and there is no need for paranoia.
		/// </summary>
		CheatNoCheck = 0x02000000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value is set in system.cfg file and
		/// cannot be changed.
		/// </summary>
		SystemSpecOverwrite = 0x04000000,
		/// <summary>
		/// When set for a console variable that is in a console variable group, indicates that variable's
		/// value cannot be returned through the group object.
		/// </summary>
		ConsoleVariableGroupIgnoreInRealValue = 0x08000000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value will be synchronized with
		/// clients created using LiveCreate (don't ask what this means).
		/// </summary>
		LiveCreateSynced = 0x10000000,
		/// <summary>
		/// When set for a console variable, indicates that variable's new value will be accepted in render
		/// thread.
		/// </summary>
		RendererVariable = 0x20000000,
		/// <summary>
		/// When set for a console variable, indicates that variable's value cannot be changed outside of
		/// the code.
		/// </summary>
		Deprecated = 0x40000000
	}
}