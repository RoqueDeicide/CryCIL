using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a <see cref="Quaternion"/> that is packed into a <see cref="Vector4Int16"/>
	/// </summary>
	public struct CryMeshQTangents
	{
		#region Fields
		private Vector4Int16 qTangent;
		#endregion
		#region Properties
		/// <summary>
		/// Gets quaternion that packed into this object.
		/// </summary>
		public Quaternion Quaternion
		{
			get
			{
				return new Quaternion
				{
					X = Quantize.Int16ToSingle(this.qTangent.X),
					Y = Quantize.Int16ToSingle(this.qTangent.Y),
					Z = Quantize.Int16ToSingle(this.qTangent.Z),
					W = Quantize.Int16ToSingle(this.qTangent.W)
				};
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="quaternion">Quaternion to pack into the new object.</param>
		public CryMeshQTangents(Quaternion quaternion)
		{
			this.qTangent.X = Quantize.SingleToInt16(quaternion.X);
			this.qTangent.Y = Quantize.SingleToInt16(quaternion.Y);
			this.qTangent.Z = Quantize.SingleToInt16(quaternion.Z);
			this.qTangent.W = Quantize.SingleToInt16(quaternion.W);
		}
		#endregion
	}
}