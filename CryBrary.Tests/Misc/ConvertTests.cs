using System;
using NUnit.Framework;

namespace CryBrary.Tests.Misc
{
	public class ConvertTests
	{
		[Test]
		public void FromString_ValidBool_BoolResult()
		{
			// Arrange
			const string allLowercaseBool = "true";
			const string capitalizedBool = "True";
			const string allUppercaseBool = "TRUE";

			// Act
			object allLowercaseBoolResult = CryEngine.Convert.FromString(CryEngine.EditorPropertyType.Bool, allLowercaseBool);
			object capitalizedBoolResult = CryEngine.Convert.FromString(CryEngine.EditorPropertyType.Bool, capitalizedBool);
			object allUppercaseBoolResult = CryEngine.Convert.FromString(CryEngine.EditorPropertyType.Bool, allUppercaseBool);

			// Assert
			Assert.True(allLowercaseBoolResult is bool);
			Assert.True((bool)allLowercaseBoolResult);

			Assert.True(capitalizedBoolResult is bool);
			Assert.True((bool)capitalizedBoolResult);

			Assert.True(allUppercaseBoolResult is bool);
			Assert.True((bool)allUppercaseBoolResult);
		}

		[Test]
		public void FromString_NullBool_ArgumentNullException()
		{
			// Arrange
			string input = null;

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				CryEngine.Convert.FromString(CryEngine.EditorPropertyType.Bool, input);
			});
		}
	}
}