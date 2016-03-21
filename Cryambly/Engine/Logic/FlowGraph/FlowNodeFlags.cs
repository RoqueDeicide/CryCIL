using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that can be set for a flow node.
	/// </summary>
	[Flags]
	public enum FlowNodeFlags : uint
	{
		/// <summary>
		/// Core flag that if set indicates that this node targets an entity, therefore the target Id must
		/// be provided.
		/// </summary>
		TargetEntity = 0x0001,
		/// <summary>
		/// Core flag that if set indicates that this node cannot be selected by user for placement in flow
		/// graph UI.
		/// </summary>
		HideUi = 0x0002,
		/// <summary>
		/// Core flag that if set indicates that this node is setup for dynamic output port growth at
		/// runtime.
		/// </summary>
		DynamicOutput = 0x0004,
		/// <summary>
		/// Core flag that if set indicates that this node cannot be deleted by the user.
		/// </summary>
		Unremoveable = 0x0008,
		/// <summary>
		/// Mask that allows to isolate core flags from the rest.
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
		/// Category flag that if set indicates that this node is obsolete and is not available in the
		/// editor.
		/// </summary>
		Obsolete = 0x0200,
		/// <summary>
		/// Mask that allows to isolate category flags from the rest.
		/// </summary>
		CategoryMask = 0x0FF0,

		/// <summary>
		/// Mask that allows to isolate unused second half of second byte.
		/// </summary>
		CustomMask = 0xF000
	}
}