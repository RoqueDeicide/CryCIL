using System;
using System.Linq;

using CryEngine.Native;

namespace CryEngine.Flowgraph
{
	public enum NodePortType
	{
		Any = -1,
		Void,
		Int,
		Float,
		EntityId,
		Vec3,
		String,
		Bool
	}
}