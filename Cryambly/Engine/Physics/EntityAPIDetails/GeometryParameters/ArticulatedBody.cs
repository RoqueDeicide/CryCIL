using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify the geometry that is used by one of the articulated
	/// bodies that form the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct GeometryParametersArticulatedBody
	{
		#region Fields
		/// <summary>
		/// Pass a reference to this field to <see cref="PhysicalEntity.AddGeometry"/> to add the geometry
		/// with these parameters.
		/// </summary>
		public GeometryParameters Base;
		[UsedImplicitly] private int idBody;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets an identifier of the sub-body this articulated body is attached to.
		/// </summary>
		public int BodyId
		{
			get { return this.idBody; }
			set { this.idBody = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass this parameter to invoked this constructor.</param>
		public GeometryParametersArticulatedBody([UsedImplicitly] int notUsed)
		{
			this.Base = new GeometryParameters(notUsed)
			{
				type = PhysicsGeometryParametersTypes.ArticulatedBody
			};
			this.idBody = 0;
		}
		#endregion
	}
}