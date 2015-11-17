﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace CryCil.RunTime
{
	internal static class AssemblyLookUp
	{
		[UnmanagedThunk("Used by IMonoAssemblyCollection implementation to try looking up loaded assemblies.")]
		[SuppressMessage("ReSharper", "ExceptionNotDocumented")]
		private static string LookUpAssembly(string shortName)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			// ReSharper disable LoopCanBeConvertedToQuery
			for (int i = 0; i < assemblies.Length; i++)
				// ReSharper restore LoopCanBeConvertedToQuery
			{
				if (assemblies[i].GetName().Name == shortName)
				{
					return assemblies[i].FullName;
				}
			}
			return null;
		}
	}
}