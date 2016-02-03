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
	public class GeometricEntity : MonoEntity
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
		/// Releases resources held by this entity.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether this entity was released from native code.
		/// </param>
		public override void Dispose(bool invokedFromNativeCode)
		{
		}
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
		/// <summary>
		/// Synchronizes the state of this entity with its representation in other place (e.g. a save game
		/// file) .
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronize(CrySync sync)
		{
		}
		/// <summary>
		/// Updates logical state of this entity.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
		}
		/// <summary>
		/// Updates logical state of this entity after most other stuff is updated.
		/// </summary>
		public override void PostUpdate()
		{
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