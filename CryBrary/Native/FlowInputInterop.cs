using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Entities;
using CryEngine.Logic.Entities;
using CryEngine.Mathematics;

namespace CryEngine.Native
{
	public struct FlowInputData
	{
		public IntPtr VTable;
		public IntPtr Data;
	}
	internal static class FlowInputInterop
	{
		internal static extern FlowInputData CreateVoid();
		internal static extern FlowInputData CreateInt(int value);
		internal static extern FlowInputData CreateFloat(float value);
		internal static extern FlowInputData CreateString(string value);
		internal static extern FlowInputData CreateVector(Vector3 value);
		internal static extern FlowInputData CreateEntityId(EntityId value);
		internal static extern FlowInputData CreateBool(bool value);
		internal static extern FlowInputData CreateAny();
	}
}