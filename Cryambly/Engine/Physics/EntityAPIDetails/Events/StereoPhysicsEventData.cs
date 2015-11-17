using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates basic information that is passed with every physics event that involves 2 physical
	/// entities.
	/// </summary>
	public struct StereoPhysicsEventData
	{
		#region Fields
		private readonly PhysicalEntity firstEntity;
		private readonly PhysicalEntity secondEntity;
		private readonly ForeignData firstForeignData;
		private readonly ForeignData secondForeignData;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the first entity that is associated with the event.
		/// </summary>
		public PhysicalEntity FirstEntity
		{
			get { return this.firstEntity; }
		}
		/// <summary>
		/// Gets the object that encapsulates foreign data that is associated with the
		/// <see cref="FirstEntity"/>.
		/// </summary>
		public ForeignData ForeignData
		{
			get { return this.firstForeignData; }
		}
		/// <summary>
		/// Gets the second entity that is associated with the event.
		/// </summary>
		public PhysicalEntity SecondEntity
		{
			get { return this.secondEntity; }
		}
		/// <summary>
		/// Gets the object that encapsulates foreign data that is associated with the
		/// <see cref="SecondEntity"/>.
		/// </summary>
		public ForeignData SecondForeignData
		{
			get { return this.secondForeignData; }
		}
		#endregion
		#region Construction
		internal StereoPhysicsEventData(PhysicalEntity firstEntity, ForeignData firstForeignData,
										PhysicalEntity secondEntity, ForeignData secondForeignData)
		{
			this.firstEntity = firstEntity;
			this.firstForeignData = firstForeignData;
			this.secondEntity = secondEntity;
			this.secondForeignData = secondForeignData;
		}
		#endregion
	}
}