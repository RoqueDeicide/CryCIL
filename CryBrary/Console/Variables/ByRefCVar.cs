namespace CryEngine.Console.Variables
{
	/// <summary>
	/// CVar pointing to a managed value by reference.
	/// </summary>
	internal class ByRefCVar : ExternalCVar
	{
		internal ByRefCVar(string name)
		{
			this.Name = name;
		}
	}
}