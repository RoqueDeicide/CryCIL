using System.Reflection;

namespace CryEngine.Console.Variables
{
	/// <summary>
	/// CVar created using CVarAttribute, targeting a field.
	/// </summary>
	internal class StaticCVarField : CVar
	{
		private readonly FieldInfo field;

		public StaticCVarField(CVarAttribute attribute, FieldInfo fieldInfo)
		{
			this.Name = attribute.Name;
			this.Flags = attribute.Flags;
			this.Help = attribute.Help;

			fieldInfo.SetValue(null, attribute.DefaultValue);

			this.field = fieldInfo;
		}

		public override string ValueString
		{
			get { return this.field.GetValue(null) as string; }
			set { this.field.SetValue(null, value); }
		}

		public override float ValueFloat
		{
			get { return (float)this.field.GetValue(null); }
			set { this.field.SetValue(null, value); }
		}

		public override int ValueInt32
		{
			get { return (int)this.field.GetValue(null); }
			set { this.field.SetValue(null, value); }
		}
	}
}