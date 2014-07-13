using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Sandbox
{
	/// <summary>
	/// Defines a Sandbox extension.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class SandboxExtensionAttribute : Attribute
	{
		/// <summary>
		/// The name of the extension.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// An optional description of the extension's purpose.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The name of the author.
		/// </summary>
		public string AuthorName { get; set; }

		/// <summary>
		/// A method, such as a website or email address, to contact the extension developer.
		/// </summary>
		public string AuthorContact { get; set; }

		public SandboxExtensionAttribute(string name, string desc)
		{
			Name = name;
			Description = desc;
		}
	}
}