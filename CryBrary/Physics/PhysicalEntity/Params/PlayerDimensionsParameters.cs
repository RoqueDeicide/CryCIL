using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.RunTime;
using CryEngine.Utilities;

namespace CryEngine
{
	public struct PlayerDimensionsParameters
	{
		internal static PlayerDimensionsParameters Create()
		{
			var dim = new PlayerDimensionsParameters();

			dim.type = 1;

			dim.dirUnproj = new Vector3(0, 0, 1);

			dim.sizeCollider = UnusedMarker.Vector3;
			dim.heightPivot = UnusedMarker.Float;
			dim.heightCollider = UnusedMarker.Float;
			dim.heightEye = UnusedMarker.Float;
			dim.heightHead = UnusedMarker.Float;
			dim.headRadius = UnusedMarker.Float;
			dim.bUseCapsule = UnusedMarker.Integer;

			return dim;
		}

		internal int type;

		public float heightPivot; // offset from central ground position that is considered entity center
		public float heightEye; // vertical offset of camera
		public Vector3 sizeCollider; // collision cylinder dimensions
		public float heightCollider;    // vertical offset of collision geometry center
		public float headRadius;    // radius of the 'head' geometry (used for camera offset)
		public float heightHead;    // center.z of the head geometry
		public Vector3 dirUnproj;    // unprojection direction to test in case the new position overlaps with the environment (can be 0 for 'auto')
		public float maxUnproj; // maximum allowed unprojection
		public int bUseCapsule; // switches between capsule and cylinder collider geometry
	}
}