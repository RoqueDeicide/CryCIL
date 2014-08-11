using System.Collections.Generic;
using System.IO;
using System.Linq;

using CryEngine;
using CryEngine.Mathematics;
using CryEngine.Utilities;
using CryEngine.Serialization;
using NUnit.Framework;

namespace CryBrary.Tests.Serialization
{
	public class UnusedMarkerSerializationTests
	{
		[Test]
		public void Unused_Integer()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				var unused = UnusedMarker.Integer;
				serializer.Serialize(stream, unused);

				unused = (int)serializer.Deserialize(stream);
				Assert.True(UnusedMarker.IsUnused(unused));
			}
		}

		[Test]
		public void Unused_UnsignedInteger()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				var unused = UnusedMarker.UnsignedInteger;
				serializer.Serialize(stream, unused);

				unused = (uint)serializer.Deserialize(stream);
				Assert.True(UnusedMarker.IsUnused(unused));
			}
		}

		[Test]
		public void Unused_Float()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				var unused = UnusedMarker.Float;
				serializer.Serialize(stream, unused);

				unused = (float)serializer.Deserialize(stream);
				Assert.True(UnusedMarker.IsUnused(unused));
			}
		}

		[Test]
		public void Unused_Vec3()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				var unused = UnusedMarker.Vector3;
				serializer.Serialize(stream, unused);

				unused = (Vector3)serializer.Deserialize(stream);
				Assert.True(UnusedMarker.IsUnused(unused));
			}
		}
	}
}