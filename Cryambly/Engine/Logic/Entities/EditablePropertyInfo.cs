﻿using System;
using System.Linq;
using System.Reflection;
using CryCil.RunTime.Registration;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates description of the editable entity property.
	/// </summary>
	public struct EditablePropertyInfo
	{
		#region Fields
		private readonly string name;
		private readonly EditablePropertyType type;
		private readonly string editType;
		private readonly string description;
		private readonly uint flags;
		private readonly Vector2 limits;
		private readonly string defaultValue;
		#endregion
		#region Properties
		/// <summary>
		/// Name of the property.
		/// </summary>
		public string Name => this.name;
		/// <summary>
		/// Type of the property.
		/// </summary>
		public EditablePropertyType Type => this.type;
		/// <summary>
		/// Prefix that designates which edit control to use.
		/// </summary>
		public string EditType => this.editType;
		/// <summary>
		/// Description of the property.
		/// </summary>
		public string Description => this.description;
		/// <summary>
		/// A set of flags that describes the property.
		/// </summary>
		/// <remarks>Not used.</remarks>
		public uint Flags => this.flags;
		/// <summary>
		/// 2D vector where <see cref="Vector2.X"/> field designates minimal value this property can take
		/// and <see cref="Vector2.Y"/> field designates maximal one.
		/// </summary>
		public Vector2 Limits => this.limits;
		/// <summary>
		/// Text representation of the default value of this property.
		/// </summary>
		public string DefaultValue => this.defaultValue;
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="property">An object that represents the editable property.</param>
		/// <exception cref="Exception">
		/// Invalid member was passed to EditablePropertyInfo constructor.
		/// </exception>
		/// <exception cref="Exception">
		/// Ui control specified for the property cannot be used for it.
		/// </exception>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		public EditablePropertyInfo(EditableProperty property)
		{
			MemberInfo member = property.Member;
			Type memberType = member.GetAssociatedType();
			EditablePropertyAttribute attribute = member.GetAttribute<EditablePropertyAttribute>();
			if (attribute == null)
			{
				throw new Exception("Invalid member was passed to EditablePropertyInfo constructor.");
			}
			this.name = member.Name;
			this.type = property.Type;

			var typeDesc = EntityRegistry.EditablePropertyUiMap[(int)attribute.UiControl];
			// Gotta validate that type.
			if (typeDesc.ManagedType != memberType)
			{
				string error =
					$"Ui control specified for the property {member.Name} of type {memberType.FullName} cannot be used: it works only " +
					$"with properties of type {typeDesc.ManagedType.FullName}";
				throw new Exception(error);
			}
			this.editType = typeDesc.Prefix;

			this.description = attribute.Description;
			this.flags = 0;
			this.limits = attribute.Limits;

			if (attribute.DefaultValue == null)
			{
				this.defaultValue = string.Empty;
			}
			else if (attribute.DefaultValue.GetType() != typeDesc.ManagedType)
			{
				string error =
					$"Default value for the property {member.Name} of type {memberType.FullName} is of incompatible type {attribute.DefaultValue.GetType().FullName}.";
				throw new Exception(error);
			}
			else
			{
				this.defaultValue = attribute.DefaultValue.ToString();
			}
		}
		/// <summary>
		/// Creates new property info for a folder.
		/// </summary>
		/// <param name="folderName">Name of the folder.</param>
		/// <param name="start">     Indicates whether this is a beginning of the folder.</param>
		public EditablePropertyInfo(string folderName, bool start)
			: this()
		{
			this.name = folderName;
			this.type = start ? EditablePropertyType.FolderBegin : EditablePropertyType.FolderEnd;
		}
		#endregion
	}
}