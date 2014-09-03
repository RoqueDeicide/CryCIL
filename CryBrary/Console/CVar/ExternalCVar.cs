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
			get { return Native.ConsoleInterop.GetCVarString(Name); }
			set { Native.ConsoleInterop.SetCVarString(Name, value); }
		}

		public override float FVal
		{
			get { return Native.ConsoleInterop.GetCVarFloat(Name); }
			set { Native.ConsoleInterop.SetCVarFloat(Name, value); }
		}

		public override int IVal
		{
			get { return Native.ConsoleInterop.GetCVarInt(Name); }
			set { Native.ConsoleInterop.SetCVarInt(Name, value); }
		}
	}
}