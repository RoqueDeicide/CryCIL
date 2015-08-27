using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.MemoryMapping;
using CryCil.Utilities;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of ways to lock access to the rope points.
	/// </summary>
	public enum RopeAccessLockMode
	{
		/// <summary>
		/// Specifies to release the read-only lock that was established using <see cref="EngageLock"/>.
		/// </summary>
		ReleaseLock = -1,
		/// <summary>
		/// Specifies to only engage the read only lock only for getting the details about the rope.
		/// </summary>
		LocalLock = 0,
		/// <summary>
		/// Specifies to engage the read-only lock that can be released using <see cref="ReleaseLock"/>.
		/// </summary>
		/// <remarks>
		/// Use this value if you want to acquire an array of points of the rope, if you don't know the
		/// segment count.
		/// </remarks>
		EngageLock = 1
	}
	/// <summary>
	/// Encapsulates description of the object that is used to query the status of the physical entity that
	/// is a rope.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Acquiring the positions of all points that connect segments requires usage of 2 calls to
	/// <see cref="PhysicalEntity.GetStatus"/>, the first call must be done with simple object and after
	/// the call, another one will have to be made with the same object and that will actually retrieve
	/// positions of points.
	/// </para>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusRope
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int nSegments;
		[UsedImplicitly] private int nVtx;
		[UsedImplicitly] private Vector3[] pPoints;
		[UsedImplicitly] private Vector3[] pVelocities;
		[UsedImplicitly] private Vector3[] pVtx;
		[UsedImplicitly] private int nCollStat, nCollDyn;
		[UsedImplicitly] private int bTargetPoseActive;
		[UsedImplicitly] private float stiffnessAnim;
		[UsedImplicitly] private int bStrained;
		[UsedImplicitly] private StridedPointer pContactEnts;
		[UsedImplicitly] private float timeLastActive;
		[UsedImplicitly] private Vector3 posHost;
		[UsedImplicitly] private Quaternion qHost;
		[UsedImplicitly] private int @lock;
		[UsedImplicitly] private uint gcHandle0;
		[UsedImplicitly] private uint gcHandle1;
		[UsedImplicitly] private uint gcHandle2;
		#endregion
		#region Properties
		/// <summary>
		/// Gets an array of positions of points that connect segments of the rope together.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if this object was passed to <see cref="PhysicalEntity.GetStatus"/> less then 2
		/// times and it was created originally without valid segment count.
		/// </returns>
		/// <example>
		/// <code>
		/// // Here is how you can acquire an array of points.
		/// PhysicalEntity entity = GetEntity();
		/// 
		/// PhysicsStatusRope ropeStatus = new PhysicsStatusRope(0);
		/// 
		/// // First call will finish initialization of ropeStatus.
		/// if (entity.GetStatus(ref ropeStatus.Base) != 0)
		/// {
		///     // Now that our object is initialized we now can get the points.
		///     entity.GetStatus(ref ropeStatus.Base);
		/// 
		///     // Lets calculate the length of the rope.
		///     float length = 0;
		///     for (int i = 1; i &lt; ropeStatus.Points.Length; i++)
		///     {
		///         length += (ropeStatus.Points[i] - ropeStatus.Points[i - 1]).Length;
		///     }
		/// 
		///     // Print the length in the console.
		///     Console.WriteLine("Length of the rope = {0}", length);
		/// }
		/// else
		/// {
		///     throw new Exception("Given entity is not a rope!");
		/// }
		/// </code>
		/// </example>
		public Vector3[] Points
		{
			get { return this.pPoints; }
		}
		/// <summary>
		/// Gets the array of vertices that describe the shape of the rope that uses dynamic subdivision.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if this object was passed to <see cref="PhysicalEntity.GetStatus"/> less then 2
		/// times.
		/// </returns>
		/// <example>
		/// <code>
		/// // Here is how you can acquire an array of vertices.
		/// PhysicalEntity entity = GetEntity();
		/// 
		/// PhysicsStatusRope ropeStatus = new PhysicsStatusRope(0)
		/// {
		///     // We must engage the read-only lock to make sure our ropeStatus object doesn't get invalidated.
		///     Lock = RopeAccessLockMode.EngageLock
		/// };
		/// 
		/// // First call will finish initialization of ropeStatus.
		/// if (entity.GetStatus(ref ropeStatus.Base) != 0)
		/// {
		///     // We need to release the lock now, so the rope can actually be changed by the physics.
		///     ropeStatus.Lock = RopeAccessLockMode.ReleaseLock;
		/// 
		///     // Now that our object is initialized we now can get the vertices.
		///     entity.GetStatus(ref ropeStatus.Base);
		/// 
		///     // Lets calculate the length of the rope.
		///     float length = 0;
		///     for (int i = 1; i &lt; ropeStatus.Vertices.Length; i++)
		///     {
		///         length += (ropeStatus.Vertices[i] - ropeStatus.Vertices[i - 1]).Length;
		///     }
		/// 
		///     // Print the length in the console.
		///     Console.WriteLine("Length of the rope = {0}", length);
		/// }
		/// else
		/// {
		///     throw new Exception("Given entity is not a rope!");
		/// }
		/// </code>
		/// </example>
		public Vector3[] Vertices
		{
			get { return this.pVtx; }
		}
		/// <summary>
		/// Gets an array of velocities of points that connect segments of the rope together.
		/// </summary>
		/// <returns>
		/// <c>null</c>, if this object was passed to <see cref="PhysicalEntity.GetStatus"/> less then 2
		/// times and it was created originally without valid segment count.
		/// </returns>
		public Vector3[] Velocities
		{
			get { return this.pVelocities; }
		}
		/// <summary>
		/// Gets the number of static objects this rope is contacting with.
		/// </summary>
		public int StaticContactCount
		{
			get { return this.nCollStat; }
		}
		/// <summary>
		/// Gets the number of dynamic objects this rope is contacting with.
		/// </summary>
		public int DynamicContactCount
		{
			get { return this.nCollDyn; }
		}
		/// <summary>
		/// Gets the target pose enforcement mode.
		/// </summary>
		public RopeTargetPoseMode TargetPoseMode
		{
			get { return (RopeTargetPoseMode)this.bTargetPoseActive; }
		}
		/// <summary>
		/// Gets the shape preserving stiffness.
		/// </summary>
		public float AnimationStiffness
		{
			get { return this.stiffnessAnim; }
		}
		/// <summary>
		/// Gets the value that indicates whether the rope is strained, either along the line, or wrapped
		/// around the object.
		/// </summary>
		public bool Strained
		{
			get { return this.bStrained != 0; }
		}
		/// <summary>
		/// Gets the array of entities the rope has a contact with.
		/// </summary>
		public unsafe PhysicalEntity[] Contacts
		{
			get
			{
				PhysicalEntity* contactPtr = (PhysicalEntity*)this.pContactEnts.Pointer;
				if (contactPtr == null || this.nCollDyn == 0)
				{
					return null;
				}

				PhysicalEntity[] contacts = new PhysicalEntity[this.nCollDyn];

				fixed (PhysicalEntity* cPtr = contacts)
				{
					for (int i = 0; i < this.nCollDyn; i++)
					{
						cPtr[i] = contactPtr[i];
					}
				}

				return contacts;
			}
		}
		/// <summary>
		/// Gets the last time stamp when the entity was awake.
		/// </summary>
		public float LastAwakeTime
		{
			get { return this.timeLastActive; }
		}
		/// <summary>
		/// Gets the position of the first entity part this rope was attached to when this status was
		/// queried.
		/// </summary>
		public Vector3 AttachmentPointPosition
		{
			get { return this.posHost; }
		}
		/// <summary>
		/// Gets the orientation of the first entity part this rope was attached to when this status was
		/// queried.
		/// </summary>
		public Quaternion AttachmentPointOrientation
		{
			get { return this.qHost; }
		}
		/// <summary>
		/// Gets the value that is used to control the access lock on the rope when getting the array of
		/// vertices of the rope that has sub-division mode on.
		/// </summary>
		public RopeAccessLockMode Lock
		{
			get { return (RopeAccessLockMode)this.@lock; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="segmentCount">
		/// A number of segments. This parameter is required to not be equal to 0, if you want to acquire
		/// <see cref="Points"/> array on a rope or <see cref="Velocities"/> array without calling
		/// <see cref="PhysicalEntity.GetStatus"/> second time with this object.
		/// </param>
		public PhysicsStatusRope(int segmentCount)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Rope);

			this.nSegments = segmentCount;
			this.gcHandle0 = this.gcHandle1 = this.gcHandle2 = new Bytes4(-1).UnsignedInt;
		}
		#endregion
	}
}