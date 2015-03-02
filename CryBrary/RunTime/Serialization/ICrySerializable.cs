using System;

namespace CryEngine.RunTime.Serialization
{
	/// <summary>
	/// Defines common properties of objects that can be serialized using CryEngine serializer.
	/// </summary>
	public interface ICrySerializable
	{
		/// <summary>
		/// When implemented in derived class, serializes the object.
		/// </summary>
		/// <param name="serializer">Serializer that will handle the process.</param>
		void Serialize(ICrySerialize serializer);
	}
}