namespace CryCil
{
	/// <summary>
	/// Defines common properties and functionality of types that represent vectors.
	/// </summary>
	/// <typeparam name="ComponentType">Type of vector components.</typeparam>
	/// <typeparam name="VectorType">Type that represents the vector itself, or any other type this one can operate with.</typeparam>
	public interface IVector<ComponentType, VectorType>
	{
		/// <summary>
		/// Gets the squared length of the vector.
		/// </summary>
		/// <remarks>
		/// Squared length of the vector is a sum of all components of this vector raised to power of 2.
		/// </remarks>
		/// <example>
		/// Implementation of this property in <see cref="Vector3"/>:
		/// <code>
		/// public float LengthSquared
		/// {
		///     get { return this.X * this.X + this.Y * this.Y + this.Z * this.Z; }
		/// }
		/// </code>
		/// </example>
		ComponentType LengthSquared { get; }
		/// <summary>
		/// Gets the length of the vector.
		/// </summary>
		/// <remarks>
		/// Length of the vector is a square root of sum of all components of this vector raised to power of 2.
		/// </remarks>
		/// <example>
		/// Implementation of this property in <see cref="Vector3"/>:
		/// <code>
		/// public float Length
		/// {
		///     get { return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z); }
		/// }
		/// </code>
		/// </example>
		ComponentType Length { get; }
		/// <summary>
		/// Provides read/write access to the component of the vector with a given index.
		/// </summary>
		/// <param name="index">Index of the component to access.</param>
		/// <returns>Accessed component.</returns>
		ComponentType this[int index] { get; set; }
		/// <summary>
		/// Gets the deep copy of this vector.
		/// </summary>
		/// <remarks>
		/// Only really needed for vectors that are classes. Structs can just use <c>return this;</c>.
		/// </remarks>
		VectorType DeepCopy { get; }
		/// <summary>
		/// Calculates the dot product of this vector and another one.
		/// </summary>
		/// <remarks>
		/// Dot product of 2 vectors is a sum of products of corresponding components.
		/// </remarks>
		/// <example>
		/// Implementation of this method in <see cref="Vector3"/>:
		/// <code>
		/// public float Dot(Vector3 other)
		/// {
		///     return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
		/// }
		/// </code>
		/// </example>
		/// <param name="other">Another vector.</param>
		/// <returns>Dot product of the 2 vectors.</returns>
		ComponentType Dot(VectorType other);
		/// <summary>
		/// Adds components from another vector to respective components of this one.
		/// </summary>
		/// <example>
		/// Implementation of this method in <see cref="Vector3"/>:
		/// <code>
		/// public void Add(Vector3 other)
		/// {
		///     this.X += other.X;
		///     this.Y += other.Y;
		///     this.Z += other.Z;
		/// }
		/// </code>
		/// </example>
		/// <param name="other">Another vector.</param>
		void Add(VectorType other);
		/// <summary>
		/// Multiplies components of this vector by the given factor.
		/// </summary>
		/// <example>
		/// Implementation of this method in <see cref="Vector3"/>:
		/// <code>
		/// public void Scale(float factor)
		/// {
		///     this.X *= factor;
		///     this.Y *= factor;
		///     this.Z *= factor;
		/// }
		/// </code>
		/// </example>
		/// <param name="factor">Scaling factor.</param>
		void Scale(ComponentType factor);
		/// <summary>
		/// Creates a new vector that is a sum of this one and another one.
		/// </summary>
		/// <example>
		/// Implementation of this method in <see cref="Vector3"/>:
		/// <code>
		/// public Vector3 Added(Vector3 other)
		/// {
		///     return new Vector3(this.X + other.X, this.Y + other.Y, this.Z + other.Z);
		/// }
		/// </code>
		/// </example>
		/// <param name="other">Another vector.</param>
		/// <returns>Sum of two vectors.</returns>
		VectorType Added(VectorType other);
		/// <summary>
		/// Creates a new vector that is a scaled version of this one.
		/// </summary>
		/// <example>
		/// Implementation of this method in <see cref="Vector3"/>:
		/// <code>
		/// public Vector3 Scaled(float factor)
		/// {
		///     return new Vector3(this.X * factor, this.Y * factor, this.Z * factor);
		/// }
		/// </code>
		/// </example>
		/// <param name="factor">Scaling factor.</param>
		/// <returns>Scaled vector.</returns>
		VectorType Scaled(ComponentType factor);
	}
}
