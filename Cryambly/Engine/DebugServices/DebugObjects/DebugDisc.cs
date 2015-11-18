using System;
using System.Collections.Generic;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that represents either a 2D ring or a 2D circle.
	/// </summary>
	public class DebugDisc : Debug3DObject
	{
		#region Fields
		private uint[] indexes;
		private Vector3[] vertexes;
		private float innerRadius;
		private float outerRadius;
		private Quaternion orientation;
		private static readonly uint[] triangleIndexes = {0u, 1u, 2u};
		private static readonly uint[] quadIndexes = {0u, 2u, 1u, 0u, 3u, 2u};
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets coordinates of the center of the sphere.
		/// </summary>
		public Vector3 Center { get; set; }
		/// <summary>
		/// Gets or sets inner radius of this disc.
		/// </summary>
		public float InnerRadius
		{
			get { return this.innerRadius; }
			set
			{
				if (Math.Abs(value - this.innerRadius) < MathHelpers.ZeroTolerance)
				{
					return;
				}
				this.innerRadius = value;
				this.UpdateMesh();
			}
		}
		/// <summary>
		/// Gets or sets outer radius of this disc.
		/// </summary>
		public float OuterRadius
		{
			get { return this.outerRadius; }
			set
			{
				if (Math.Abs(value - this.outerRadius) < MathHelpers.ZeroTolerance)
				{
					return;
				}
				this.outerRadius = value;
				this.UpdateMesh();
			}
		}
		/// <summary>
		/// Gets or sets quaternion that represents orientation of this disc.
		/// </summary>
		public Quaternion Orientation
		{
			get { return this.orientation; }
			set
			{
				if (value.IsEquivalent(this.orientation, MathHelpers.ZeroTolerance))
				{
					return;
				}
				this.orientation = value;
				this.UpdateMesh();
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that represents a circle.
		/// </summary>
		/// <param name="radius">Radius of the circle.</param>
		public DebugDisc(float radius)
			: this(radius, Quaternion.Identity)
		{
		}
		/// <summary>
		/// Creates an object that represents a circle.
		/// </summary>
		/// <param name="radius">     Radius of the circle.</param>
		/// <param name="orientation">Orientation of the circle.</param>
		public DebugDisc(float radius, Quaternion orientation)
		{
			this.innerRadius = 0.0f;
			this.outerRadius = radius;
			this.orientation = orientation;
			this.UpdateMesh();
		}
		/// <summary>
		/// Creates an object that represents a ring.
		/// </summary>
		/// <param name="innerRadius">Inner radius of the ring.</param>
		/// <param name="outerRadius">Outer radius of the ring.</param>
		public DebugDisc(float innerRadius, float outerRadius)
			: this(innerRadius, outerRadius, Quaternion.Identity)
		{
		}
		/// <summary>
		/// Creates an object that represents a ring.
		/// </summary>
		/// <param name="innerRadius">Inner radius of the ring.</param>
		/// <param name="outerRadius">Outer radius of the ring.</param>
		/// <param name="orientation">Orientation of the ring.</param>
		public DebugDisc(float innerRadius, float outerRadius, Quaternion orientation)
		{
			this.innerRadius = innerRadius;
			this.outerRadius = outerRadius;
			this.orientation = orientation;
			this.UpdateMesh();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this disc.
		/// </summary>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawTriangles(this.vertexes, this.indexes, this.Color);
		}
		#endregion
		#region Utilities
		private void UpdateMesh()
		{
			int steps = Math.Max((int)(10 * this.outerRadius), 10);
			float angularStep = (float)(MathHelpers.PI2 / steps);

			List<Vector3> vertexList = new List<Vector3>((steps + 1) * 2);
			List<uint> indexList = new List<uint>((steps + 1) * 12);

			// Calculate all sines and cosines.
			Vector3[] sinesCosines = new Vector3[steps + 1];
			for (int i = 0; i < sinesCosines.Length; i++)
			{
				sinesCosines[i] = new Vector3(angularStep * i, 0, 0);
			}

			BatchOps.Math.SineCosine(sinesCosines);

			for (int i = 0; i < steps; i++)
			{
				Vector3 angleSineCosine0 = sinesCosines[i];
				Vector3 angleSineCosine1 = sinesCosines[i + 1];
				Vector3 v0 = new Vector3(angleSineCosine0.Z, angleSineCosine0.Y, 0);
				Vector3 v1 = new Vector3(angleSineCosine1.Z, angleSineCosine1.Y, 0);

				if (this.innerRadius > MathHelpers.ZeroTolerance)
				{
					// Form a part of the 2D ring.
					vertexList.Add(this.Center + this.innerRadius * v0);
					vertexList.Add(this.Center + this.innerRadius * v1);
					vertexList.Add(this.Center + this.outerRadius * v1);
					vertexList.Add(this.Center + this.outerRadius * v0);

					indexList.AddRange(quadIndexes);
				}
				else
				{
					// Form a part of the 2D circle.
					vertexList.Add(this.Center);
					vertexList.Add(this.Center + this.outerRadius * v0);
					vertexList.Add(this.Center + this.outerRadius * v1);

					indexList.AddRange(triangleIndexes);
				}
			}

			this.vertexes = vertexList.ToArray();
			this.indexes = indexList.ToArray();

			if (this.orientation.IsEquivalent(Quaternion.Identity, MathHelpers.ZeroTolerance))
			{
				return;
			}

			// Rotate the disc.
			for (int i = 0; i < this.vertexes.Length; i++)
			{
				Transformation.Apply(ref this.vertexes[i], ref this.orientation);
			}
		}
		#endregion
	}
}