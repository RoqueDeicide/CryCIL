namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a combination of quaternion and a vector.
	/// </summary>
	/// <remarks>
	/// Such combination allows one to represent two transformations: rotation and
	/// translation by using only 7 numbers instead of 12 (when using
	/// <see cref="Matrix34"/>) or 16 (when using <see cref="Matrix44"/>).
	/// </remarks>
	public struct Quatvec
	{
		/// <summary>
		/// Combination of <see cref="Quaternion.Identity"/> and zeroed vector.
		/// </summary>
		public static readonly Quatvec Identity = new Quatvec(new Vector3(), Quaternion.Identity);
		#region Fields
		/// <summary>
		/// Quaternion that represents a rotation.
		/// </summary>
		public Quaternion Quaternion;
		/// <summary>
		/// Vector that represents translation.
		/// </summary>
		public Vector3 Vector;
		#endregion
		#region Properties

		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="Quatvec"/>.
		/// </summary>
		/// <param name="t"><see cref="Vector3"/> that represents translation.</param>
		/// <param name="q"><see cref="Quaternion"/> that represents orientation.</param>
		public Quatvec(Vector3 t, Quaternion q)
		{
			this.Quaternion = q;
			this.Vector = t;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Quatvec"/>.
		/// </summary>
		/// <param name="m">
		/// <see cref="Matrix34"/> that represents both translation and orientation.
		/// </param>
		public Quatvec(Matrix34 m)
		{
			this.Quaternion = new Quaternion(m);
			this.Vector = m.Translation;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities

		#endregion
	}
}