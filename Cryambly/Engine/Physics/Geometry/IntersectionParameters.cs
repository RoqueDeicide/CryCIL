namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify how to calculate the intersection of 2 geometric objects.
	/// </summary>
	public unsafe struct IntersectionParameters
	{
		#region Fields
		int iUnprojectionMode; // 0-angular, 1-rotational
		Vector3 centerOfRotation; // for mode 1 only
		Vector3 axisOfRotation; // if left 0, will be set based on collision area normal
		float time_interval; // used to set unprojection limits
		float vrel_min;	// if local relative velocity in contact area is above this, unprojects along its derection; otherwise along area normal
		float maxSurfaceGapAngle;	// theshold for generating area contacts
		float minAxisDist; // disables rotational unprojection if contact point is closer than this to the axis
		Vector3 unprojectionPlaneNormal; // restrict linear unprojection to this plane
		Vector3 axisContactNormal; // a mild hint about possible contact normal
		float maxUnproj; // unprojections longer than this are discarded
		Vector3 ptOutsidePivot[2];	// discard contacts that are not facing outward wrt this point
		bool bSweepTest; // requests a linear sweep test along v*time_interval (v from geom_world_data)
		bool bKeepPrevContacts; // append results to existing contact buffer
		bool bStopAtFirstTri;	// stop after the first collision is detected
		bool bNoAreaContacts;	// don't try to detect contact areas
		bool bNoBorder;	// don't trace contact border
		int bExactBorder; // always tries to return a consequtive border (useful for boolean ops)
		int bNoIntersection; // don't find all intersection points (only applies to primitive-primitive collisions)
		int bBothConvex; // (output) both operands were convex
		int bThreadSafe; // set if it's known that no other thread will contend for the internal intersection data (only used in PrimitiveWorldIntersection now)
		int bThreadSafeMesh; // set if it's known that no other thread will try to modify the colliding geometry
		GeometryContact *pGlobalContacts; // pointer to thread's global contact buffer
		#endregion
		#region Properties
		
		#endregion
		#region Events
		
		#endregion
		#region Construction
		
		#endregion
		#region Interface
		
		#endregion
		#region Utilities
		
		#endregion
	}
}