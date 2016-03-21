using System;
using System.Linq;
using System.Text;
using CryCil.Engine.Models.Characters;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering.Nodes;

namespace CryCil.Engine
{
	/// <summary>
	/// Encapsulates information about a decal to spawn.
	/// </summary>
	public struct DecalInfo
	{
		#region Fields
		private CryRenderNode owner;
		private Vector3 position;
		private Vector3 normal;
		private float size;
		private float lifeTime;
		private float angle;
		private StaticObject staticObject;
		private Vector3 hitDirection;
		private float growTime, growTimeAlpha;
		private uint groupId;
		private bool skipOverlappingTest;
		private bool assemble;
		private bool forceEdge;
		private bool forceSingleOwner;
		private bool deferred;
		private byte sortPriority;
		private string materialName;
		private bool preventDecalOnGround;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the render node to place the decal on.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Probably not necessary when creating a decal on the character using
		/// <see cref="Character.CreateDecal"/>.
		/// </para>
		/// <para>
		/// If not specified when passed to <see cref="Decal.Create"/> then the decal will be cast into
		/// environment.
		/// </para>
		/// </remarks>
		public CryRenderNode Owner
		{
			get { return this.owner; }
			set { this.owner = value; }
		}
		/// <summary>
		/// Gets or sets the world-space position of the decal before casting.
		/// </summary>
		public Vector3 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}
		/// <summary>
		/// Gets or sets the normal to the plane the decal will be on before casting.
		/// </summary>
		public Vector3 Normal
		{
			get { return this.normal; }
			set { this.normal = value; }
		}
		/// <summary>
		/// Gets or sets the size of the decal.
		/// </summary>
		public float Size
		{
			get { return this.size; }
			set { this.size = value; }
		}
		/// <summary>
		/// Gets or sets the life-time of the decal in seconds.
		/// </summary>
		public float LifeTime
		{
			get { return this.lifeTime; }
			set { this.lifeTime = value; }
		}
		/// <summary>
		/// Gets or sets the angle of rotation of the decal around the <see cref="Normal"/>.
		/// </summary>
		public float Angle
		{
			get { return this.angle; }
			set { this.angle = value; }
		}
		/// <summary>
		/// Gets or sets the static object that represents the mesh of the decal.
		/// </summary>
		public StaticObject StaticObject
		{
			get { return this.staticObject; }
			set { this.staticObject = value; }
		}
		/// <summary>
		/// Gets or sets normalized vector that represents the casting direction to the decal.
		/// </summary>
		public Vector3 HitDirection
		{
			get { return this.hitDirection; }
			set { this.hitDirection = value; }
		}
		/// <summary>
		/// Gets or sets the time it takes for the size of the decal to reach full amount after spawn.
		/// </summary>
		/// <remarks>Can be used for blood pools.</remarks>
		public float GrowTime
		{
			get { return this.growTime; }
			set { this.growTime = value; }
		}
		/// <summary>
		/// Gets or sets the time it takes for the alpha component of the decal's texture to reach full
		/// force after spawn.
		/// </summary>
		/// <remarks>Can be used to fade in the texture (e.g. for blood pools).</remarks>
		public float GrowTimeAlpha
		{
			get { return this.growTimeAlpha; }
			set { this.growTimeAlpha = value; }
		}
		/// <summary>
		/// Gets or sets the identifier of the group the decal is supposed to be a part of.
		/// </summary>
		public uint GroupId
		{
			get { return this.groupId; }
			set { this.groupId = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the decal must be spawned even if there are too
		/// many decals in the same place.
		/// </summary>
		public bool SkipOverlappingTest
		{
			get { return this.skipOverlappingTest; }
			set { this.skipOverlappingTest = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the decal must be united with others, if there are
		/// multiple decals in the same place.
		/// </summary>
		public bool Assemble
		{
			get { return this.assemble; }
			set { this.assemble = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the position of the decal must be forced to the
		/// nearest edge of the owner mesh and project it accordingly.
		/// </summary>
		public bool ForceEdge
		{
			get { return this.forceEdge; }
			set { this.forceEdge = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether there can only be one owner for the decal.
		/// </summary>
		/// <remarks>
		/// Can be set to <c>true</c> to prevent the decal from being cast into environment, if it's too
		/// big.
		/// </remarks>
		public bool ForceSingleOwner
		{
			get { return this.forceSingleOwner; }
			set { this.forceSingleOwner = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether deferred rendering should be used for the
		/// decal(?).
		/// </summary>
		public bool Deferred
		{
			get { return this.deferred; }
			set { this.deferred = value; }
		}
		/// <summary>
		/// Gets or sets priority of sorting. Used when there are multiple decals on top of each other(?).
		/// </summary>
		public byte SortPriority
		{
			get { return this.sortPriority; }
			set { this.sortPriority = value; }
		}
		/// <summary>
		/// Gets or sets the name of the material to use for the decal. Must not be longer then 256 bytes.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// The name of the material cannot be longer then 256 bytes. Only thrown in debug build.
		/// </exception>
		public string MaterialName
		{
			get { return this.materialName; }
			set
			{
				if (Encoding.UTF8.GetByteCount(value) > 256)
				{
#if DEBUG
					throw new ArgumentException("The name of the material cannot be longer then 256 bytes.");
#else
					return;
#endif
				}

				this.materialName = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether the decal must not be placed on the ground.
		/// </summary>
		public bool PreventDecalOnGround
		{
			get { return this.preventDecalOnGround; }
			set { this.preventDecalOnGround = value; }
		}
		#endregion
	}
}