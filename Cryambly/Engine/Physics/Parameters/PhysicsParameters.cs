namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Base part of all structures that encapsulate parameters that are used to change physical entities.
	/// </summary>
	public struct PhysicsParameters
	{
		private readonly PhysicsParametersTypes type;
		/// <summary>
		/// Gets the type of this parameter set.
		/// </summary>
		public PhysicsParametersTypes Type
		{
			get { return this.type; }
		}
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="type">A type of this set.</param>
		internal PhysicsParameters(PhysicsParametersTypes type)
		{
			this.type = type;
		}
	}
}