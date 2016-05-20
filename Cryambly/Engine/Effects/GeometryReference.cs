using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Models;
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
		private MeshObject meshObject;
		private PhysicalEntity physicalEntity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the static or animated object to attach the particle effect to.
		/// </summary>
		public MeshObject MeshObject
		{
			get { return this.meshObject; }
			set { this.meshObject = value; }
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