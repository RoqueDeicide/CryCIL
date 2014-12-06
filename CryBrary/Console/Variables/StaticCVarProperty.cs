using System.Reflection;

namespace CryEngine.Console.Variables
{
	/// <summary>
	/// CVar created using CVarAttribute, targeting a property.
	/// </summary>
	internal class StaticCVarProperty : CVar
	{
		private readonly PropertyInfo property;

		public StaticCVarProperty(CVarAttribute attribute, PropertyInfo propertyInfo)
		{
			this.Name = attribute.Name;
			this.Flags = attribute.Flags;
			this.Help = attribute.Help;

			this.property.SetValue(null, attribute.DefaultValue, null);

			this.property = propertyInfo;
		}

		public override string ValueString
		{
			get { return this.property.GetValue(null, null) as string; }
			set { this.property.SetValue(null, value, null); }
		}

		public override float ValueFloat
		{
			get { return (float)this.property.GetValue(null, null); }
			set { this.property.SetValue(null, value, null); }
		}

		public override int ValueInt32
		{
			get { return (int)this.property.GetValue(null, null); }
			set { this.property.SetValue(null, value, null); }
		}
	}
}