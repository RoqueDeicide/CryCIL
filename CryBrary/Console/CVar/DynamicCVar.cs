using CryEngine.Native;

namespace CryEngine
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
			Flags = flags;
			Help = help;
			Name = name;

			if (value is int)
			{
				intValue = (int)value;

				Native.ConsoleInterop.RegisterCVarInt(Name, ref intValue, intValue, Flags, Help);
			}
			else if (value is float || value is double)
			{
				floatValue = (float)value;

				Native.ConsoleInterop.RegisterCVarFloat(Name, ref floatValue, floatValue, Flags, Help);
			}
			else if (value is string)
			{
				stringValue = value as string;

				// String CVars are not supported yet.
				Native.ConsoleInterop.RegisterCVarString(Name, stringValue, stringValue, Flags, Help);
			}
			else
				throw new CVarException(string.Format("Invalid data type ({0}) used in CVar {1}.", value.GetType(), Name));
		}

		public override string String
		{
			get { return stringValue; }
			set { stringValue = value; }
		}

		public override float FVal
		{
			get { return floatValue; }
			set { floatValue = value; }
		}

		public override int IVal
		{
			get { return intValue; }
			set { intValue = value; }
		}

		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var cvar = obj as CVar;
			if (cvar == null)
				return false;

			return Name.Equals(cvar.Name);
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + floatValue.GetHashCode();
				hash = hash * 29 + intValue.GetHashCode();
				hash = hash * 29 + stringValue.GetHashCode();
				hash = hash * 29 + Flags.GetHashCode();
				hash = hash * 29 + Name.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		#endregion
	}
}