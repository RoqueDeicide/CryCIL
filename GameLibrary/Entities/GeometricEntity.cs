using System;
using System.Linq;
using CryCil;
using CryCil.Engine.Data;
using CryCil.Engine.Logic;

namespace GameLibrary.Entities
{
	/// <summary>
	/// Represents an entity that is a simple geometric object.
	/// </summary>
	[Entity("A simple geometric object.", "", "", "prompt.bmp", EntityClassFlags.Default)]
	public class GeometricEntity : SimpleMonoEntity
	{
		#region Fields
		private string objectPath;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the path to the file to load geometry from.
		/// </summary>
		[EditableProperty(
			folder: "Geometry",
			defaultValue: "editor/objects/sphere.cgf",
			uiControl: EditablePropertyUiControl.Object,
			description: "Path to the file to load the geometry from.")]
		public string ObjectModel
		{
			get { return this.objectPath; }
			set
			{
				if (this.objectPath == value)
				{
					return;
				}

				if (value.IsNullOrWhiteSpace())
				{
					this.objectPath = "";
					this.Entity.Slots[0].Release();
					return;
				}

				this.objectPath = value;
				this.Entity.Slots[0].LoadStaticObject(this.objectPath);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes base properties of all objects that serve as abstraction layers between CryEngine
		/// entities and logic defined in CryCIL for them.
		/// </summary>
		/// <param name="handle">Pointer to the entity itself.</param>
		/// <param name="id">    Identifier of the entity.</param>
		public GeometricEntity(CryEntity handle, EntityId id)
			: base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Performs preliminary initialization of this object.
		/// </summary>
		public override void Initialize()
		{
			this.ResetInEditor += this.Reset;
		}
		/// <summary>
		/// Performs final initialization of this object.
		/// </summary>
		public override void PostInitialize()
		{
			this.Reset(null, false);
		}
		#endregion
		#region Utilities
		private void Reset(MonoEntity sender, bool enteringGameMode)
		{
			if (!this.ObjectModel.IsNullOrWhiteSpace())
			{
				this.Entity.Slots[0].Release();
				this.Entity.Slots[0].LoadStaticObject(this.objectPath);
			}
		}
		#endregion
	}
}