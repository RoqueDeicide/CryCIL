using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Enumeration of options that specify how ocean is rendered.
	/// </summary>
	public enum OceanRenderOptions : byte
	{
		/// <summary>
		/// When set, disables rendering of the ocean water volume.
		/// </summary>
		/// <remarks>
		/// Used for vehicles and buildings that are partially underwater but not supposed to have water in
		/// them.
		/// </remarks>
		NoDraw = 1,
		/// <summary>
		/// When set, enables rendering of the ocean water volume.
		/// </summary>
		Visible = 2
	}
	/// <summary>
	/// Enumeration of flags that can specify what can be a valid bottom of the ocean.
	/// </summary>
	public enum ValidBottomFlags
	{
		/// <summary>
		/// Default value.
		/// </summary>
		Default = 0,
		/// <summary>
		/// ???
		/// </summary>
		Hidable = 0x1,
		/// <summary>
		/// ???
		/// </summary>
		HidableSecondary = 0x2,
		/// <summary>
		/// ???
		/// </summary>
		ExcludeFromStatic = 0x4,
		/// <summary>
		/// A simple static geometry.
		/// </summary>
		Brush = 0x8,
		/// <summary>
		/// A vegetation object.
		/// </summary>
		Vegetation = 0x10,
		/// <summary>
		/// ???
		/// </summary>
		Unimportant = 0x20,
		/// <summary>
		/// Outdoor terrain???
		/// </summary>
		OutdoorArea = 0x40,
		/// <summary>
		/// Moving platform object.
		/// </summary>
		MovingPlatform = 0x80
	}
	/// <summary>
	/// Encapsulates ocean caustics rendering parameters.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct OceanCaustics
	{
		/// <summary>
		/// Distance attenuation of light.
		/// </summary>
		[FieldOffset(4)] public float DistanceAttenuation;
		/// <summary>
		/// Caustics multiplier.
		/// </summary>
		[FieldOffset(8)] public float Multiplier;
		/// <summary>
		/// Caustics darkening multiplier.
		/// </summary>
		[FieldOffset(12)] public float DarkeningMultiplier;
	}
	/// <summary>
	/// Encapsulates ocean caustics animation parameters.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct OceanAnimationCaustics
	{
		/// <summary>
		/// Distance animation eight.
		/// </summary>
		[FieldOffset(4)] public float Height;
		/// <summary>
		/// Caustics animation depth.
		/// </summary>
		[FieldOffset(8)] public float Depth;
		/// <summary>
		/// Caustics animation intensity.
		/// </summary>
		[FieldOffset(12)] public float Intensity;
	}
	/// <summary>
	/// Encapsulates parameters that specify ocean animation.
	/// </summary>
	public struct OceanAnimation
	{
		/// <summary>
		/// Azimuth? of the wind direction.
		/// </summary>
		public float WindDirection;
		/// <summary>
		/// Speed of the wind.
		/// </summary>
		public float WindSpeed;
		/// <summary>
		/// Speed of waves.
		/// </summary>
		public float WavesSpeed;
		/// <summary>
		/// Number of waves.
		/// </summary>
		public float WavesAmount;
		/// <summary>
		/// Size of waves.
		/// </summary>
		public float WavesSize;
	}
	/// <summary>
	/// Provides access to CryEngine ocean and water API.
	/// </summary>
	public static class Ocean
	{
		#region Fields
		/// <summary>
		/// A value that is returned by <see cref="GetWaterLevel"/> when water cannot be found at the
		/// specified location.
		/// </summary>
		public const float UnknownWaterLevel = -1000000.0f;
		/// <summary>
		/// A value that is returned by <see cref="GetBottomLevel"/> when bottom cannot be found at the
		/// specified location.
		/// </summary>
		public const float UnknownBottomLevel = -1000000.0f;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets options that specify whether ocean should be rendered.
		/// </summary>
		public static extern OceanRenderOptions RenderOptions { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Gets number of ocean pixels that are visible on the screen.
		/// </summary>
		public static extern uint VisiblePixelCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the height of the ocean relative to the origin of coordinates.
		/// </summary>
		public static extern float WaterLevel { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets ocean caustics parameters.
		/// </summary>
		public static extern OceanCaustics CausticsParameters { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets ocean caustics animation parameters.
		/// </summary>
		public static extern OceanAnimationCaustics CausticsAnimationParameters {
			[MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets ocean animation parameters.
		/// </summary>
		public static extern OceanAnimation AnimationParameters { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether given position is below the water surface.
		/// </summary>
		/// <param name="pos">Coordinates of the point to check.</param>
		/// <returns>True, if given point is below the ocean level.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsUnderwater(ref Vector3 pos);
		/// <summary>
		/// Gets the closest bottom level.
		/// </summary>
		/// <param name="position">        Coordinates of the point.</param>
		/// <param name="maxRelevantDepth">Maximal relevant depth. Use for better performance.</param>
		/// <param name="objectFlags">     
		/// A set of flags that specifies what physical entity is a valid bottom.
		/// </param>
		/// <returns>
		/// Height of the interval between specified position and closest bottom level of the ocean below
		/// it. <see cref="UnknownBottomLevel"/> will be returned if given <paramref name="position"/> is
		/// below the terrain.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetBottomLevel(ref Vector3 position, float maxRelevantDepth = 10.0f,
												  ValidBottomFlags objectFlags = ValidBottomFlags.Default);
		/// <summary>
		/// Determines the water level straight above given position.
		/// </summary>
		/// <param name="position">Coordinates of the point where to check the water level.</param>
		/// <param name="accurate">Indicates if calculations must be accurate. Very slow, if true.</param>
		/// <returns>
		/// Height of the water region above the point or <see cref="UnknownWaterLevel"/>, if no water
		/// could be found at the specified position.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetWaterLevel(ref Vector3 position, bool accurate = false);
		#endregion
	}
}