using System;
using CryCil.Engine.Logic;
using GameLibrary.Entities;

namespace GameLibrary.FlowNodes
{
	/// <summary>
	/// Represents a flow node that targets an entity and is configured by it.
	/// </summary>
	[FlowNode("entity:GeomEntity",
		"Can be used to manipulate entities of type GeometricEntity.",
		FlowNodeFlags.Approved | FlowNodeFlags.TargetEntity | FlowNodeFlags.HideUi)]
	public class GeometricEntityNode : FlowNode
	{
		#region Fields
		private GeometricEntity target;
		private readonly OutputPortBool donePort;
		#endregion
		#region Properties
		
		#endregion
		#region Construction
		/// <summary>
		/// Initializes base properties of the new flow node wrapper object.
		/// </summary>
		/// <param name="id">   Identifier of the node.</param>
		/// <param name="graph">
		/// Pointer to the native object that represents the FlowGraph where this node is located.
		/// </param>
		public GeometricEntityNode(ushort id, IntPtr graph)
			: base(id, graph)
		{
			this.donePort = new OutputPortBool("Done", "Done", "Activates and send boolean value of [true] when " +
															   "geometry is loaded via LoadGeometry input.");
		}
		#endregion
		#region Interface
		/// <summary>
		/// Defines a list of specific properties of this flow node.
		/// </summary>
		public override void Define()
		{
			this.Inputs = new InputPort[1];
			this.Inputs[0] = new InputPortString("LoadGeometry", "Load Geometry",
												 "Specifies the path to the geometric object to " +
												 "use to represent the target entity.",
												 this.LoadGeometry, "editor/objects/sphere.cgf");

			this.Outputs = new OutputPort[1];
			this.Outputs[0] = this.donePort;

			this.EntityTargeted += this.OnEntityTargeted;
		}
		#endregion
		#region Utilities
		private void LoadGeometry(string s)
		{
			if (this.target != null)
			{
				this.target.ObjectModel = s;

				this.donePort.Activate(true);
			}
		}
		private void OnEntityTargeted(FlowNode flowNode, EntityId entityId)
		{
			if (entityId == default(EntityId))
			{
				this.target = null;
			}
			else
			{
				CryEntity cryEntity = entityId.Entity;
				this.target = cryEntity.ManagedEntity as GeometricEntity;
			}
		}
		#endregion
	}
}