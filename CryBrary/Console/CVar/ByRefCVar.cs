using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// CVar pointing to a managed value by reference.
	/// </summary>
	internal class ByRefCVar : ExternalCVar
	{
		internal ByRefCVar(string name)
		{
			Name = name;
		}
	}
}