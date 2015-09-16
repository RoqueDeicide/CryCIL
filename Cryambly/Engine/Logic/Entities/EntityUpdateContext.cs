using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Graphics;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates information about the context of entity update.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct EntityUpdateContext
	{
		#region Fields
		[UsedImplicitly] private int frameID;
		[UsedImplicitly] private Camera* camera;
		[UsedImplicitly] private float currTime;
		[UsedImplicitly] private float frameTime;
		[UsedImplicitly] private bool profileToLog;
		[UsedImplicitly] private int numUpdatedEntities;
		[UsedImplicitly] private int numVisibleEntities;
		[UsedImplicitly] private float maxViewDist;
		[UsedImplicitly] private float maxViewDistSquared;
		[UsedImplicitly] private Vector3 cameraPos;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of the current rendering frame.
		/// </summary>
		public int FrameIdentifier
		{
			get { return this.frameID; }
		}
		/// <summary>
		/// Gets pointer to the camera that is used to render the current frame.
		/// </summary>
		public Camera* Camera
		{
			get { return this.camera; }
		}
		/// <summary>
		/// Gets current system time.
		/// </summary>
		/// <remarks>This time is more up to date then <see cref="Time.FrameStart"/>.</remarks>
		public DateTime CurrentTime
		{
			get { return Time.FromSeconds(this.currTime); }
		}
		/// <summary>
		/// Gets length of the last frame in seconds.
		/// </summary>
		/// <remarks>This time is more up to date then <see cref="Time.Frame"/>.</remarks>
		public float FrameTime
		{
			get { return this.frameTime; }
		}
		#endregion
	}
}