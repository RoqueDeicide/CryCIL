using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryCil.Engine.Logic;

namespace CryCil.RunTime.Registration
{
	internal struct RmiTypeDesc
	{
		internal Type Type;
		internal ConstructorInfo DefaultConstructor;
		internal MethodInfo GetReceptorMethod;
	}
	/// <summary>
	/// Defines functions that register types that handle transfer of arguments for RMI calls.
	/// </summary>
	[InitializationClass]
	public static class RmiParametersRegistry
	{
		#region Fields
		private static SortedList<string, RmiTypeDesc> rmiParamTypes;
		#endregion
		#region Interface
		[InitializationStage((int)DefaultInitializationStages.RmiRegistrationStage)]
		private static void RegisterRmiParamTypes(int index)
		{
			var linq = from assembly in MonoInterface.CryCilAssemblies
					   from type in assembly.GetTypes()
					   let valid = type.Implements<RmiParameters>()
					   let defCtor = type.GetConstructor(BindingFlags.Public, null, Type.EmptyTypes, null)
					   let hasDefaultCtor = defCtor != null
					   let acquireProperty = type.GetProperty("Receptor", BindingFlags.Static, null, typeof(RmiParameters),
															  Type.EmptyTypes, null)
					   let hasAcquireProperty = acquireProperty != null
					   let acquireMethod = type.GetMethod("GetReceptor", BindingFlags.Static, null, Type.EmptyTypes, null)
					   let hasAcquireMethod = acquireMethod != null
					   where valid && hasDefaultCtor || hasAcquireMethod || hasAcquireProperty
					   select new RmiTypeDesc
					   {
						   Type = type,
						   DefaultConstructor = defCtor,
						   GetReceptorMethod = acquireMethod ?? acquireProperty.GetGetMethod()
					   };

			rmiParamTypes = new SortedList<string, RmiTypeDesc>(linq.ToDictionary(desc => desc.Type.FullName,
																				  desc => desc));
		}
		/// <summary>
		/// Creates an object of type that derives from <see cref="RmiParameters"/>.
		/// </summary>
		/// <param name="name">Name of the type an object of which to get.</param>
		/// <returns>
		/// An object that is capable of receiving RMI data from remote machine, or <c>null</c>, if the type
		/// with given name wasn't found.
		/// </returns>
		public static RmiParameters AcquireReceptor(string name)
		{
			try
			{
				RmiTypeDesc desc;
				if (rmiParamTypes.TryGetValue(name, out desc))
				{
					if (desc.DefaultConstructor != null)
					{
						return desc.DefaultConstructor.Invoke(null) as RmiParameters;
					}
					return desc.GetReceptorMethod.Invoke(null, null) as RmiParameters;
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
			return null;
		}
		#endregion
		#region Utilities
		#endregion
	}
}