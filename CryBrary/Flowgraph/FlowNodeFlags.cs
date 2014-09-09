using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Enumeration of flags that can be set for a flow node.
	/// </summary>
	[Flags]
	public enum FlowNodeFlags : uint
	{
		/// <summary>
		/// Core flag that if set indicates that this node targets an entity, therefore the target
		/// Id must be provided.
		/// </summary>
		TargetEntity = 0x0001,
		/// <summary>
		/// Core flag that if set indicates that this node cannot be selected by user for placement
		/// in flow graph UI.
		/// </summary>
		HideUi = 0x0002,
		/// <summary>
		/// Core flag that if set indicates that this node is setup for dynamic output port growth
		/// in runtime.
		/// </summary>
		DynamicOutput = 0x0004,
		/// <summary>
		/// Core flag that if set indicates that this node cannot be deleted by the user.
		/// </summary>
		Unremoveable = 0x0008,
		/// <summary>
		/// Mask that provides access to core flags.
		/// </summary>
		CoreMask = 0x000F,
		/// <summary>
		/// Category flag that if set indicates that this node is approved for designers.
		/// </summary>
		Approved = 0x0010,
		/// <summary>
		/// Category flag that if set indicates that this node is slightly advanced and approved.
		/// </summary>
		Advanced = 0x0020,
		/// <summary>
		/// Category flag that if set indicates that this node is for debug purpose only.
		/// </summary>
		Debug = 0x0040,
		/// <summary>
		/// Category flag that if set indicates that this node is obsolete and is not available in
		/// the editor.
		/// </summary>
		Obsolete = 0x0200,
		/// <summary>
		/// Mask that provides access to category flags.
		/// </summary>
		CategoryMask = 0x0FF0,
		/// <summary>
		/// Mask that provides access to unused second half of second byte.
		/// </summary>
		UsageMask = 0xF000,
		/// <summary>
		/// This node deals with AI sequence actions.
		/// </summary>
		AiSequenceAction = 0x010000,
		/// <summary>
		/// This node supports AI sequence actions.
		/// </summary>
		AiSequenceSupported = 0x020000,
		/// <summary>
		/// This node ends AI sequence actions.
		/// </summary>
		AiSequenceEnd = 0x040000,
		/// <summary>
		/// This node starts the AI action.
		/// </summary>
		AiActionStart = 0x080000,
		/// <summary>
		/// This node ends the AI action.
		/// </summary>
		AiActionEnd = 0x100000,
		/// <summary>
		/// Mask that allows easy access to AI flags.
		/// </summary>
		TypeMask = 0xFF0000,
	}
}