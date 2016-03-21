using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Marks classes that define logic for entities that are defined in CryCIL.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class EntityAttribute : Attribute
	{
		#region Properties
		/// <summary>
		/// Indicates whether information that is supplied through attributes of this type that mark base
		/// classes of one this attribute marks should be used when defining this entity class along with
		/// information that is defined by this attribute.
		/// </summary>
		public bool Inherit { get; private set; }
		/// <summary>
		/// Indicates whether <see cref="Flags"/> that are defined in this attribute should be combined with
		/// flags that are defined in attributes that mark classes that are derived by one this attribute
		/// marks using OR.
		/// </summary>
		public bool CombineFlags { get; }
		/// <summary>
		/// Gets the name of the entity class.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets description of this entity class.
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// Gets name of the category this entity is in.
		/// </summary>
		public string Category { get; private set; }
		/// <summary>
		/// Gets path to the .cgf file that contains geometry that is used by the editor to visually
		/// identify entities of this class.
		/// </summary>
		public string EditorHelper { get; private set; }
		/// <summary>
		/// Gets path to the image file that is used by the editor to visually identify entities of this
		/// class.
		/// </summary>
		public string EditorIcon { get; private set; }
		/// <summary>
		/// Gets a set of flags that describe this entity class.
		/// </summary>
		public EntityClassFlags Flags { get; private set; }
		/// <summary>
		/// Can be set to <c>true</c> to instruct underlying object to not synchronize any editable
		/// properties of entities of this class.
		/// </summary>
		/// <remarks>
		/// Setting this property to <c>true</c> makes sense when a lot of editable properties of this
		/// entity class are numerical or vectors in which case allowing the underlying object to
		/// synchronize is going to be quite slow, since it synchronizes all of them using text format.
		/// </remarks>
		public bool DontSyncEditableProperties { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new attribute that provides full description of the entity class.
		/// </summary>
		/// <param name="description"> <see cref="Description"/></param>
		/// <param name="category">    <see cref="Category"/></param>
		/// <param name="editorHelper"><see cref="EditorHelper"/></param>
		/// <param name="editorIcon">  <see cref="EditorIcon"/></param>
		/// <param name="flags">       <see cref="Flags"/></param>
		public EntityAttribute(string description, string category, string editorHelper, string editorIcon,
							   EntityClassFlags flags)
		{
			this.Inherit = false;
			this.CombineFlags = false;
			this.Description = description;
			this.Category = category;
			this.EditorHelper = editorHelper;
			this.EditorIcon = editorIcon;
			this.Flags = flags;
		}
		/// <summary>
		/// Creates new attribute that provides partial description of the entity class.
		/// </summary>
		/// <remarks>
		/// Any blanks in information this attribute provides will be filled with information defined
		/// attributes that mark classes the one this attribute marks derives from.
		/// </remarks>
		/// <param name="combineFlags"><see cref="CombineFlags"/></param>
		/// <param name="description"> <see cref="Description"/></param>
		/// <param name="category">    <see cref="Category"/></param>
		/// <param name="editorHelper"><see cref="EditorHelper"/></param>
		/// <param name="editorIcon">  <see cref="EditorIcon"/></param>
		/// <param name="flags">       <see cref="Flags"/></param>
		public EntityAttribute(bool combineFlags, string description = null, string category = null,
							   string editorHelper = null, string editorIcon = null, EntityClassFlags flags = 0)
		{
			this.Inherit = true;
			this.CombineFlags = combineFlags;
			this.Description = description;
			this.Category = category;
			this.EditorHelper = editorHelper;
			this.EditorIcon = editorIcon;
			this.Flags = flags;
		}
		/// <summary>
		/// Creates new attribute that provides full description of the entity class.
		/// </summary>
		/// <param name="name">        Name of the entity class.</param>
		/// <param name="description"> <see cref="Description"/></param>
		/// <param name="category">    <see cref="Category"/></param>
		/// <param name="editorHelper"><see cref="EditorHelper"/></param>
		/// <param name="editorIcon">  <see cref="EditorIcon"/></param>
		/// <param name="flags">       <see cref="Flags"/></param>
		public EntityAttribute(string name, string description, string category, string editorHelper, string editorIcon,
							   EntityClassFlags flags)
		{
			this.Inherit = false;
			this.CombineFlags = false;
			this.Name = name;
			this.Description = description;
			this.Category = category;
			this.EditorHelper = editorHelper;
			this.EditorIcon = editorIcon;
			this.Flags = flags;
		}
		/// <summary>
		/// Creates new attribute that provides partial description of the entity class.
		/// </summary>
		/// <remarks>
		/// Any blanks in information this attribute provides will be filled with information defined
		/// attributes that mark classes the one this attribute marks derives from.
		/// </remarks>
		/// <param name="name">        Name of the entity class.</param>
		/// <param name="combineFlags"><see cref="CombineFlags"/></param>
		/// <param name="description"> <see cref="Description"/></param>
		/// <param name="category">    <see cref="Category"/></param>
		/// <param name="editorHelper"><see cref="EditorHelper"/></param>
		/// <param name="editorIcon">  <see cref="EditorIcon"/></param>
		/// <param name="flags">       <see cref="Flags"/></param>
		public EntityAttribute(string name, bool combineFlags, string description = null, string category = null,
							   string editorHelper = null, string editorIcon = null, EntityClassFlags flags = 0)
		{
			this.Inherit = true;
			this.CombineFlags = combineFlags;
			this.Name = name;
			this.Description = description;
			this.Category = category;
			this.EditorHelper = editorHelper;
			this.EditorIcon = editorIcon;
			this.Flags = flags;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Inherits information from attributes that mark types given one inherits from.
		/// </summary>
		/// <param name="type">Class this attribute marks.</param>
		/// <returns>A new object of this type with inherited information.</returns>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		public EntityAttribute InheritFrom(Type type)
		{
			var baseAttributes =
				from attribute in
					(from baseType in type.EnumerateInheritancChain() select baseType.GetAttribute<EntityAttribute>())
				where attribute != null
				select attribute;

			EntityAttribute result = new EntityAttribute(this.Description, this.Category, this.EditorHelper,
														 this.EditorIcon, this.Flags);

			foreach (EntityAttribute baseAttribute in baseAttributes)
			{
				if (result.Category == null)
				{
					result.Category = baseAttribute.Category;
				}
				if (result.Description == null)
				{
					result.Description = baseAttribute.Description;
				}
				if (result.EditorHelper == null)
				{
					result.EditorHelper = baseAttribute.EditorHelper;
				}
				if (result.EditorIcon == null)
				{
					result.EditorIcon = baseAttribute.EditorIcon;
				}
				if (result.Flags == 0 || this.CombineFlags)
				{
					if (result.CombineFlags)
					{
						result.Flags |= baseAttribute.Flags;
					}
					else
					{
						result.Flags = baseAttribute.Flags;
					}
				}
			}

			return result;
		}
		#endregion
	}
}