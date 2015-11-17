using System;
using CryCil.Annotations;
using CryCil.Utilities;

namespace CryCil.Engine.Logic
{
	// Encapsulates description of the flow port.
	internal struct FlowPortConfig
	{
		[UsedImplicitly] private IntPtr name; // Internal name.
		[UsedImplicitly] private IntPtr humanName; // Display name.
		[UsedImplicitly] private IntPtr description; // Description of the port.
		[UsedImplicitly] private IntPtr enumConfig; // Enumeration of selectable values for an input port.
		[UsedImplicitly] private FlowData defaultValue; // Default value and type for input ports and type for output ports.
		// Creates an object that describes an input port.
		/// <exception cref="OutOfMemoryException">There is insufficient memory available.</exception>
		internal FlowPortConfig(string name, string displayName, string description, string enumConfig, FlowData def)
		{
			this.name = StringPool.Get(name);
			this.humanName = StringPool.Get(displayName);
			this.description = StringPool.Get(description);
			this.enumConfig = StringPool.Get(enumConfig);
			this.defaultValue = def;
		}
		// Creates an object that describes an output port.
		/// <exception cref="OutOfMemoryException">There is insufficient memory available.</exception>
		internal FlowPortConfig(string name, string displayName, string description, FlowDataType type)
		{
			this.name = StringPool.Get(name);
			this.humanName = StringPool.Get(displayName);
			this.description = StringPool.Get(description);
			this.enumConfig = IntPtr.Zero;
			this.defaultValue = new FlowData(type);
		}
	}
}