using System;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about a simulation step of the physical entity.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TimeStepInfo
	{
		#region Fields
		private readonly float dt;
		private readonly Vector3 pos;
		private readonly Quaternion q;
		private readonly int idStep;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the time it took to simulate the step.
		/// </summary>
		public float StepTime => this.dt;
		/// <summary>
		/// Gets the position of the entity at the end of the step.
		/// </summary>
		public Vector3 Position => this.pos;
		/// <summary>
		/// Gets the orientation of the entity at the end of the step.
		/// </summary>
		public Quaternion Orientation => this.q;
		/// <summary>
		/// Gets the identifier of the step.
		/// </summary>
		public int StepId => this.idStep;
		#endregion
		#region Construction
		internal TimeStepInfo(float dt, Vector3 pos, Quaternion q, int idStep)
		{
			this.dt = dt;
			this.pos = pos;
			this.q = q;
			this.idStep = idStep;
		}
		#endregion
	}
}