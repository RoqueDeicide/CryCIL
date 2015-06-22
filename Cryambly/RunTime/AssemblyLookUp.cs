using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.RunTime
{
	internal static class AssemblyLookUp
	{
		[UnmanagedThunk("Used by IMonoAssemblyCollection implementation to try looking up loaded assemblies.")]
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