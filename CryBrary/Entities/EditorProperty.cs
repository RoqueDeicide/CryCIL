using System;
using CryEngine.Mathematics;

namespace CryEngine.Entities
{
	/// <summary>
	/// Encapsulates details about the property of the entity that can be edited from
	/// Sandbox editor.
	/// </summary>
	public struct EditorProperty
	{
		private static readonly string[] typePrefixes =
		{
			"b",
			"i",
			"f",
			"vector",
			"",
			"",
			"",
			"",
			"object",
			"texture",
			"file",
			"sound",
			"dialog",
			"color",
			"_seq"
		};
		#region Fields
		/// <summary>
		/// Name of the property.
		/// </summary>
		public string Name;
		/// <summary>
		/// Description of the property.
		/// </summary>
		public string Description;
		/// <summary>
		/// Prefix for the name of the property that indicates its type..
		/// </summary>
		public string EditorTypePrefix;
		/// <summary>
		/// Default value of the property.
		/// </summary>
		public string DefaultValue;

		private EditorPropertyType type;
		/// <summary>
		/// Flags assigned to the property.
		/// </summary>
		public int Flags;
		/// <summary>
		/// Minimal and maximal values of this property.
		/// </summary>
		public EditorPropertyLimits Limits;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the type of this property.
		/// </summary>
		public EditorPropertyType Type
		{
			get
			{
				return this.type;
			}

			set
			{
				this.type = value;
				int valueId = (int)value;

				this.EditorTypePrefix = typePrefixes[valueId];
				if (valueId > 3 && value == EditorPropertyType.Color)
				{
					// Everything that is a some form of text is assigned String as a
					// type.
					this.type = EditorPropertyType.String;
				}
				else if (value == EditorPropertyType.Color)
				{
					// Colors are vectors.
					this.type = EditorPropertyType.Vector3;
				}
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of type <see cref="EditorProperty"/>.
		/// </summary>
		/// <param name="name">        Name of the property.</param>
		/// <param name="description"> Description of the property.</param>
		/// <param name="defaultValue">Default value of the property.</param>
		/// <param name="type">        Type of the property.</param>
		/// <param name="limits">      Limitations applied to the property.</param>
		/// <param name="flags">       Flags assigned to the property.</param>
		public EditorProperty(string name, string description, string defaultValue, EditorPropertyType type,
							  EditorPropertyLimits limits, int flags = 0)
			: this(name, description, defaultValue, type)
		{
			if (Math.Abs(limits.Max) < MathHelpers.ZeroTolerance &&
				Math.Abs(limits.Min) < MathHelpers.ZeroTolerance)
			{
				this.Limits.Max = Sandbox.UserInterfaceConstants.MaxSliderValue;
			}
			else
			{
				this.Limits.Max = limits.Max;
				this.Limits.Min = limits.Min;
			}

			this.Flags = flags;
		}
		/// <summary>
		/// Initializes new instance of type <see cref="EditorProperty"/>.
		/// </summary>
		/// <param name="name">        Name of the property.</param>
		/// <param name="description"> Description of the property.</param>
		/// <param name="defaultValue">Default value of the property.</param>
		/// <param name="type">        Type of the property.</param>
		public EditorProperty(string name, string description, string defaultValue, EditorPropertyType type)
			: this()
		{
			this.Name = name;
			this.Description = description;
			this.DefaultValue = defaultValue;

			this.Type = type;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates minimal and maximal values that can assigned to an editor property.
	/// </summary>
	public struct EditorPropertyLimits
	{
		/// <summary>
		/// The smallest value that can assigned to an editor property.
		/// </summary>
		public float Min;
		/// <summary>
		/// The biggest value that can assigned to an editor property.
		/// </summary>
		public float Max;
	}
	/// <summary>
	/// Defines the list of supported editor types.
	/// </summary>
	public enum EditorPropertyType
	{
		/// <summary>
		/// Boolean type.
		/// </summary>
		Bool,
		/// <summary>
		/// Integer number.
		/// </summary>
		Int,
		/// <summary>
		/// Floating point number.
		/// </summary>
		Float,
		/// <summary>
		/// Vector that denotes some kind of position in the world.
		/// </summary>
		Vector3,
		/// <summary>
		/// Plain text.
		/// </summary>
		String,
		/// <summary>
		/// Identifier of the entity.
		/// </summary>
		Entity,
		/// <summary>
		/// Name of the folder.
		/// </summary>
		FolderBegin,
		/// <summary>
		/// Name of the folder.
		/// </summary>
		FolderEnd,
		/// <summary>
		/// </summary>
		Object,
		/// <summary>
		/// Path to texture file.
		/// </summary>
		Texture,
		/// <summary>
		/// Path to the file.
		/// </summary>
		File,
		/// <summary>
		/// Path to sound file.
		/// </summary>
		Sound,
		/// <summary>
		/// Name of the dialog.
		/// </summary>
		Dialogue,
		/// <summary>
		/// Color value.
		/// </summary>
		Color,
		/// <summary>
		/// Name of the sequence.
		/// </summary>
		Sequence
	}
}