using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents an entity proxy that can be used to make the entity host an area trigger. Areas can be
	/// shapes, boxes or spheres. When entities enter or leave the area, area-related events are raised in
	/// <see cref="MonoEntity"/>.
	/// </summary>
	public unsafe struct CryEntityAreaProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets or sets the value that indicates whether this area needs to be updated when its points are
		/// set (whatever that means).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool UpdateArea
		{
			get
			{
				this.AssertInstance();

				return (GetFlags(this.handle) & AreaProxyFlags.NotUpdateArea) != 0;
			}
			set
			{
				this.AssertInstance();

				AreaProxyFlags flags = GetFlags(this.handle);
				if (value)
				{
					flags &= ~AreaProxyFlags.NotUpdateArea;
				}
				else
				{
					flags |= AreaProxyFlags.NotUpdateArea;
				}
				SetFlags(this.handle, flags);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this proxy needs to be synchronized when the
		/// entity is synchronized.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Synchronize
		{
			get
			{
				this.AssertInstance();

				return (GetFlags(this.handle) & AreaProxyFlags.NotSerialize) != 0;
			}
			set
			{
				this.AssertInstance();

				AreaProxyFlags flags = GetFlags(this.handle);
				if (value)
				{
					flags &= ~AreaProxyFlags.NotSerialize;
				}
				else
				{
					flags |= AreaProxyFlags.NotSerialize;
				}
				SetFlags(this.handle, flags);
			}
		}
		/// <summary>
		/// Gets the type of the area represented by this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityAreaType Type
		{
			get
			{
				this.AssertInstance();

				return GetAreaType(this.handle);
			}
		}
		/// <summary>
		/// Gets the array of points that form the shape of this area if this area is a
		/// <see cref="EntityAreaType.Shape"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		[CanBeNull]
		public Vector3[] ShapePoints
		{
			get
			{
				this.AssertInstance();

				int pointCount = GetPointsCount(this.handle);
				if (pointCount == 0)
				{
					return null;
				}

				Vector3[] vectors = new Vector3[pointCount];
				Vector3* points = GetPoints(this.handle);
				for (int i = 0; i < pointCount; i++)
				{
					vectors[i] = points[i];
				}

				return vectors;
			}
		}
		/// <summary>
		/// Gets the height of the shape that represents this area, if this area is a
		/// <see cref="EntityAreaType.Shape"/>. Returns 0, if this area is a shape, or itf its height is
		/// infinite.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ShapeHeight
		{
			get
			{
				this.AssertInstance();

				return GetHeight(this.handle);
			}
		}
		/// <summary>
		/// Gets the bounding box that represents this area, if this area is a
		/// <see cref="EntityAreaType.Box"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox Box
		{
			get
			{
				this.AssertInstance();

				BoundingBox box = new BoundingBox();
				GetBox(this.handle, out box.Minimum, out box.Maximum);
				return box;
			}
		}
		/// <summary>
		/// Gets the bounding sphere that represents this area, if this area is a
		/// <see cref="EntityAreaType.Sphere"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingSphere Sphere
		{
			get
			{
				this.AssertInstance();

				BoundingSphere sphere = new BoundingSphere();
				GetSphere(this.handle, out sphere.Center, out sphere.Radius);
				return sphere;
			}
		}
		/// <summary>
		/// Gets or sets the identifier of this area.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Id
		{
			get
			{
				this.AssertInstance();

				return GetID(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetID(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the group this area is in.
		/// </summary>
		/// <remarks>
		/// When the entity is in multiple area that are in the same group, the entity will only be
		/// considered inside one of them (one that is the closest to the entity).
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Group
		{
			get
			{
				this.AssertInstance();

				return GetGroup(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetGroup(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the priority of this area.
		/// </summary>
		/// <remarks>
		/// Areas within the group depend on areas with higher priority within that group.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Priority
		{
			get
			{
				this.AssertInstance();

				return GetPriority(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetPriority(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the size of the proximity region around the border of the area.
		/// </summary>
		/// <remarks>
		/// Proximity is used to determine fade factor value that is used in area-related events in
		/// <see cref="MonoEntity"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Proximity
		{
			get
			{
				this.AssertInstance();

				return GetProximity(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProximity(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the number of entities inside this area.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int EntityCount
		{
			get
			{
				this.AssertInstance();

				try
				{
					return GetNumberOfEntitiesInArea(this.handle);
				}
				catch (OverflowException)
				{
					return -1;
				}
			}
		}
		#endregion
		#region Construction
		internal CryEntityAreaProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Assigns a 2D shape to this area.
		/// </summary>
		/// <param name="points">          
		/// An array of points which XY coordinates are going to be used to form the shape and which lowest
		/// Z coordinate will be used as a plane. Coordinates are in entity-space.
		/// </param>
		/// <param name="soundObstructors">
		/// An array of boolean values that indicate whether corresponding segments obstruct sounds.
		/// </param>
		/// <param name="height">          
		/// Optional parameter that represents the height of the area. If not specified, the height is
		/// infinite.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">An array of points must have objects in it.</exception>
		/// <exception cref="ArgumentNullException">
		/// An array of sound obstruction indicators must have objects in it.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Length of points array must be equal to length of the array of sound obstruction indicators.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The array of points is either multi-dimensional, or has too many points in it.
		/// </exception>
		public void AssignShape([NotNull] Vector3[] points, [NotNull] bool[] soundObstructors, float height = 0)
		{
			this.AssertInstance();
			if (points.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(points), "An array of points must have objects in it.");
			}
			if (soundObstructors.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(soundObstructors),
												"An array of sound obstruction indicators must have objects in it.");
			}
			if (points.Length != soundObstructors.Length)
			{
				throw new ArgumentException(
					"Length of points array must be equal to length of the array of sound obstruction indicators.");
			}

			fixed (Vector3* vectors = points)
			fixed (bool* indicators = soundObstructors)
			{
				SetPoints(this.handle, vectors, indicators, points.Length, height);
			}
		}
		/// <summary>
		/// Assigns a 2D shape to this area.
		/// </summary>
		/// <param name="points">
		/// An array of points which XY coordinates are going to be used to form the shape and which lowest
		/// Z coordinate will be used as a plane. Coordinates are in entity-space.
		/// </param>
		/// <param name="height">
		/// Optional parameter that represents the height of the area in meters. If not specified, the
		/// height is infinite.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">An array of points must have objects in it.</exception>
		/// <exception cref="OverflowException">
		/// The array of points is either multi-dimensional, or has too many points in it.
		/// </exception>
		public void AssignShape([NotNull] Vector3[] points, float height = 0)
		{
			this.AssertInstance();
			if (points.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(points), "An array of points must have objects in it.");
			}

			fixed (Vector3* vectors = points)
			{
				SetPointsDefault(this.handle, vectors, points.Length, height);
			}
		}
		/// <summary>
		/// Assigns a box to this area.
		/// </summary>
		/// <param name="box">             
		/// A bounding box to assign to the area. Coordinates are in entity-space.
		/// </param>
		/// <param name="soundObstructors">
		/// An optional array of boolean values that indicate whether corresponding side of the box
		/// obstructs sound.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Too many objects in the list of sound obstructors. There cannot be more then 6.
		/// </exception>
		public void AssignBox(ref BoundingBox box, bool[] soundObstructors = null)
		{
			this.AssertInstance();

			if (soundObstructors.IsNullOrEmpty())
			{
				SetBox(this.handle, ref box.Minimum, ref box.Maximum, null, 0);
			}
			else if (soundObstructors.Length < 7)
			{
				fixed (bool* indicators = soundObstructors)
				{
					try
					{
						SetBox(this.handle, ref box.Minimum, ref box.Maximum, indicators, soundObstructors.Length);
					}
					catch (OverflowException)
					{
					}
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(soundObstructors),
													  "Too many objects in the list of sound obstructors. There cannot be more then 6.");
			}
		}
		/// <summary>
		/// Assigns a sphere to this area.
		/// </summary>
		/// <param name="center">Coordinates of the center of the sphere in local-space.</param>
		/// <param name="radius">Radius of the sphere in meters.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AssignSphere(ref Vector3 center, float radius)
		{
			this.AssertInstance();

			SetSphere(this.handle, ref center, radius);
		}
		/// <summary>
		/// Begins definition of the solid that will represent this area.
		/// </summary>
		/// <remarks>Call this method before calls to <see cref="AddToSolid"/>.</remarks>
		/// <param name="transformation">
		/// Reference to the matrix that represents entity-space transformation of the solid.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void BeginSolid(ref Matrix34 transformation)
		{
			this.AssertInstance();

			BeginSettingSolid(this.handle, ref transformation);
		}
		/// <summary>
		/// Begins definition of the solid that will represent this area.
		/// </summary>
		/// <remarks>Call this method before calls to <see cref="AddToSolid"/>.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void BeginSolid()
		{
			this.AssertInstance();

			BeginSettingSolid(this.handle, ref Matrix34.SecretIdentity);
		}
		/// <summary>
		/// Finishes definition of the solid that will represent this area.
		/// </summary>
		/// <remarks>Call this method after calls to <see cref="AddToSolid"/>.</remarks>
		public void FinishSolid()
		{
			this.AssertInstance();

			EndSettingSolid(this.handle);
		}
		/// <summary>
		/// Adds a polygon to the solid that will represent this area.
		/// </summary>
		/// <remarks>
		/// This method can only be called between calls to
		/// <see cref="o:CryCil.Engine.Logic.EntityProxies.CryEntityAreaProxy.BeginSolid"/> and
		/// <see cref="FinishSolid"/>.
		/// </remarks>
		/// <param name="points">        An array of at least 3 points that form the polygon.</param>
		/// <param name="obstructsSound">Indicates whether this polygon obstructs the sound.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// An array of points must have more then 2 objects in it.
		/// </exception>
		public void AddToSolid([NotNull] Vector3[] points, bool obstructsSound = false)
		{
			this.AssertInstance();
			if (points.IsNullOrTooSmall(3))
			{
				throw new ArgumentNullException(nameof(points), "An array of points must have more then 2 objects in it.");
			}

			fixed (Vector3* vectors = points)
			{
				try
				{
					AddConvexHullToSolid(this.handle, vectors, obstructsSound, points.Length);
				}
				catch (OverflowException)
				{
				}
			}
		}
		/// <summary>
		/// Assigns a volume that is a circle extruded along the Bezier curve to the area.
		/// </summary>
		/// <param name="points">              An array of points that form the curve.</param>
		/// <param name="radius">              
		/// Radius of the circle that is extruded along the curve.
		/// </param>
		/// <param name="dontDisableInvisible">Unknown.</param>
		/// <param name="falloff">             Fall off parameter.</param>
		/// <param name="damping">             Damping parameter.</param>
		/// <param name="gravity">             
		/// Magnitude of the gravity that is used to propel entities caught in this area.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// An array of points must have more then 1 object in it.
		/// </exception>
		public void AssignBezierVolume([NotNull] Vector3[] points, float radius, bool dontDisableInvisible,
									   float falloff, float damping, float gravity = 0)
		{
			this.AssertInstance();
			if (points.IsNullOrTooSmall(3))
			{
				throw new ArgumentNullException(nameof(points), "An array of points must have more then 1 object in it.");
			}

			fixed (Vector3* vectors = points)
			{
				try
				{
					SetGravityVolume(this.handle, vectors, points.Length, radius, gravity, dontDisableInvisible, falloff, damping);
				}
				catch (OverflowException)
				{
				}
			}
		}
		/// <summary>
		/// Changes whether an element of the area obstructs the sound.
		/// </summary>
		/// <remarks>
		/// Depending on the type of the entity the elements can be: segments for
		/// <see cref="EntityAreaType.Shape"/>, box sides for <see cref="EntityAreaType.Box"/>, polygons for
		/// <see cref="EntityAreaType.Solid"/>.
		/// </remarks>
		/// <param name="elementIndex">Zero-based index of the element to set the obstruction for.</param>
		/// <param name="obstructs">   
		/// Indicates whether the element needs to start obstructing the sound.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AssignSoundObstruction(uint elementIndex, bool obstructs)
		{
			this.AssertInstance();

			SetSoundObstructionOnAreaFace(this.handle, elementIndex, obstructs);
		}
		/// <summary>
		/// Adds an entity to the list of entities that will receive area-related events whenever any entity
		/// enters or leaves this area.
		/// </summary>
		/// <param name="entity">Identifier of the entity to add.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AddTarget(EntityId entity)
		{
			this.AssertInstance();

			AddEntity(this.handle, entity);
		}
		/// <summary>
		/// Adds an entity to the list of entities that receive area-related events whenever any entity
		/// enters or leaves this area.
		/// </summary>
		/// <param name="entity">Identifier of the entity to add.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AddTarget(EntityGUID entity)
		{
			this.AssertInstance();

			AddEntityGuid(this.handle, entity);
		}
		/// <summary>
		/// Clears the list of entities that receive area-related events whenever any entity enters or
		/// leaves this area.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ClearTargets()
		{
			this.AssertInstance();

			ClearEntities(this.handle);
		}
		/// <summary>
		/// Computes the distance to the closest point on the area's hull and caches the result.
		/// </summary>
		/// <param name="entity">
		/// Identifier of the entity to associated with cached result. When this entity moves, its cache of
		/// results is cleared.
		/// </param>
		/// <param name="point"> 
		/// Coordinates of the point the distance between which and area's hull is calculated.
		/// </param>
		/// <param name="onHull">Resultant point on the area's hull.</param>
		/// <returns>
		/// Squared distance between <paramref name="point"/> and <paramref name="onHull"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ComputeNearestSquaredDistance(EntityId entity, ref Vector3 point, out Vector3 onHull)
		{
			this.AssertInstance();

			return CalcPointNearDistSq(this.handle, entity, ref point, out onHull);
		}
		/// <summary>
		/// Computes the distance to the closest point on the area's hull.
		/// </summary>
		/// <param name="point"> 
		/// Coordinates of the point the distance between which and area's hull is calculated.
		/// </param>
		/// <param name="onHull">Resultant point on the area's hull.</param>
		/// <returns>
		/// Squared distance between <paramref name="point"/> and <paramref name="onHull"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ComputeNearestSquaredDistance(ref Vector3 point, out Vector3 onHull)
		{
			this.AssertInstance();

			return CalcPointNearDistSq(this.handle, new EntityId(), ref point, out onHull);
		}
		/// <summary>
		/// Computes the distance to the closest point on the area's hull and caches the result.
		/// </summary>
		/// <remarks>
		/// The only difference between this function and
		/// <see cref="o:CryCil.Engine.Logic.EntityProxies.CryEntityAreaProxy.ComputeNearestSquaredDistance"/>
		/// is that this one only takes the outer hull into account, which only makes the difference if this
		/// area is of types <see cref="EntityAreaType.Shape"/>.
		/// </remarks>
		/// <param name="entity">
		/// Identifier of the entity to associated with cached result. When this entity moves, its cache of
		/// results is cleared.
		/// </param>
		/// <param name="point"> 
		/// Coordinates of the point the distance between which and area's hull is calculated.
		/// </param>
		/// <param name="onHull">Resultant point on the area's hull.</param>
		/// <returns>
		/// Squared distance between <paramref name="point"/> and <paramref name="onHull"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ComputeSquaredDistanceToHull(EntityId entity, ref Vector3 point, out Vector3 onHull)
		{
			this.AssertInstance();

			return ClosestPointOnHullDistSq(this.handle, entity, ref point, out onHull);
		}
		/// <summary>
		/// Computes the distance to the closest point on the area's hull.
		/// </summary>
		/// <remarks>
		/// The only difference between this function and
		/// <see cref="o:CryCil.Engine.Logic.EntityProxies.CryEntityAreaProxy.ComputeNearestSquaredDistance"/>
		/// is that this one only takes the outer hull into account, which only makes the difference if this
		/// area is of types <see cref="EntityAreaType.Shape"/>.
		/// </remarks>
		/// <param name="point"> 
		/// Coordinates of the point the distance between which and area's hull is calculated.
		/// </param>
		/// <param name="onHull">Resultant point on the area's hull.</param>
		/// <returns>
		/// Squared distance between <paramref name="point"/> and <paramref name="onHull"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ComputeSquaredDistanceToHull(ref Vector3 point, out Vector3 onHull)
		{
			this.AssertInstance();

			return ClosestPointOnHullDistSq(this.handle, new EntityId(), ref point, out onHull);
		}
		/// <summary>
		/// Determines whether a point is within this area.
		/// </summary>
		/// <param name="entity">      
		/// Identifier of the entity to associated with cached result. When this entity moves, its cache of
		/// results is cleared.
		/// </param>
		/// <param name="point">       
		/// Coordinates of the point which location needs to be determined.
		/// </param>
		/// <param name="ignoreHeight">
		/// Indicates whether we need to do the comparison on XY plane only.
		/// </param>
		/// <returns>Indication whether the point is within the area's hull.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsPointWithin(EntityId entity, ref Vector3 point, bool ignoreHeight = false)
		{
			this.AssertInstance();

			return CalcPointWithin(this.handle, entity, ref point, ignoreHeight);
		}
		/// <summary>
		/// Determines whether a point is within this area.
		/// </summary>
		/// <param name="point">       
		/// Coordinates of the point which location needs to be determined.
		/// </param>
		/// <param name="ignoreHeight">
		/// Indicates whether we need to do the comparison on XY plane only.
		/// </param>
		/// <returns>Indication whether the point is within the area's hull.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsPointWithin(ref Vector3 point, bool ignoreHeight = false)
		{
			this.AssertInstance();

			return CalcPointWithin(this.handle, new EntityId(), ref point, ignoreHeight);
		}
		/// <summary>
		/// Gets the entity inside this area.
		/// </summary>
		/// <param name="index">Zero-based index of the entity to get.</param>
		/// <returns>Identifier of the entity.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityId GetEntity(int index)
		{
			this.AssertInstance();

			return GetEntityInAreaByIdx(this.handle, index);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, AreaProxyFlags nAreaProxyFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AreaProxyFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityAreaType GetAreaType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPoints(IntPtr handle, Vector3* vPoints, bool* pabSoundObstructionSegments,
											 int nPointsCount, float fHeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPointsDefault(IntPtr handle, Vector3* vPoints, int nPointsCount,
													float fHeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBox(IntPtr handle, ref Vector3 min, ref Vector3 max,
										  bool* pabSoundObstructionSides, int nSideCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSphere(IntPtr handle, ref Vector3 vCenter, float fRadius);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BeginSettingSolid(IntPtr handle, ref Matrix34 worldTM);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddConvexHullToSolid(IntPtr handle, Vector3* verticesOfConvexHull,
														bool bObstruction, int numberOfVertices);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EndSettingSolid(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPointsCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3* GetPoints(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetHeight(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBox(IntPtr handle, out Vector3 min, out Vector3 max);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSphere(IntPtr handle, out Vector3 vCenter, out float fRadius);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGravityVolume(IntPtr handle, Vector3* pPoints, int nNumPoints,
													float fRadius, float fGravity, bool bDontDisableInvisible,
													float fFalloff, float fDamping);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetID(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetID(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGroup(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGroup(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPriority(IntPtr handle, int nPriority);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPriority(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSoundObstructionOnAreaFace(IntPtr handle, uint nFaceIndex,
																 bool bObstructs);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddEntity(IntPtr handle, EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddEntityGuid(IntPtr handle, EntityGUID guid);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearEntities(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProximity(IntPtr handle, float fProximity);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetProximity(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float CalcPointNearDistSq(IntPtr handle, EntityId nEntityID, ref Vector3 Point3d,
														out Vector3 OnHull3d);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float ClosestPointOnHullDistSq(IntPtr handle, EntityId nEntityID,
															 ref Vector3 Point3d, out Vector3 OnHull3d);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CalcPointWithin(IntPtr handle, EntityId nEntityID, ref Vector3 Point3d,
												   bool bIgnoreHeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetNumberOfEntitiesInArea(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EntityId GetEntityInAreaByIdx(IntPtr handle, int index);
		#endregion
		#region Nested Types
		[Flags]
		private enum AreaProxyFlags
		{
			NotUpdateArea = 1 << 1, // When set points in the area will not be updated.
			NotSerialize = 1 << 2 // Areas with this flag will not be serialized
		}
		#endregion
	}
}