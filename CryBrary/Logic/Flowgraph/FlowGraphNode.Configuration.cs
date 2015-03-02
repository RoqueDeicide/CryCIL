using System;
using System.Runtime.InteropServices;
using CryEngine.NativeMemory;

namespace CryEngine.Logic.Flowgraph
{
	public partial class FlowGraphNode
	{
		/// <summary>
		/// Transfers configuration data to native memory.
		/// </summary>
		/// <param name="configPointer">Pointer to node configaration structure.</param>
		internal unsafe bool FillConfiguration(IntPtr configPointer)
		{
			// Let this node configure itself.
			if (this.Inputs == null)
			{
				this.Configure();
				// Tell ports, who we are and where they are.
				for (int i = 0; i < this.Inputs.Count; i++)
				{
					this.Inputs[i].Initialize(this, i);
				}
				for (int i = 0; i < this.Outputs.Count; i++)
				{
					this.Outputs[i].Initialize(this, i);
				}
			}
			NativeFlowNodeConfiguration* config =
				(NativeFlowNodeConfiguration*)configPointer.ToPointer();
			// I'm not too sure on how the memory is supposed to be freed, I guess CryEngine can handle
			// that.
			config->Description = Marshal.StringToHGlobalUni(this.Description);
			// Write the flags.
			config->Flags |= (uint)this.Flags;
			// Allocate arrays for ports.
			config->InputConfigurations =
				(NativeInputPortConfiguration*)
					CryMarshal
						.Allocate
						(
							(ulong)(Marshal.SizeOf(typeof(NativeInputPortConfiguration)) * this.Inputs.Count), false
						)
						.ToPointer();
			config->OutputConfigurations =
				(NativeOutputPortConfiguration*)
					CryMarshal
						.Allocate
						(
							(ulong)(Marshal.SizeOf(typeof(NativeOutputPortConfiguration)) * this.Outputs.Count), false
						)
						.ToPointer();
			// Transfer inputs and outputs.
			for (int i = 0; i < this.Inputs.Count; i++)
			{
				this.Inputs[i].WriteConfiguration(new IntPtr(config->InputConfigurations), i);
			}
			for (int i = 0; i < this.Outputs.Count; i++)
			{
				this.Outputs[i].WriteConfiguration(new IntPtr(config->OutputConfigurations), i);
			}
			return this.IsSingleton;
		}
	}
	/// <summary>
	/// SFlowNodeConfig's C# counterpart.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct NativeFlowNodeConfiguration
	{
		/// <summary>
		/// Pointer to configurations of inputs.
		/// </summary>
		public NativeInputPortConfiguration* InputConfigurations;
		/// <summary>
		/// Pointer to configurations of outputs.
		/// </summary>
		public NativeOutputPortConfiguration* OutputConfigurations;
		/// <summary>
		/// A set of flags that describe the node.
		/// </summary>
		public uint Flags;
		/// <summary>
		/// Pointer to description.
		/// </summary>
		public IntPtr Description;
		/// <summary>
		/// Pointer to unknown text.
		/// </summary>
		public IntPtr UserInterfaceClassName;
	}
	/// <summary>
	/// Input port configuration located in native memory.
	/// </summary>
	public struct NativeInputPortConfiguration
	{
		/// <summary>
		/// Null-terminated string that represents a name of the port.
		/// </summary>
		public IntPtr Name;
		/// <summary>
		/// Null-terminated string that represents a display name of the port.
		/// </summary>
		public IntPtr HumanName;
		/// <summary>
		/// Null-terminated string that represents a description of the port.
		/// </summary>
		public IntPtr Description;
		/// <summary>
		/// Null-terminated string that represents a list of enumeration values of the port.
		/// </summary>
		public IntPtr EnumConfig;
		/// <summary>
		/// Default value of the port.
		/// </summary>
		public Native.FlowInputData DefaultValue;
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="name">        Name of the port.</param>
		/// <param name="displayName"> Display name of this port.</param>
		/// <param name="description"> Description of this port.</param>
		/// <param name="enumConfig">  Enumeration config.</param>
		/// <param name="defaultValue">Default value to use.</param>
		public NativeInputPortConfiguration
			(string name, string displayName, string description, string enumConfig, Native.FlowInputData defaultValue)
		{
			this.Name = Marshal.StringToHGlobalUni(name);
			this.HumanName = Marshal.StringToHGlobalUni(displayName);
			this.Description = Marshal.StringToHGlobalUni(description);
			this.EnumConfig = Marshal.StringToHGlobalUni(enumConfig);
			this.DefaultValue = defaultValue;
		}
	}
	/// <summary>
	/// Output port configuration located in native memory.
	/// </summary>
	public struct NativeOutputPortConfiguration
	{
		/// <summary>
		/// Null-terminated string that represents a name of the port.
		/// </summary>
		public IntPtr Name;
		/// <summary>
		/// Null-terminated string that represents a display name of the port.
		/// </summary>
		public IntPtr HumanName;
		/// <summary>
		/// Null-terminated string that represents a description of the port.
		/// </summary>
		public IntPtr Description;
		/// <summary>
		/// Index of the type of the port in the NTypeList.
		/// </summary>
		public int TypeIndex;
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="name">       Name of the port.</param>
		/// <param name="displayName">Display name of this port.</param>
		/// <param name="description">Description of this port.</param>
		/// <param name="typeIndex">  Index of the type of this port.</param>
		public NativeOutputPortConfiguration(string name, string displayName, string description, int typeIndex)
		{
			this.Name = Marshal.StringToHGlobalUni(name);
			this.HumanName = Marshal.StringToHGlobalUni(displayName);
			this.Description = Marshal.StringToHGlobalUni(description);
			this.TypeIndex = typeIndex;
		}
	}
}