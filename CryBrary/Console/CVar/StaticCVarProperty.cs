using System.Reflection;

namespace CryEngine
{
	/// <summary>
	/// CVar created using CVarAttribute, targeting a property.
	/// </summary>
	internal class StaticCVarProperty : CVar
	{
		private readonly PropertyInfo property;

		public StaticCVarProperty(CVarAttribute attribute, PropertyInfo propertyInfo)
		{
			Name = attribute.Name;
			Flags = attribute.Flags;
			Help = attribute.Help;

			property.SetValue(null, attribute.DefaultValue, null);

			property = propertyInfo;
		}

		public override string String
		{
			get { return property.GetValue(null, null) as string; }
			set { property.SetValue(null, value, null); }
		}

		public override float FVal
		{
			get { return (float)property.GetValue(null, null); }
			set { property.SetValue(null, value, null); }
		}

		public override int IVal
		{
			get { return (int)property.GetValue(null, null); }
			set { property.SetValue(null, value, null); }
		}
	}
}