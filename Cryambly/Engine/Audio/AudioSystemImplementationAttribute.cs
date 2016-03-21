using System;
using System.Linq;
using CryCil.Annotations;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Marks class that derives from <see cref="AudioSystemImplementation"/> to make it possible to be
	/// registered.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[BaseTypeRequired(typeof(AudioSystemImplementation))]
	public sealed class AudioSystemImplementationAttribute : Attribute
	{
	}
}