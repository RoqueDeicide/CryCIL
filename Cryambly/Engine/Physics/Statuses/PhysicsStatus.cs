using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Base part of all structures that encapsulate parameters that are used to query data about the physical entity.
	/// </summary>
	/// <remarks>
	/// Never create objects of this type directly.
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatus
	{
		private readonly PhysicsStatusTypes type;
		/// <summary>
		/// Gets the type of this status query.
		/// </summary>
		public PhysicsStatusTypes Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="type">A type of the status query.</param>
		internal PhysicsStatus(PhysicsStatusTypes type)
		{
			this.type = type;
		}
	}
}