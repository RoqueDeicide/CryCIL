using System.Reflection;

namespace CryEngine
{
	/// <summary>
	/// CVar created using CVarAttribute, targeting a field.
	/// </summary>
	internal class StaticCVarField : CVar
	{
		private readonly FieldInfo field;

		public StaticCVarField(CVarAttribute attribute, FieldInfo fieldInfo)
		{
			Name = attribute.Name;
			Flags = attribute.Flags;
			Help = attribute.Help;

			fieldInfo.SetValue(null, attribute.DefaultValue);

			field = fieldInfo;
		}

		public override string String
		{
			get { return field.GetValue(null) as string; }
			set { field.SetValue(null, value); }
		}

		public override float FVal
		{
			get { return (float)field.GetValue(null); }
			set { field.SetValue(null, value); }
		}

		public override int IVal
		{
			get { return (int)field.GetValue(null); }
			set { field.SetValue(null, value); }
		}
	}
}