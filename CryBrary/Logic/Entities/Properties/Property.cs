using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Logic.Entities.Properties
{
	/// <summary>
	/// Base class for entity properties that can be changed through Editor.
	/// </summary>
	public  abstract class Property
	{
		/// <summary>
		/// Name of the property.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the property.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Default value of the property.
		/// </summary>
		public string DefaultValue { get; set; }
	}
}
