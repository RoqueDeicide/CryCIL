using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine.Entities
{
	/// <summary>
	/// Encapsulates result of area query which is used to see which areas are near the entity.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct AreaQueryResult
	{
		/// <summary>
		/// Pointer to the IArea object in native memory.
		/// </summary>
		public IntPtr AreaHandle;
		/// <summary>
		/// Squared distance from the area to the entity.
		/// </summary>
		public float DistanceSquared;
		/// <summary>
		/// Position of the entity on the outer hull of the area.
		/// </summary>
		public Vector3 PositionOnHull;
		/// <summary>
		/// Indicates whether the entity is inside the area.
		/// </summary>
		public bool Inside;
		/// <summary>
		/// Indicates whether the entity is close to the area.
		/// </summary>
		public bool Near;
		/// <summary>
		/// Gets <see cref="Area" /> wrapper object.
		/// </summary>
		public Area Area
		{
			get
			{
				return Area.TryGet(this.AreaHandle);
			}
		}
	}
	/// <summary>
	/// Represents a wrapper object for IArea.
	/// </summary>
	public class Area
	{
		#region Statics
		private static readonly List<Area> areas = new List<Area>();
		/// <summary>
		/// Gets number areas registered by the engine.
		/// </summary>
		public static int AreaCount { get { return EntityInterop.GetNumAreas(); } }
		/// <summary>
		/// Gets wrapper for IArea object that is identified by given number.
		/// </summary>
		/// <param name="areaId"> <see cref="Int32" /> number that identifies a requested area. </param>
		/// <returns> Wrapper for IArea object that is identified by given number. </returns>
		public static Area GetArea(int areaId)
		{
			return TryGet(EntityInterop.GetArea(areaId));
		}
		/// <summary>
		/// </summary>
		/// <param name="id">               </param>
		/// <param name="pos">              </param>
		/// <param name="maxResults">       </param>
		/// <param name="forceCalculation"> </param>
		/// <returns> </returns>
		public static IEnumerable<AreaQueryResult> QueryAreas(EntityId id, Vector3 pos, int maxResults, bool forceCalculation)
		{
			var objAreas = EntityInterop.QueryAreas(id, pos, maxResults, forceCalculation);

			return objAreas.Cast<AreaQueryResult>();
		}

		internal static Area TryGet(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var area = areas.FirstOrDefault(x => x.Handle == ptr);
			if (area != null)
				return area;

			area = new Area(ptr);
			areas.Add(area);

			return area;
		}
		#endregion

		private Area(IntPtr ptr)
		{
			this.Handle = ptr;
		}

		public EntityId GetEntityIdByIndex(int index)
		{
			return EntityInterop.GetAreaEntityByIdx(this.Handle, index);
		}

		public BoundingBox BoundingBox
		{
			get
			{
				var bbox = new BoundingBox();

				EntityInterop.GetAreaMinMax(this.Handle, ref bbox.Minimum, ref bbox.Maximum);

				return bbox;
			}
		}

		public int EntityCount { get { return EntityInterop.GetAreaEntityAmount(this.Handle); } }

		public int Priority { get { return EntityInterop.GetAreaPriority(this.Handle); } }

		/// <summary>
		/// IArea pointer
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}