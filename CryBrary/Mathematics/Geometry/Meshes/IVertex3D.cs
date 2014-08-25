using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common properties of vertices located in 3D space.
	/// </summary>
	public interface IVertex3D : IVertex
	{
		/// <summary>
		/// When implemented, gets 3-dimensional vector that represents location of this vertex in 3D space.
		/// </summary>
		Vector3 Location { get; }
	}
}
