using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph
{
	[Flags]
	internal enum FlowNodeFlags
	{
		/// <summary>
		/// This node targets an entity, entity id must be provided.
		/// </summary>
		TargetEntity = 0x0001,

		/// <summary>
		/// This node cannot be selected by user for placement in flow graph UI.
		/// </summary>
		HideUI = 0x0002,

		/// <summary>
		/// This node is setup for dynamic output port growth in runtime.
		/// </summary>
		DynamicOutput = 0x0004,

		/// <summary>
		/// This node cannot be deleted by the user.
		/// </summary>
		Unremovable = 0x0008,
	}
}