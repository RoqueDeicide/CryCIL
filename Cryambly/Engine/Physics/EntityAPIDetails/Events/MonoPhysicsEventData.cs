namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates basic information that is passed with every physics event that only involves one
	/// physical entity.
	/// </summary>
	public struct MonoPhysicsEventData
	{
		#region Fields
		private readonly PhysicalEntity entity;
		private readonly ForeignData foreignData;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the entity that is associated with the event.
		/// </summary>
		public PhysicalEntity Entity
		{
			get { return this.entity; }
		}
		/// <summary>
		/// Gets the object that encapsulates foreign data that is associated with the
		/// <see cref="Entity"/>.
		/// </summary>
		public ForeignData ForeignData
		{
			get { return this.foreignData; }
		}
		#endregion
		#region Construction
		internal MonoPhysicsEventData(PhysicalEntity entity, ForeignData foreignData)
		{
			this.entity = entity;
			this.foreignData = foreignData;
		}
		#endregion
	}
}