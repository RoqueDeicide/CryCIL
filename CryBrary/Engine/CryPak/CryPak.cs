using System;
using System.IO;
using CryEngine.Console.Variables;
using CryEngine.Initialization;
using CryEngine.Native;

namespace CryEngine.Utilities
{
	/// <summary>
	/// Direct interface to the Crytek pack files management system
	/// </summary>
	public static class CryPak
	{
		public static void SetAlias(string name, string alias, bool bAdd)
		{
			CryPakInterop.SetAlias(name, alias, bAdd);
		}

		public static string GetAlias(string name, bool returnSame = true)
		{
			return CryPakInterop.GetAlias(name, returnSame);
		}

		[CLSCompliant(false)]
		public static string AdjustFileName(string source, PathResolutionRules rules)
		{
			return CryPakInterop.AdjustFileName(source, rules);
		}

		#region Properties
		/// <summary>
		/// Gets the root engine folder.
		/// </summary>
		/// <returns>The filepath to the root folder as a string, ex:</returns>
		/// <example>"C:\CryENGINE3"</example>
		private static string RootFolder { get { return Directory.GetCurrentDirectory(); } }

		/// <summary>
		/// Gets the folder containing binaries relevant to current build type (Bin32 /
		/// Bin64) .
		/// </summary>
		/// <returns>
		/// The filepath to the Binary folder as a string, ex: <example>"C:\CryENGINE3\Bin32"</example>
		/// </returns>
		public static string BinaryFolder { get { return Path.Combine(RootFolder, "Bin" + (IntPtr.Size * 8).ToString()); } }

		/// <summary>
		/// Gets the engine folder. CryENGINE uses this store engine content which is not specific to the
		/// game being developed, i.e. shaders and configuration files.
		/// </summary>
		/// <returns>
		/// The filepath to the Engine folder as a string, ex: <example>"C:\CryENGINE3\Engine"</example>
		/// </returns>
		public static string EngineFolder { get { return Path.Combine(RootFolder, "Engine"); } }

		/// <summary>
		/// Gets the current game folder. CryENGINE uses this as the root for all game-related content.
		/// </summary>
		/// <returns>
		/// The filepath to the Game folder as a string, ex: <example>"C:\CryENGINE3\Game"</example>
		/// </returns>
		public static string GameFolder
		{
			get
			{
				CVar cvar;
				if (CVar.TryGet("sys_game_folder", out cvar))
					return Path.Combine(RootFolder, cvar.ValueString);

				return Path.Combine(RootFolder, "Game");
			}
		}

		public static string UserFolder
		{
			get
			{
				return CryPakInterop.GetAlias("%USER%");
			}
		}

		/// <summary>
		/// Gets the folder in which the engine stores scripts. We use this as the root for our .cs files
		/// and .dll plugins.
		/// </summary>
		/// <returns>
		/// The filepath to the Scripts folder as a string, ex:
		/// <example>"C:\CryENGINE3\Game\Scripts"</example>
		/// </returns>
		public static string ScriptsFolder { get { return Path.Combine(GameFolder, "Scripts"); } }
		public static string CodeFolder
		{
			get { return Path.Combine(GameFolder, "Code"); }
		}
		#endregion
	}
}