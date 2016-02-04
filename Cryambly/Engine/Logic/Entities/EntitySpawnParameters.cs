using System;
using CryCil.Annotations;
using CryCil.Geometry;

#pragma warning disable 169,649
namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates a set of parameters that are used when spawning the entity.
	/// </summary>
	public struct EntitySpawnParameters
	{
		#region Fields
		[UsedImplicitly] private EntityId id;
		[UsedImplicitly] private EntityId prevId;

		[UsedImplicitly] private EntityGUID guid;
		[UsedImplicitly] private EntityGUID prevGuid;

		// Class of entity.
		[UsedImplicitly] private string pClass;

		/// Entity archetype.
		[UsedImplicitly] private IntPtr pArchetype;

		[UsedImplicitly] private string layerName;

		// Reference to entity's xml node in level data
		[UsedImplicitly] private IntPtr entityNode;

		[UsedImplicitly] private string name;
		[UsedImplicitly] private EntityFlags flags;

		// Spawn lock.
		[UsedImplicitly] private bool ignoreLock;
		// Note: To support save games compatible with patched levels (patched levels might use more
		// EntityIDs and save game might conflict with dynamic ones).
		[UsedImplicitly] private bool staticEntityId;
		[UsedImplicitly] private bool createdThroughPool;
		[UsedImplicitly] private Vector3 position;
		[UsedImplicitly] private Quaternion rotation;
		[UsedImplicitly] private Vector3 scale;
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
		/// Gets or sets flags that describe the entity.
		/// </summary>
		public EntityFlags Flags
		{
			get { return this.flags; }
			set { this.flags = value; }
		}
		/// <summary>
		/// Indicates whether this entity was created from an entity pool.
		/// </summary>
		public bool CreatedThroughPool
		{
			get { return this.createdThroughPool; }
		}
		/// <summary>
		/// Gets or sets initial position of the entity in world space.
		/// </summary>
		public Vector3 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}
		/// <summary>
		/// Gets or sets initial orientation of the entity in world space.
		/// </summary>
		public Quaternion Rotation
		{
			get { return this.rotation; }
			set { this.rotation = value; }
		}
		/// <summary>
		/// Gets or sets initial scale of the entity.
		/// </summary>
		public Vector3 Scale
		{
			get { return this.scale; }
			set { this.scale = value; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new set of parameters that can be used to spawn an entity.
		/// </summary>
		/// <param name="className">Name of the class that will represent the new entity.</param>
		/// <param name="name">     The name of the new entity.</param>
		/// <param name="flags">    A set of flags to assign to the new entity.</param>
		/// <param name="id">       Id to assign to the new entity.</param>
		/// <param name="guid">     Guid to assign to the new entity.</param>
		public EntitySpawnParameters(string className, string name, EntityFlags flags,
									 EntityId id = new EntityId(), EntityGUID guid = new EntityGUID())
			: this()
		{
			this.pClass = className;
			this.id = id;
			this.guid = guid;
			this.name = name;
			this.flags = flags;
			this.rotation = Quaternion.Identity;
			this.scale = new Vector3(1);
		}
		/// <summary>
		/// Creates new set of parameters that can be used to spawn an entity.
		/// </summary>
		/// <param name="entityClass">Type that represents the new managed entity.</param>
		/// <param name="name">     The name of the new entity.</param>
		/// <param name="flags">    A set of flags to assign to the new entity.</param>
		/// <param name="id">       Id to assign to the new entity.</param>
		/// <param name="guid">     Guid to assign to the new entity.</param>
		/// <exception cref="ArgumentException">Given type is not a valid entity type.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="entityClass"/> is <see langword="null" />.</exception>
		public EntitySpawnParameters(Type entityClass, string name, EntityFlags flags,
									 EntityId id = new EntityId(), EntityGUID guid = new EntityGUID())
			: this()
		{
			if (entityClass == null)
			{
				throw new ArgumentNullException("entityClass");
			}
			if (!entityClass.Implements<MonoEntity>() || !entityClass.ContainsAttribute<EntityAttribute>())
			{
				throw new ArgumentException("Given type is not a valid entity type.");
			}

			// ReSharper disable once ExceptionNotDocumented
			EntityAttribute attribute = entityClass.GetAttribute<EntityAttribute>();
			this.pClass = attribute.Name ?? entityClass.Name;
			this.name = name;
			this.id = id;
			this.guid = guid;
			this.flags = flags;
			this.rotation = Quaternion.Identity;
			this.scale = new Vector3(1);
		}
		#endregion
	}
}
#pragma warning restore 169,649