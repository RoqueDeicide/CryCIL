using CryEngine.Native;

namespace CryEngine
{
	public static class Terrain
	{
		/// <summary>
		/// Gets the size of the terrain in meters.
		/// </summary>
		public static int Size { get { return Native.Engine3DInterop.GetTerrainSize(); } }
		/// <summary>
		/// Gets the size of each terrain unit.
		/// </summary>
		public static int UnitsPerMetre { get { return Native.Engine3DInterop.GetTerrainUnitSize(); } }
		/// <summary>
		/// Gets the size of the terrain in units.
		/// </summary>
		/// <remarks>
		/// The terrain system calculates the overall size by multiplying this value by
		/// the units per meter setting. A map set to 1024 units at 2 meters per unit will
		/// have a size of 2048 meters.
		/// </remarks>
		public static int UnitSize { get { return Size / UnitsPerMetre; } }

		public static float GetTerrainElevation(int x, int y)
		{
			return Native.Engine3DInterop.GetTerrainZ(x, y);
		}

		public static float GetTerrainElevation(float x, float y)
		{
			return Native.Engine3DInterop.GetTerrainElevation(x, y);
		}
	}
}