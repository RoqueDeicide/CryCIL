using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;

namespace CryEngine.Misc.TypeConverters
{
	public class ColorTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(Vector3))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(Vector3))
				return true;

			return base.CanConvertTo(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is Vector3)
				return new ColorSingle((Vector3)value);

			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(Vector3))
				return (Vector3)(ColorSingle)value;

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}