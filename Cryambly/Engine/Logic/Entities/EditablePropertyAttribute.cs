using System;
using CryCil.Annotations;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Marks fields and properties of the entity that can be edited in Sandbox editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class EditablePropertyAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the folder where the property resides.
		/// </summary>
		[NotNull]
		public string Folder { get; private set; }
		/// <summary>
		/// This number allows to override the default sorting.
		/// </summary>
		/// <example>
		/// Without specifying this value the properties Name and FilePath will be listed in the following
		/// order: <list type="bullet"><item>FilePath</item><item>Name</item></list> If you assign 0 to
		/// Name property and 1 to FilePath, they will be listed in the different order:
		/// <list type="bullet"><item>Name</item><item>FilePath</item></list>
		/// </example>
		public int SortHelper { get; private set; }
		/// <summary>
		/// Specifies which Ui element to use to edit the property in Sandbox editor.
		/// </summary>
		public EditablePropertyUiControl UiControl { get; private set; }
		/// <summary>
		/// Gets description of this property.
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// Gets minimal and maximal value of the property.
		/// </summary>
		public Vector2 Limits { get; private set; }
		/// <summary>
		/// Gets default value of this property.
		/// </summary>
		public object DefaultValue { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="sortHelper">  <see cref="SortHelper"/></param>
		/// <param name="folder">      <see cref="Folder"/></param>
		/// <param name="defaultValue"><see cref="DefaultValue"/></param>
		/// <param name="uiControl">   <see cref="UiControl"/></param>
		/// <param name="description"> <see cref="Description"/></param>
		/// <param name="min">         Minimal value of this property, if it is numeric.</param>
		/// <param name="max">         Maximal value of this property, if it is numeric.</param>
		/// <exception cref="Exception">
		/// Provided default value cannot be null, if UI control has to be chosen automatically.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="folder"/> is <see langword="null"/>.
		/// </exception>
		public EditablePropertyAttribute(int sortHelper = 0, [NotNull] string folder = "", object defaultValue = null,
										 EditablePropertyUiControl uiControl = EditablePropertyUiControl.Default,
										 string description = "", float min = 0,
										 float max = 100)
		{
			if (folder == null) throw new ArgumentNullException("folder");
			this.SortHelper = sortHelper;
			this.Folder = folder;
			if (uiControl == EditablePropertyUiControl.Default)
			{
				if (defaultValue == null)
				{
					throw new Exception("Cannot determine which UI control to use for the editable property," +
										" if provided default value object is null.");
				}
				Type propType = defaultValue.GetType();
				if (propType == typeof(bool))
				{
					uiControl = EditablePropertyUiControl.Boolean;
				}
				else if (propType == typeof(int))
				{
					uiControl = EditablePropertyUiControl.Integer;
				}
				else if (propType == typeof(float))
				{
					uiControl = EditablePropertyUiControl.Float;
				}
				else if (propType == typeof(Vector3))
				{
					uiControl = EditablePropertyUiControl.Vector;
				}
				else
				{
					uiControl = EditablePropertyUiControl.Text;
				}
			}
			this.UiControl = uiControl;
			this.Description = description;
			this.Limits = new Vector2(min, max);
			this.DefaultValue = defaultValue;
		}
	}
}