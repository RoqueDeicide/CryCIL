using CryEngine.Testing;

namespace CryBrary.Tests.UnitTester
{
	public class Assertion
	{
		[Test]
		public void ReferenceEquals_Valid()
		{
			var lhs = new object();
			var rhs = lhs;

			Assert.IsTrue(lhs == rhs);
		}

		[Test]
		public void ReferenceEquals_Invalid()
		{
			var lhs = new object();
			var rhs = new object();

			Assert.Throws<AssertionFailedException>(() =>
			{
				Assert.IsTrue(lhs == rhs);
			});
		}
	}
}