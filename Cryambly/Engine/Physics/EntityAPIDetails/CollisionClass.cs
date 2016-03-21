using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information that can be used to indicate whether entity should ignore collision with
	/// another.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CollisionClass
	{
		#region Fields
		private ColliderTypes type;
		private ColliderTypes ignore;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets flags that specify this collision class.
		/// </summary>
		public ColliderTypes Type
		{
			get { return this.type; }
			set { this.type = value; }
		}
		/// <summary>
		/// Gets or sets 'ignore' flags. If another entity that collided with that uses this collision class
		/// has any bits from this entity's 'ignore' flags set in its 'type' flags, the collision will be
		/// ignored.
		/// </summary>
		public ColliderTypes Ignore
		{
			get { return this.ignore; }
			set { this.ignore = value; }
		}
		#endregion
	}
}