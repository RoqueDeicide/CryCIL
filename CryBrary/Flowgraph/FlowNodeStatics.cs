using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Initialization;

using CryEngine.Native;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	public abstract partial class FlowNode
	{
		internal static void Register(string typeName)
		{
			FlowNodeInterop.RegisterNode(typeName);
		}

		internal static void InternalRemove(int scriptId)
		{
			ScriptManager.Instance.RemoveInstances(ScriptType.FlowNode, instance =>
			{
				var node = instance as FlowNode;
				if (node != null && node.ScriptId == scriptId)
				{
					node.OnRemove();

					return true;
				}

				return false;
			});
		}
	}
}