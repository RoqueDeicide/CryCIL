using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Console.Variables;
using CryEngine.Entities;

namespace CryEngine
{
	/// <summary>
	/// Defines additional information used by the entity registration system.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EntityAttribute : Attribute
	{
		/// <summary>
		/// Initializes default instance of type <see cref="EntityAttribute"/>.
		/// </summary>
		public EntityAttribute()
		{
			Flags = EntityClassFlags.Default;
		}
		/// <summary>
		/// Gets or sets the Entity class name. Uses class name if not set.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the path to file that contains helper mesh displayed inside Sandbox.
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
		/// Gets or sets the path to file that contains helper graphic displayed inside Sandbox.
		/// </summary>
		public string Icon { get; set; }
	}
	/// <summary>
	/// Defines a property that is displayed and editable inside Sandbox.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class EditorPropertyAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the minimum value of the property.
		/// </summary>
		public float Min { get; set; }
		/// <summary>
		/// Gets or sets the maximum value of the property.
		/// </summary>
		public float Max { get; set; }
		/// <summary>
		/// Gets or sets the property type. Should be used for special types such as files.
		/// </summary>
		public EditorPropertyType Type { get; set; }
		/// <summary>
		/// Gets or sets editor property flags.
		/// </summary>
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
		/// <summary>
		/// Gets or sets default value of the property.
		/// </summary>
		public string DefaultValue { get; set; }
	}
	/// <summary>
	/// Applied to fields and properties to mark them as console variable wrappers.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class CVarAttribute : Attribute
	{
		/// <summary>
		/// Default value of the console variable.
		/// </summary>
		public object DefaultValue;
		/// <summary>
		/// Flags applied to the console variable.
		/// </summary>
		public CVarFlags Flags;
		/// <summary>
		/// Text associated with the console variable.
		/// </summary>
		public string Help;
		/// <summary>
		/// Name of the console variable.
		/// </summary>
		public string Name;
	}
	/// <summary>
	/// Applied to methods to associate them with the console command.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ConsoleCommandAttribute : Attribute
	{
		/// <summary>
		/// Name of the console command that invokes marked method.
		/// </summary>
		public string Name;
		/// <summary>
		/// Text associated with the console command.
		/// </summary>
		public string Comment;
		/// <summary>
		/// Flags applied to the console command.
		/// </summary>
		public CVarFlags Flags;
	}
	/// <summary>
	/// Attribute used for specifying extra functionality for custom game rules classes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class GameRulesAttribute : Attribute
	{
		/// <summary>
		/// Sets the game mode's name. Uses the class name if not set.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// If set to true, the game mode will be set as default.
		/// </summary>
		public bool Default { get; set; }
	}
	/// <summary>
	/// Applied to classes to mark them as actors.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class ActorAttribute : Attribute
	{
	}
}