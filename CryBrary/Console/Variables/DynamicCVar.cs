namespace CryEngine.Console.Variables
{
	/// <summary>
	/// CVar created at run-time
	/// </summary>
	internal class DynamicCVar : CVar
	{
		private float floatValue;
		private int intValue;
		private string stringValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicCVar"/> class. Used by
		/// CryConsole.RegisterCVar to construct the CVar.
		/// </summary>
		/// <param name="name"> </param>
		/// <param name="value"></param>
		/// <param name="flags"></param>
		/// <param name="help"> </param>
		internal DynamicCVar(string name, object value, CVarFlags flags, string help)
		{
			this.Flags = flags;
			this.Help = help;
			this.Name = name;

			if (value is int)
			{
				this.intValue = (int)value;

				Native.ConsoleInterop.RegisterCVarInt(this.Name, ref this.intValue, this.intValue, this.Flags, this.Help);
			}
			else if (value is float || value is double)
			{
				this.floatValue = (float)value;

				Native.ConsoleInterop.RegisterCVarFloat(this.Name, ref this.floatValue, this.floatValue, this.Flags, this.Help);
			}
			else if (value is string)
			{
				this.stringValue = value as string;

				// String CVars are not supported yet.
				Native.ConsoleInterop.RegisterCVarString(this.Name, this.stringValue, this.stringValue, this.Flags, this.Help);
			}
			else
				throw new ConsoleVariableException(string.Format("Invalid data type ({0}) used in CVar {1}.", value.GetType(), this.Name));
		}

		public override string String
		{
			get { return this.stringValue; }
			set { this.stringValue = value; }
		}

		public override float FVal
		{
			get { return this.floatValue; }
			set { this.floatValue = value; }
		}

		public override int IVal
		{
			get { return this.intValue; }
			set { this.intValue = value; }
		}

		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var cvar = obj as CVar;
			if (cvar == null)
				return false;

			return this.Name.Equals(cvar.Name);
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + this.floatValue.GetHashCode();
				hash = hash * 29 + this.intValue.GetHashCode();
				hash = hash * 29 + this.stringValue.GetHashCode();
				hash = hash * 29 + this.Flags.GetHashCode();
				hash = hash * 29 + this.Name.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		#endregion
	}
}