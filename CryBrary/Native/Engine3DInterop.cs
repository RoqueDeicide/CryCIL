using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;

namespace CryEngine.Native
{
	internal static class Engine3DInterop
	{

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeOfDay(float hour, bool forceUpdate = false);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static float GetTimeOfDay();

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static TimeOfDay.AdvancedInfo GetTimeOfDayAdvancedInfo();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeOfDayAdvancedInfo(TimeOfDay.AdvancedInfo advancedInfo);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeOfDayVariableValue(int id, float value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeOfDayVariableValueColor(int id, Vector3 value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivatePortal(Vector3 pos, bool activate, string entityName);
	}
}