using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that is a manifestation of the quaternion in 3D world.
	/// </summary>
	public class PersistentDebugQuaternion : PersistentDebug3DObject
	{
		#region Fields
		private Vector3 x;
		private Vector3 y;
		private Vector3 z;
		private Quaternion quat;
		private static OBB obb = new OBB(Matrix33.Identity, new Vector3(0.5f), new Vector3());
		private static ColorByte red = new ColorByte(255, 0, 0, 255);
		private static ColorByte green = new ColorByte(0, 255, 0, 255);
		private static ColorByte blue = new ColorByte(0, 0, 255, 255);
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets position of this object in 3D world.
		/// </summary>
		public Vector3 Position { get; set; }
		/// <summary>
		/// Gets or sets a quaternion that is represented by this object.
		/// </summary>
		public Quaternion Quaternion
		{
			get { return this.quat; }
			set
			{
				if (this.quat.IsEquivalent(value, MathHelpers.ZeroTolerance))
				{
					return;
				}

				this.quat = value;
				// Update axes.
				this.x = quat.Column0;
				this.y = quat.Column1;
				this.z = quat.Column2;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="quat">Quaternion new object is going to represent.</param>
		public PersistentDebugQuaternion(Quaternion quat)
		{
			this.Quaternion = quat;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this quaternion.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawOBB(ref obb, this.Position, false, this.Color, BoundingBoxRenderStyle.ExtremesColorEncoded);
			AuxiliaryGeometry.DrawLine(this.Position, this.Position + this.x, red.ModifyAlpha(this.Color.Alpha));
			AuxiliaryGeometry.DrawLine(this.Position, this.Position + this.y, green.ModifyAlpha(this.Color.Alpha));
			AuxiliaryGeometry.DrawLine(this.Position, this.Position + this.z, blue.ModifyAlpha(this.Color.Alpha));
		}
		#endregion
	}
}