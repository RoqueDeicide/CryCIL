namespace CryCil.Geometry
{
	/// <summary>
	/// Defines functions that are used when there is a need to perform scale affine
	/// transformation.
	/// </summary>
	public static class Scale
	{
		/// <summary>
		/// Applies scaling transformation to the vector.
		/// </summary>
		/// <remarks>
		/// Applying scaling transformation is as simple as multiplying respective
		/// components of given two vectors.
		/// </remarks>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="scale">         
		/// <see cref="Vector3"/> that provides factors of scaling along both axes.
		/// </param>
		public static void Apply(ref Vector2 vectorToChange, ref Vector2 scale)
		{
			vectorToChange.X *= scale.X;
			vectorToChange.Y *= scale.Y;
		}
		/// <summary>
		/// Applies scaling transformation to the vector.
		/// </summary>
		/// <remarks>
		/// Applying scaling transformation is as simple as multiplying respective
		/// components of given two vectors.
		/// </remarks>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="scale">         
		/// <see cref="Vector3"/> that provides factors of scaling along first two axes.
		/// </param>
		public static void Apply(ref Vector2 vectorToChange, ref Vector3 scale)
		{
			vectorToChange.X *= scale.X;
			vectorToChange.Y *= scale.Y;
		}
		/// <summary>
		/// Applies scaling transformation to the vector.
		/// </summary>
		/// <remarks>
		/// Applying scaling transformation is as simple as multiplying respective
		/// components of given two vectors.
		/// </remarks>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="scale">         
		/// <see cref="Vector3"/> that provides factors of scaling along all three axes.
		/// </param>
		public static void Apply(ref Vector3 vectorToChange, ref Vector3 scale)
		{
			vectorToChange.X *= scale.X;
			vectorToChange.Y *= scale.Y;
			vectorToChange.Z *= scale.Z;
		}
		/// <summary>
		/// Applies scaling transformation to the vector.
		/// </summary>
		/// <remarks>
		/// Applying scaling transformation is as simple as multiplying respective
		/// components of given two vectors.
		/// </remarks>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="scale">         
		/// <see cref="Vector3"/> that provides factors of scaling along all three axes.
		/// </param>
		public static void Apply(ref Vector4 vectorToChange, ref Vector3 scale)
		{
			vectorToChange.X *= scale.X;
			vectorToChange.Y *= scale.Y;
			vectorToChange.Z *= scale.Z;
		}
		/// <summary>
		/// Applies scaling transformation to the vector.
		/// </summary>
		/// <remarks>
		/// Applying scaling transformation is as simple as multiplying respective
		/// components of given two vectors.
		/// </remarks>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="scale">         
		/// <see cref="Vector3"/> that provides factors of scaling along all four axes.
		/// </param>
		public static void Apply(ref Vector4 vectorToChange, ref Vector4 scale)
		{
			vectorToChange.X *= scale.X;
			vectorToChange.Y *= scale.Y;
			vectorToChange.Z *= scale.Z;
		}
		/// <summary>
		/// Multiplies all components of given vector by given scaling factor.
		/// </summary>
		/// <param name="vectorToChange">   Vector that needs to be scaled.</param>
		/// <param name="commonScaleFactor">Scaling factor.</param>
		public static void Apply(ref Vector2 vectorToChange, float commonScaleFactor)
		{
			vectorToChange.X *= commonScaleFactor;
			vectorToChange.Y *= commonScaleFactor;
		}
		/// <summary>
		/// Multiplies all components of given vector by given scaling factor.
		/// </summary>
		/// <param name="vectorToChange">   Vector that needs to be scaled.</param>
		/// <param name="commonScaleFactor">Scaling factor.</param>
		public static void Apply(ref Vector3 vectorToChange, float commonScaleFactor)
		{
			vectorToChange.X *= commonScaleFactor;
			vectorToChange.Y *= commonScaleFactor;
			vectorToChange.Z *= commonScaleFactor;
		}
		/// <summary>
		/// Multiplies all components of given vector by given scaling factor.
		/// </summary>
		/// <param name="vectorToChange">   Vector that needs to be scaled.</param>
		/// <param name="commonScaleFactor">Scaling factor.</param>
		public static void Apply(ref Vector4 vectorToChange, float commonScaleFactor)
		{
			vectorToChange.X *= commonScaleFactor;
			vectorToChange.Y *= commonScaleFactor;
			vectorToChange.Z *= commonScaleFactor;
			vectorToChange.W *= commonScaleFactor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// X-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongX">    
		/// Desired length of X-component of given vector.
		/// </param>
		public static void ToX(ref Vector2 vectorToChange, float sizeAlongX)
		{
			// Calculate desired factor.
			float factor = sizeAlongX / vectorToChange.X;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// X-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongX">    
		/// Desired length of X-component of given vector.
		/// </param>
		public static void ToX(ref Vector3 vectorToChange, float sizeAlongX)
		{
			// Calculate desired factor.
			float factor = sizeAlongX / vectorToChange.X;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// X-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongX">    
		/// Desired length of X-component of given vector.
		/// </param>
		public static void ToX(ref Vector4 vectorToChange, float sizeAlongX)
		{
			// Calculate desired factor.
			float factor = sizeAlongX / vectorToChange.X;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
			vectorToChange.W *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// Y-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongY">    
		/// Desired length of Y-component of given vector.
		/// </param>
		public static void ToY(ref Vector2 vectorToChange, float sizeAlongY)
		{
			// Calculate desired factor.
			float factor = sizeAlongY / vectorToChange.Y;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// Y-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongY">    
		/// Desired length of Y-component of given vector.
		/// </param>
		public static void ToY(ref Vector3 vectorToChange, float sizeAlongY)
		{
			// Calculate desired factor.
			float factor = sizeAlongY / vectorToChange.Y;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// Y-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongY">    
		/// Desired length of Y-component of given vector.
		/// </param>
		public static void ToY(ref Vector4 vectorToChange, float sizeAlongY)
		{
			// Calculate desired factor.
			float factor = sizeAlongY / vectorToChange.Y;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
			vectorToChange.W *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// Z-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongZ">    
		/// Desired length of Z-component of given vector.
		/// </param>
		public static void ToZ(ref Vector3 vectorToChange, float sizeAlongZ)
		{
			// Calculate desired factor.
			float factor = sizeAlongZ / vectorToChange.Z;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// Z-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongZ">    
		/// Desired length of Z-component of given vector.
		/// </param>
		public static void ToZ(ref Vector4 vectorToChange, float sizeAlongZ)
		{
			// Calculate desired factor.
			float factor = sizeAlongZ / vectorToChange.Z;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
			vectorToChange.W *= factor;
		}
		/// <summary>
		/// Scales given vector by a factor that is selected in such a way that resulting
		/// W-component will be equal to given number.
		/// </summary>
		/// <param name="vectorToChange">Vector that needs to be scaled.</param>
		/// <param name="sizeAlongW">    
		/// Desired length of W-component of given vector.
		/// </param>
		public static void ToW(ref Vector4 vectorToChange, float sizeAlongW)
		{
			// Calculate desired factor.
			float factor = sizeAlongW / vectorToChange.W;
			// Scale the vector.
			vectorToChange.X *= factor;
			vectorToChange.Y *= factor;
			vectorToChange.Z *= factor;
			vectorToChange.W *= factor;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by respective components of given vector.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Vector that provides scaling factors.</param>
		public static void Add(ref Matrix44 matrixToAddTo, ref Vector4 scale)
		{
			matrixToAddTo.M00 *= scale.X;
			matrixToAddTo.M11 *= scale.Y;
			matrixToAddTo.M22 *= scale.Z;
			matrixToAddTo.M33 *= scale.W;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by respective components of given vector.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Vector that provides scaling factors.</param>
		public static void Add(ref Matrix44 matrixToAddTo, ref Vector3 scale)
		{
			matrixToAddTo.M00 *= scale.X;
			matrixToAddTo.M11 *= scale.Y;
			matrixToAddTo.M22 *= scale.Z;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by respective components of given vector.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Vector that provides scaling factors.</param>
		public static void Add(ref Matrix34 matrixToAddTo, ref Vector3 scale)
		{
			matrixToAddTo.M00 *= scale.X;
			matrixToAddTo.M11 *= scale.Y;
			matrixToAddTo.M22 *= scale.Z;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by respective components of given vector.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Vector that provides scaling factors.</param>
		public static void Add(ref Matrix33 matrixToAddTo, ref Vector3 scale)
		{
			matrixToAddTo.M00 *= scale.X;
			matrixToAddTo.M11 *= scale.Y;
			matrixToAddTo.M22 *= scale.Z;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by given factor.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Scaling factor.</param>
		public static void Add(ref Matrix44 matrixToAddTo, float scale)
		{
			matrixToAddTo.M00 *= scale;
			matrixToAddTo.M11 *= scale;
			matrixToAddTo.M22 *= scale;
			matrixToAddTo.M33 *= scale;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by given factor.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Scaling factor.</param>
		public static void Add(ref Matrix34 matrixToAddTo, float scale)
		{
			matrixToAddTo.M00 *= scale;
			matrixToAddTo.M11 *= scale;
			matrixToAddTo.M22 *= scale;
		}
		/// <summary>
		/// Adds scaling transformation to given matrix.
		/// </summary>
		/// <remarks>
		/// Adding scaling transformation is done by multiplying components at main
		/// diagonal of the matrix by given factor.
		/// </remarks>
		/// <param name="matrixToAddTo">Matrix to add transformation to.</param>
		/// <param name="scale">        Scaling factor.</param>
		public static void Add(ref Matrix33 matrixToAddTo, float scale)
		{
			matrixToAddTo.M00 *= scale;
			matrixToAddTo.M11 *= scale;
			matrixToAddTo.M22 *= scale;
		}
		/// <summary>
		/// Creates matrix that represents scaling transformation.
		/// </summary>
		/// <param name="scale">Vector that provides scaling factors.</param>
		/// <returns>
		/// Matrix that is a result of multiplication of <see cref="Matrix33.Identity"/>
		/// by given vector.
		/// </returns>
		public static Matrix33 Create33(ref Vector3 scale)
		{
			Matrix33 result = Matrix33.Identity;

			result.M00 = scale.X;
			result.M11 = scale.Y;
			result.M22 = scale.Z;

			return result;
		}
		/// <summary>
		/// Creates matrix that represents scaling transformation.
		/// </summary>
		/// <param name="scale">Vector that provides scaling factors.</param>
		/// <returns>
		/// Matrix that is a result of multiplication of <see cref="Matrix44.Identity"/>
		/// by given vector.
		/// </returns>
		public static Matrix44 Create44(ref Vector4 scale)
		{
			Matrix44 result = Matrix44.Identity;

			result.M00 = scale.X;
			result.M11 = scale.Y;
			result.M22 = scale.Z;
			result.M33 = scale.W;

			return result;
		}

		// ReSharper disable RedundantAssignment

		/// <summary>
		/// Changes given matrix so it represents a single scaling transformation.
		/// </summary>
		/// <remarks>
		/// Any transformation previously stored in the matrix are discarded.
		/// </remarks>
		/// <param name="matrixToSet">Matrix to set.</param>
		/// <param name="scale">      Vector that provides scaling factors.</param>
		public static void Set(ref Matrix33 matrixToSet, ref Vector3 scale)
		{
			matrixToSet = Matrix33.Identity;

			matrixToSet.M00 = scale.X;
			matrixToSet.M11 = scale.Y;
			matrixToSet.M22 = scale.Z;
		}
		/// <summary>
		/// Changes given matrix so it represents a single scaling transformation.
		/// </summary>
		/// <remarks>
		/// Any transformation previously stored in the matrix are discarded.
		/// </remarks>
		/// <param name="matrixToSet">Matrix to set.</param>
		/// <param name="scale">      Vector that provides scaling factors.</param>
		public static void Set(ref Matrix44 matrixToSet, ref Vector4 scale)
		{
			matrixToSet = Matrix44.Identity;

			matrixToSet.M00 = scale.X;
			matrixToSet.M11 = scale.Y;
			matrixToSet.M22 = scale.Z;
			matrixToSet.M33 = scale.W;
		}

		// ReSharper restore RedundantAssignment
	}
}