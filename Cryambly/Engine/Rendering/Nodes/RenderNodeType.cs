using System;
using System.Linq;

namespace CryCil.Engine.Rendering.Nodes
{
	/// <summary>
	/// Enumeration of types of render nodes.
	/// </summary>
	public enum RenderNodeType
	{
		/// <summary>
		/// Not a render node.
		/// </summary>
		NotRenderNode,
		/// <summary>
		/// Represents a fully static object that is placed in the editor.
		/// </summary>
		Brush,
		/// <summary>
		/// Represents a vegetation object.
		/// </summary>
		Vegetation,
		/// <summary>
		/// Represents a light source.
		/// </summary>
		Light,
		/// <summary>
		/// Represents a cloud.
		/// </summary>
		Cloud,
		/// <summary>
		/// A dummy value that is holding the of VoxelObject to preserve compatibility.
		/// </summary>
		Dummy_1,
		/// <summary>
		/// Represents a volumetric fog.
		/// </summary>
		FogVolume,
		/// <summary>
		/// Represents a decal.
		/// </summary>
		Decal,
		/// <summary>
		/// Represents a particle emitter.
		/// </summary>
		ParticleEmitter,
		/// <summary>
		/// Represents a water volume.
		/// </summary>
		WaterVolume,
		/// <summary>
		/// Represents a water wave.
		/// </summary>
		WaterWave,
		/// <summary>
		/// Represents a road.
		/// </summary>
		Road,
		/// <summary>
		/// Represents a cloud at a distance.
		/// </summary>
		DistanceCloud,
		/// <summary>
		/// Represents a volumetric object.
		/// </summary>
		VolumeObject,
		/// <summary>
		/// A dummy value that is holding the of AutoCubeMap to preserve compatibility.
		/// </summary>
		Dummy_0,
		/// <summary>
		/// Represents a rope.
		/// </summary>
		Rope,
		/// <summary>
		/// Represents a prism object that is defined in the documentation.
		/// </summary>
		PrismObject,
		/// <summary>
		/// A dummy value that is holding the of IsoMesh to preserve compatibility.
		/// </summary>
		Dummy_2,
		/// <summary>
		/// Represents a volume of light propagation.
		/// </summary>
		LightPropagationVolume,
		/// <summary>
		/// Represents an entity's render proxy.
		/// </summary>
		RenderProxy,
		/// <summary>
		/// Represents a game effect.
		/// </summary>
		GameEffect,
		/// <summary>
		/// Represents a breakable glass.
		/// </summary>
		BreakableGlass,
		/// <summary>
		/// A dummy value that is holding the of LightShape to preserve compatibility.
		/// </summary>
		Dummy_3,
		/// <summary>
		/// Represents a merged mesh.
		/// </summary>
		MergedMesh,
		/// <summary>
		/// Represents a GeomCache object.
		/// </summary>
		GeomCache,
		/// <summary>
		/// Number of types of render nodes.
		/// </summary>
		Count
	}
}