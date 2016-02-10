using System;
using System.Linq;

namespace CryCil.Utilities
{
	/// <summary>
	/// Implemented by objects that exist for the purpose of temporarily overriding some data.
	/// </summary>
	public interface ITemporaryOverrider : IDisposable
	{
	}
}