using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// CVar created outside CryMono
	/// </summary>
	internal class ExternalCVar : CVar
	{
		public ExternalCVar() { }

		public ExternalCVar(string name)
		{
			Name = name;
		}

		public override string String
		{
			get { return NativeCVarMethods.GetCVarString(Name); }
			set { NativeCVarMethods.SetCVarString(Name, value); }
		}

		public override float FVal
		{
			get { return NativeCVarMethods.GetCVarFloat(Name); }
			set { NativeCVarMethods.SetCVarFloat(Name, value); }
		}

		public override int IVal
		{
			get { return NativeCVarMethods.GetCVarInt(Name); }
			set { NativeCVarMethods.SetCVarInt(Name, value); }
		}
	}
}