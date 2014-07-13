using CryEngine.Native;

namespace CryEngine
{
	public static class Terrain
	{
		/// <summary>
		/// Gets the size of the terrain in metres.
		/// </summary>
		public static int Size { get { return Native3DEngineMethods.GetTerrainSize(); } }

		/// <summary>
		/// Gets the size of each terrain unit.
		/// </summary>
		public static int UnitsPerMetre { get { return Native3DEngineMethods.GetTerrainUnitSize(); } }

		/// <summary>
		/// Gets the size of the terrain in units.
		/// </summary>
		/// <remarks>
		/// The terrain system calculates the overall size by multiplying this value by the units
		/// per metre setting. A map set to 1024 units at 2 metres per unit will have a size of 2048 metres.
		/// </remarks>
		public static int UnitSize { get { return Size / UnitsPerMetre; } }

		public static float GetTerrainElevation(int x, int y)
		{
			return Native3DEngineMethods.GetTerrainZ(x, y);
		}

		public static float GetTerrainElevation(float x, float y)
		{
			return Native3DEngineMethods.GetTerrainElevation(x, y);
		}
	}
}