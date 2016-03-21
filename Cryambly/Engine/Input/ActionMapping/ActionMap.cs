using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Marks classes that define action maps.
	/// </summary>
	/// <remarks>
	/// The name of the class that is marked by this attribute is a name of the action map, unless
	/// <see cref="Name"/> property is not <c>null</c>. Internally names of action maps and actions are
	/// converted into lowercase, so make sure there are no case-insensitive conflicts.
	/// </remarks>
	/// <example>
	/// <code source="ActionMaps/Sample.cs"/>
	/// <para>Here are default CryEngine sample action maps:</para>
	/// <para>GameSDK:</para>
	/// <code source="ActionMaps/GameSDK/ActionMaps.cs"/>
	/// <para>GameZero:</para>
	/// <code source="ActionMaps/GameZero/ActionMaps.cs"/>
	/// </example>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ActionMapAttribute : Attribute
	{
		/// <summary>
		/// Gets custom name of this action map.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="name">
		/// An optional name for the action, must not contain characters that are not valid in Xml tags. A
		/// name of the event will be used as a name if this parameter is left at <c>null</c>.
		/// </param>
		public ActionMapAttribute(string name = null)
		{
			this.Name = name;
		}
	}
}