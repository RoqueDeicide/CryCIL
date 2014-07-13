using System;

using CryEngine;
using CryEngine.Utilities;
using NUnit.Framework;

namespace CryBrary.Tests.Misc
{
	public class UnusedMarkerTests
	{
		[Test]
		public void Float()
		{
			float myFloat = UnusedMarker.Float;
			Assert.True(UnusedMarker.IsUnused(myFloat));

			myFloat = 998.4f;
			Assert.False(UnusedMarker.IsUnused(myFloat));
		}

		[Test]
		public void Integer()
		{
			int myInt = UnusedMarker.Integer;
			Assert.True(UnusedMarker.IsUnused(myInt));

			myInt = 5;
			Assert.False(UnusedMarker.IsUnused(myInt));
		}

		[Test]
		public void UnsignedInteger()
		{
			uint myUint = UnusedMarker.UnsignedInteger;
			Assert.True(UnusedMarker.IsUnused(myUint));

			myUint = 51;
			Assert.False(UnusedMarker.IsUnused(myUint));
		}

		[Test]
		public void Vec3()
		{
			Vector3 vector3 = UnusedMarker.Vector3;
			Assert.True(UnusedMarker.IsUnused(vector3));

			vector3 = new Vector3(0, 250, 3);
			Assert.False(UnusedMarker.IsUnused(vector3));
		}
	}
}