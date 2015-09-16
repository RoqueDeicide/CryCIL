using System;
using CryCil.Annotations;

namespace CryCil.RunTime
{
	/// <summary>
	/// Marks classes that define methods that are invoked during initialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class InitializationClassAttribute : Attribute
	{
	}
	/// <summary>
	/// Marks method that represents one of the initialization stages.
	/// </summary>
	/// <remarks>
	/// It is necessary for the method marked by this attribute to only accept one argument of type
	/// <see cref="Int32"/>.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	[MeansImplicitUse]
	public sealed class InitializationStageAttribute : Attribute
	{
		/// <summary>
		/// Creates new attribute.
		/// </summary>
		/// <param name="stageIndex">
		/// Index of the stage to assign to the method this attribute marks.
		/// </param>
		public InitializationStageAttribute(int stageIndex)
		{
			this.StageIndex = stageIndex;
		}
		/// <summary>
		/// Index of the stage that is assigned to the method marked by this attribute.
		/// </summary>
		public int StageIndex { get; private set; }
	}
}