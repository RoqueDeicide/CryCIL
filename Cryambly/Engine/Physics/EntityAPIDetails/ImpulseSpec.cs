using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates parameters that describe the impulse.
	/// </summary>
	public struct ImpulseSpec
	{
		#region Fields
		internal bool HasDir;
		internal Vector3 Dir;
		internal bool HasAng;
		internal Vector3 Ang;
		internal bool HasPoint;
		internal Vector3 _Point;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets vector of directional impulse.
		/// </summary>
		public Vector3 Direction
		{
			get { return this.Dir; }
			set
			{
				this.Dir = value;
				this.HasDir = true;
			}
		}
		/// <summary>
		/// Gets or sets vector of angular impulse.
		/// </summary>
		public Vector3 AngularImpulse
		{
			get { return this.Ang; }
			set
			{
				this.Ang = value;
				this.HasAng = true;
			}
		}
		/// <summary>
		/// Gets or sets coordinates of the point of application of the impulse.
		/// </summary>
		public Vector3 Point
		{
			get { return this._Point; }
			set
			{
				this._Point = value;
				this.HasPoint = true;
			}
		}
		#endregion
	}
}