using System;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates a set of parameters that are used when spawning the entity.
	/// </summary>
	public struct EntitySpawnParameters
	{
		#region Fields
		[UsedImplicitly]
		private EntityId id;
		[UsedImplicitly]
		private EntityId prevId;

		[UsedImplicitly]
		private EntityGUID guid;
		[UsedImplicitly]
		private EntityGUID prevGuid;

		// Class of entity.
		[UsedImplicitly]
		private string pClass;

		/// Entity archetype.
		[UsedImplicitly]
		private IntPtr pArchetype;

		[UsedImplicitly]
		private string layerName;

		// Reference to entity's xml node in level data
		[UsedImplicitly]
		private IntPtr entityNode;

		[UsedImplicitly]
		private string name;
		[UsedImplicitly]
		private EntityFlags flags;

		// Spawn lock.
		[UsedImplicitly]
		private bool ignoreLock;
		// Note: To support save games compatible with patched levels (patched levels might use more
		// EntityIDs and save game might conflict with dynamic ones).
		[UsedImplicitly]
		private bool staticEntityId;
		[UsedImplicitly]
		private bool createdThroughPool;
		[UsedImplicitly]
		private Vector3 position;
		[UsedImplicitly]
		private Quaternion rotation;
		[UsedImplicitly]
		private Vector3 scale;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of the entity that is currently representing it.
		/// </summary>
		public EntityId CurrentId
		{
			get { return this.id; }
		}
		/// <summary>
		/// Gets identifier of the entity that was previously representing it.
		/// </summary>
		/// <remarks>Used when reloading.</remarks>
		public EntityId PreviousId
		{
			get { return this.prevId; }
		}
		/// <summary>
		/// Gets identifier of the entity that is currently representing it.
		/// </summary>
		public EntityGUID CurrentGuid
		{
			get { return this.guid; }
		}
		/// <summary>
		/// Gets identifier of the entity that was previously representing it.
		/// </summary>
		/// <remarks>Used when reloading.</remarks>
		public EntityGUID PreviousGuid
		{
			get { return this.prevGuid; }
		}
		/// <summary>
		/// Gets the name of the entity class.
		/// </summary>
		public string ClassName
		{
			get { return this.pClass; }
		}
		/// <summary>
		/// Gets the name of the layer the entity resides in.
		/// </summary>
		public string LayerName
		{
			get { return this.layerName; }
		}
		/// <summary>
		/// Gets the name of the entity.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		/// <summary>
		/// Gets flags that describe the entity.
		/// </summary>
		public EntityFlags Flags
		{
			get { return this.flags; }
		}
		/// <summary>
		/// Indicates whether this entity was created from an entity pool.
		/// </summary>
		public bool CreatedThroughPool
		{
			get { return this.createdThroughPool; }
		}
		/// <summary>
		/// Gets initial position of the entity.
		/// </summary>
		public Vector3 Position
		{
			get { return this.position; }
		}
		/// <summary>
		/// Gets initial orientation of the entity.
		/// </summary>
		public Quaternion Rotation
		{
			get { return this.rotation; }
		}
		/// <summary>
		/// Gets initial scale of the entity.
		/// </summary>
		public Vector3 Scale
		{
			get { return this.scale; }
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates new set of parameters that can be used to spawn an entity.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="id">       </param>
		/// <param name="guid">     </param>
		/// <param name="name">     </param>
		/// <param name="flags">    </param>
		/// <param name="position"> </param>
		/// <param name="rotation"> </param>
		/// <param name="scale">    </param>
		public EntitySpawnParameters(string className, EntityId id, EntityGUID guid, string name, EntityFlags flags,
									 Vector3 position, Quaternion rotation, Vector3 scale)
			: this()
		{
			this.pClass = className;
			this.id = id;
			this.guid = guid;
			this.name = name;
			this.flags = flags;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}
		/// <summary>
		/// Creates new set of parameters that can be used to spawn an entity.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="name">     </param>
		/// <param name="flags">    </param>
		/// <param name="position"> </param>
		/// <param name="rotation"> </param>
		/// <param name="scale">    </param>
		public EntitySpawnParameters(string className, string name, EntityFlags flags, Vector3 position,
									 Quaternion rotation, Vector3 scale)
			: this()
		{
			this.pClass = className;
			this.name = name;
			this.flags = flags;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities

		#endregion
	}
}