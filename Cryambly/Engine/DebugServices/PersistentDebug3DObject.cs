using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Base class for persistent debug objects that are rendered in 3D space.
	/// </summary>
	public abstract class PersistentDebug3DObject : PersistentDebugObject
	{
		#region Fields
		/// <summary>
		/// Constant that represents default rendering flags for 3D persistent debug objects.
		/// </summary>
		public const AuxiliaryGeometryRenderFlags DefaultRenderFlags =
			AuxiliaryGeometryRenderFlags.Default3DRenderFlags |
					AuxiliaryGeometryRenderFlags.BlendingAlpha;
		#endregion
		#region Properties
		#endregion
		#region Construction
		/// <summary>
		/// Sets rendering flags to their default value for 3D object rendering.
		/// </summary>
		protected PersistentDebug3DObject()
		{
			this.RenderingFlags = DefaultRenderFlags;
		}
		#endregion
	}
}
