using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine
{
	public struct AreaQueryResult
	{
		private IntPtr pArea;
		public Area Area
		{
			get
			{
				return Area.TryGet(pArea);
			}
		}

		float DistanceSquared { get; set; }
		Vector3 Position { get; set; }
		bool Inside { get; set; }
		bool Near { get; set; }
	}

	public class Area
	{
		#region Statics
		public static int AreaCount { get { return NativeEntityMethods.GetNumAreas(); } }

		public static Area GetArea(int areaId)
		{
			return TryGet(NativeEntityMethods.GetArea(areaId));
		}

		public static IEnumerable<AreaQueryResult> QueryAreas(EntityId id, Vector3 pos, int maxResults, bool forceCalculation)
		{
			var objAreas = NativeEntityMethods.QueryAreas(id, pos, maxResults, forceCalculation);

			return objAreas.Cast<AreaQueryResult>();
		}

		internal static Area TryGet(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var area = Areas.FirstOrDefault(x => x.Handle == ptr);
			if (area != null)
				return area;

			area = new Area(ptr);
			Areas.Add(area);

			return area;
		}

		private static List<Area> Areas = new List<Area>();
		#endregion

		private Area(IntPtr ptr)
		{
			Handle = ptr;
		}

		public EntityId GetEntityIdByIndex(int index)
		{
			return NativeEntityMethods.GetAreaEntityByIdx(Handle, index);
		}

		public BoundingBox BoundingBox
		{
			get
			{
				var bbox = new BoundingBox();

				NativeEntityMethods.GetAreaMinMax(Handle, ref bbox.Minimum, ref bbox.Maximum);

				return bbox;
			}
		}

		public int EntityCount { get { return NativeEntityMethods.GetAreaEntityAmount(Handle); } }

		public int Priority { get { return NativeEntityMethods.GetAreaPriority(Handle); } }

		/// <summary>
		/// IArea pointer
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}