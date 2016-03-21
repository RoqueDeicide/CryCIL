using System;
using System.Linq;
using CryCil.Engine.Models.Characters;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;

namespace CryCil.Engine
{
	/// <summary>
	/// Encapsulates information about a geometric object that is going to be used to attach particles to.
	/// </summary>
	public struct GeometryReference
	{
		#region Fields
		private StaticObject staticObject;
		private Character character;
		private PhysicalEntity physicalEntity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the static object to attach particle effect to.
		/// </summary>
		public StaticObject StaticObject
		{
			get { return this.staticObject; }
			set { this.staticObject = value; }
		}
		/// <summary>
		/// Gets or sets the animated character to attach particle effect to.
		/// </summary>
		public Character Character
		{
			get { return this.character; }
			set { this.character = value; }
		}
		/// <summary>
		/// Gets or sets the physical entity to attach particle effect to.
		/// </summary>
		public PhysicalEntity PhysicalEntity
		{
			get { return this.physicalEntity; }
			set { this.physicalEntity = value; }
		}
		#endregion
	}
}