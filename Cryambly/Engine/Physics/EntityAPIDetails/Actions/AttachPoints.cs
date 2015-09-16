using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that attaches a number of points of the soft body to another
	/// physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionAttachPoints
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private PhysicalEntity pEntity;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int[] piVtx;
		[UsedImplicitly] private Vector3[] points;
		[UsedImplicitly] private int bLocal;
		[UsedImplicitly] private IntPtr internal0;
		[UsedImplicitly] private IntPtr internal1;
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that can be used on soft bodies to automatically attach them to other
		/// entities using the parameters from the lattice.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsActionAttachPoints([UsedImplicitly] int notUsed)
			: this(PhysicalEntity.World, null)
		{
		}
		/// <summary>
		/// Creates an object that can be used on soft bodies to attach them to other entities.
		/// </summary>
		/// <param name="target">         An entity to attach this one to.</param>
		/// <param name="vertexes">       
		/// An array of indices of vertexes to attach to other entity.
		/// </param>
		/// <param name="vertexPositions">
		/// An optional array of new positions of attached points. If <c>null</c> is passed, then current
		/// positions are used.
		/// </param>
		/// <param name="localSpace">     
		/// Indicates whether <paramref name="vertexPositions"/> are in local coordinate space.
		/// </param>
		/// <param name="partId">         
		/// An optional identifier of the part of the entity to attach to.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Number of indices and number of positions must match.
		/// </exception>
		public PhysicsActionAttachPoints(PhysicalEntity target, int[] vertexes, Vector3[] vertexPositions = null,
										 bool localSpace = false, int partId = UnusedValue.Int32)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.AttachPoints);
			this.bLocal = localSpace ? 1 : 0;
			this.internal0 = new IntPtr();
			this.internal1 = new IntPtr();
			this.pEntity = target;
			this.partid = partId;
			if (vertexes != null && vertexes.Length == 0)
			{
				vertexes = null;
			}
			if (vertexPositions != null && vertexPositions.Length == 0)
			{
				vertexPositions = null;
			}
			this.piVtx = vertexes;
			this.points = vertexPositions;

			if (this.piVtx != null && this.points != null && this.piVtx.Length != this.points.Length)
			{
				throw new ArgumentException("Number of indices and number of positions must match.");
			}
		}
		#endregion
	}
}