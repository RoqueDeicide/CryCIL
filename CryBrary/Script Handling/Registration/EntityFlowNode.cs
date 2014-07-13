using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
	public struct EntityFlowNodeRegistrationParams : FlowNodeBaseRegistrationParams
	{
		public string entityName;

		public InputPortConfig[] InputPorts { get; set; }
		public MethodInfo[] InputMethods { get; set; }

		public OutputPortConfig[] OutputPorts { get; set; }
		public MemberInfo[] OutputMembers { get; set; }
	}
}