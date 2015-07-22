using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Network
{
	/// <summary>
	/// Represents identifier of a network channel that can be used to communicate via network.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ChannelId
	{
		#region Fields
		[UsedImplicitly]
		private ushort id;
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="id">Identifier to assign to this object.</param>
		public ChannelId(ushort id)
		{
			this.id = id;
		}
		#endregion
		#region Interface

		#endregion
		#region Utilities

		#endregion
	}
}