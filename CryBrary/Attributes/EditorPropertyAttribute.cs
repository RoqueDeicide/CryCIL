using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Defines a property that is displayed and editable inside Sandbox.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class EditorPropertyAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the minimum value
		/// </summary>
		public float Min { get; set; }

		/// <summary>
		/// Gets or sets the maximum value
		/// </summary>
		public float Max { get; set; }

		/// <summary>
		/// Gets or sets the property type. Should be used for special types such as files.
		/// </summary>
		public EditorPropertyType Type { get; set; }

		public int Flags { get; set; }

		/// <summary>
		/// Gets or sets the name of the property, if not set the entity class name will be used.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description to display when the user hovers over this property inside Sandbox.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Folder in which the entity property resides. If null, not contained in a folder.
		/// </summary>
		public string Folder { get; set; }

		public string DefaultValue { get; set; }
	}
}