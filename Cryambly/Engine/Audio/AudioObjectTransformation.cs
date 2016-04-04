namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Encapsulates information about the transformation of the audio object.
	/// </summary>
	/// <remarks>
	/// The default object of this type is represented by <see cref="Default"/> field, rather then default
	/// constructor.
	/// </remarks>
	public struct AudioObjectTransformation
	{
		#region Fields
		/// <summary>
		/// 3D vector that represents a set of coordinates that specify position of the audio object.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// 3D vector that represents the direction the audio object is facing.
		/// </summary>
		public Vector3 Forward;
		/// <summary>
		/// 3D vector that represents the direction the audio object's top is facing.
		/// </summary>
		public Vector3 Up;

		/// <summary>
		/// Represents a default object of this type.
		/// </summary>
		public static readonly AudioObjectTransformation Default = new AudioObjectTransformation
		{
			Position = Vector3.Zero,
			Forward = Vector3.Forward,
			Up = Vector3.Up
		};
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="position">Coordinates of the audio object.</param>
		public AudioObjectTransformation(Vector3 position)
		{
			this.Position = position;
			this.Forward = Vector3.Forward;
			this.Up = Vector3.Up;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="transformation">
		/// A 3x4 matrix to construct this object from. This matrix is assumed to be in standard CryEngine
		/// coordinate system (Forward - (0, 1, 0), Up - (0, 0, 1)) and is unscaled.
		/// </param>
		public AudioObjectTransformation(Matrix34 transformation)
		{
			this.Position = transformation.Translation;
			this.Forward = transformation.ColumnVector1.Normalized;
			this.Up = transformation.ColumnVector2.Normalized;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this object can be considered equivalent to the 3x4 matrix.
		/// </summary>
		/// <param name="transformation">Reference to 3x4 matrix to compare this object to.</param>
		/// <param name="precision">     Precision of comparison.</param>
		/// <returns>True, if 2 objects can be considered equal.</returns>
		public bool IsEquivalent(ref Matrix34 transformation, float precision)
		{
			return this.Position.IsEquivalent(transformation.Translation, precision) &&
				   this.Forward.IsEquivalent(transformation.ColumnVector1, precision) &&
				   this.Up.IsEquivalent(transformation.ColumnVector2, precision);
		}
		#endregion
	}
}