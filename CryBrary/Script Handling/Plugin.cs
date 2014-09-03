using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Generic;

namespace CryEngine.Initialization
{
	/// <summary>
	/// Represents a CryMono plugin.
	/// </summary>
	public interface ICryMonoPlugin
	{
		/// <summary>
		/// Called to obtain types for CryMono scripts, e.g. FlowNode, Actor and Entity.
		/// </summary>
		/// <param name="assemblies">
		/// All other assemblies found in the directory this compiler is located in.
		/// </param>
		/// <returns>Found types that should be scanned for CryMono scripts.</returns>
		IEnumerable<Type> GetTypes(IEnumerable<Assembly> assemblies);

		/// <summary>
		/// Called shortly after <see cref="GetTypes" /> to get registration parameters for certain
		/// types, see <see cref="FlowNodeRegistrationParams" /> and more.
		/// </summary>
		/// <param name="scriptType">The type of script we want parameters of.</param>
		/// <param name="type">The type we want parameters of.</param>
		/// <returns>The script registration parameters for this type.</returns>
		IScriptRegistrationParams GetRegistrationParams(ScriptType scriptType, Type type);
	}
}