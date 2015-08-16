using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Base part of all structures that encapsulate parameters that are used to perform actions on
	/// physical entities.
	/// </summary>
	/// <remarks>Never create objects of this type directly.</remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsAction
	{
		private readonly PhysicsActionTypes type;
		private readonly bool initialized;
		/// <summary>
		/// Indicates whether an object of type that uses this object was not created using default
		/// constructor.
		/// </summary>
		public bool Initialized
		{
			get { return this.initialized; }
		}
		/// <summary>
		/// Gets the type of this action.
		/// </summary>
		public PhysicsActionTypes Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="type">A type of this action.</param>
		internal PhysicsAction(PhysicsActionTypes type)
		{
			this.initialized = true;
			this.type = type;
		}
	}
}