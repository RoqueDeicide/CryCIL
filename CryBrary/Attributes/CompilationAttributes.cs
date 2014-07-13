using System;

namespace CryEngine
{
	/// <summary>
	/// If this attribute is attached to a class, it will be excluded from compilation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ExcludeFromCompilationAttribute : Attribute { }
}