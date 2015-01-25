using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Extensions;
using CryEngine.Initialization;

namespace CryEngine.RunTime.Registration
{
	/// <summary>
	/// Handles registration of flow nodes.
	/// </summary>
	public class FlowNodeRegister
	{
		internal static readonly SortedList<string, Type> FlowNodeTypes = new SortedList<string, Type>();
		/// <summary>
		/// Prepares registration data for a FlowGraph node.
		/// </summary>
		/// <param name="type">Type of the FlowGraph node to register.</param>
		public static void Prepare(Type type)
		{
			FlowNodeRegister.FlowNodeTypes.Add(type.Name, type);
		}
		/// <summary>
		/// Registers all FlowGraph nodes.
		/// </summary>
		public static void RegisterTypes()
		{
			foreach (string flowNodeTypeName in FlowNodeTypes.Keys)
			{
				Native.FlowNodeInterop.RegisterNode(flowNodeTypeName);
			}
		}
	}
}