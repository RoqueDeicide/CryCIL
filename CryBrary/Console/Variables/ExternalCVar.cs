namespace CryEngine.Console.Variables
{
	/// <summary>
	/// CVar created outside CryMono
	/// </summary>
	internal class ExternalCVar : CVar
	{
		public ExternalCVar() { }

		public ExternalCVar(string name)
		{
			this.Name = name;
		}

		public override string ValueString
		{
			get { return Native.ConsoleInterop.GetCVarString(this.Name); }
			set { Native.ConsoleInterop.SetCVarString(this.Name, value); }
		}

		public override float ValueFloat
		{
			get { return Native.ConsoleInterop.GetCVarFloat(this.Name); }
			set { Native.ConsoleInterop.SetCVarFloat(this.Name, value); }
		}

		public override int ValueInt32
		{
			get { return Native.ConsoleInterop.GetCVarInt(this.Name); }
			set { Native.ConsoleInterop.SetCVarInt(this.Name, value); }
		}
	}
}