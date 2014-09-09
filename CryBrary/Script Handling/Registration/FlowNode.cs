using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
	public interface IFlowNodeBaseRegistrationParams : IScriptRegistrationParams
	{
		InputPortConfig[] InputPorts { get; }
		MethodInfo[] InputMethods { get; }

		OutputPortConfig[] OutputPorts { get; }
		MemberInfo[] OutputMembers { get; }
	}

	public struct FlowNodeRegistrationParams : IFlowNodeBaseRegistrationParams
	{
		public string name;
		public string category;

		public string description;

		public FlowNodeFilter filter;

		public FlowNodeCloneType type;

		public bool hasTargetEntity;

		public InputPortConfig[] InputPorts { get; set; }
		public MethodInfo[] InputMethods { get; set; }

		public OutputPortConfig[] OutputPorts { get; set; }
		public MemberInfo[] OutputMembers { get; set; }
	}
}