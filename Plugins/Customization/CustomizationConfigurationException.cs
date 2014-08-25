using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.CharacterCustomization
{
	public class CustomizationConfigurationException : Exception
	{
		public CustomizationConfigurationException()
		{
		}

		public CustomizationConfigurationException(string message)
			: base(message)
		{
		}

		public CustomizationConfigurationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}