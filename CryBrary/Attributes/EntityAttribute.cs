using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Defines additional information used by the entity registration system.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EntityAttribute : Attribute
	{
		public EntityAttribute()
		{
			Flags = EntityClassFlags.Default;
		}

		/// <summary>
		/// Gets or sets the Entity class name. Uses class name if not set.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the helper mesh displayed inside Sandbox.
		/// </summary>
		public string EditorHelper { get; set; }

		/// <summary>
		/// Gets or sets the class flags for this entity.
		/// </summary>
		public EntityClassFlags Flags { get; set; }

		/// <summary>
		/// Gets or sets the category in which the entity will be placed.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// Gets or sets the helper graphic displayed inside Sandbox.
		/// </summary>
		public string Icon { get; set; }
	}
}