using System;
using CryEngine.RunTime.Testing;

namespace CryBrary.Tests.UnitTester
{
	public class Exceptions
	{
		[Test]
		public void Throw_Valid()
		{
			var obj = new object();

			Assert.Throws<InvalidCastException>(() =>
			{
				var myInt = (int)obj;
			});
		}

		[Test]
		public void Throw_Invalid()
		{
			object obj = 1;

			Assert.Throws<AssertionFailedException>(() =>
			{
				Assert.Throws<InvalidCastException>(() =>
				{
					var myInt = (int)obj;
				});
			});
		}
	}
}