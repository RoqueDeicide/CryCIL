using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class GameRulesInterop
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void RegisterGameMode(string gamemode);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void AddGameModeAlias(string gamemode, string alias);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void AddGameModeLevelLocation(string gamemode, string location);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetDefaultGameMode(string gamemode);
	}
}