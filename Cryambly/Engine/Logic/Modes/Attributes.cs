using System;
using System.Linq;
using CryCil.Annotations;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Marks that class that represents an object that defines a particular set of rules for the game.
	/// </summary>
	/// <remarks>
	/// It's highly recommended that the name of the type that is marked by this attribute ends with
	/// "GameRules" and starts with the name of the set as set in <see cref="Name"/>.
	/// </remarks>
	[BaseTypeRequired(typeof(GameRules))]
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class GameRulesAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the rule set.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="GameRulesAttribute"/> class.
		/// </summary>
		/// <param name="name">Name of the rule set.</param>
		public GameRulesAttribute(string name)
		{
			this.Name = name;
		}
	}
	/// <summary>
	/// Adds an alias to the game rule set that is represented by the class this attribute marks.
	/// </summary>
	[BaseTypeRequired(typeof(GameRules))]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class GameRulesAliasAttribute : Attribute
	{
		/// <summary>
		/// Gets the alias of the rule set.
		/// </summary>
		public string Alias { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="GameRulesAliasAttribute"/> class.
		/// </summary>
		/// <param name="alias">Alias of the rule set.</param>
		public GameRulesAliasAttribute(string alias)
		{
			this.Alias = alias;
		}
	}
	/// <summary>
	/// Registers a folder where that level system can look up levels that support the game rule set that is represented by the class this attribute marks.
	/// </summary>
	[BaseTypeRequired(typeof(GameRules))]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class GameRulesFileLocationAttribute : Attribute
	{
		/// <summary>
		/// Gets the path to levels that support the game rule set.
		/// </summary>
		public string Path { get; private set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="GameRulesAliasAttribute"/> class.
		/// </summary>
		/// <param name="path">Path to levels that support the game rule set.</param>
		public GameRulesFileLocationAttribute(string path)
		{
			this.Path = path;
		}
	}
}