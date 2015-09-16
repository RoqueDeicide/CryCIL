using System.Runtime.CompilerServices;

namespace CryCil
{
	/// <summary>
	/// Defines classes where various batch operations are defined.
	/// </summary>
	public static unsafe partial class BatchOps
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MathSimpleOpSingle(float* numbers, long count, MathSimpleOperations op);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MathSimpleOpDouble(double* numbers, long count, MathSimpleOperations op);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Math3NumberOpSingle(Vector3* numbers, long count, Math3NumberOperations op);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Math3NumberOpDouble(Vector3Double* numbers, long count, Math3NumberOperations op);
	}
}