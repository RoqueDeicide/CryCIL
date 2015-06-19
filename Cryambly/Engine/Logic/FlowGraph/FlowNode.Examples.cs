using System;
using System.Collections.Generic;
using CryCil.Engine.Data;
using CryCil.Engine.DebugServices;
using CryCil.Utilities;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a flow node that stores a value that can be incremented and decremented and a certain
	/// level can be outputted.
	/// </summary>
	[FlowNode("Counter")]
	public class CounterNode : FlowNode
	{
		#region Fields
		private int count;
		/// <summary>
		/// Gets or sets current count.
		/// </summary>
		public int Count
		{
			get { return this.count; }
			set
			{
				if (this.count == value)
				{
					return;
				}

				this.count = value;
				this.currentCount.Activate(value);
				this.aboveThreshold.Activate(value >= this.threshold.Value);
			}
		}
		// These fields provide convenient access to specific ports without having to make enumerations of
		// indices of each particular port.
		private InputPortInt decrementValue;
		private InputPortInt incrementValue;
		private InputPortVoid decrementTrigger;
		private InputPortVoid incrementTrigger;

		private InputPortInt threshold;
		private InputPortInt minLevel;
		private InputPortInt maxLevel;

		private OutputPortInt currentCount;
		private OutputPortBool aboveThreshold;
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Don't touch signature of this constructor.
		/// </summary>
		/// <param name="id">   Identifier of this node.</param>
		/// <param name="graph">Pointer to the native FlowGraph object.</param>
		public CounterNode(ushort id, IntPtr graph)
			: base(id, graph)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Here we need to specify our node.
		/// </summary>
		public override void Define()
		{
			this.decrementValue =
				new InputPortInt
				(
					"decrementValue",
					"DecrementValue",
					"How much to subtract from the current value when DecrementTrigger port is activated.",
					null,
					1
				);
			this.decrementTrigger =
				new InputPortVoid
				(
					"decrementTrigger",
					"DecrementTrigger",
					"When activated decrements current value by the amount specified by DecrementValue port.",
					Decrement
				);
			this.incrementValue =
				new InputPortInt
				(
					"incrementValue",
					"IncrementValue",
					"How much to add to the current value when IncrementTrigger port is activated.",
					null,
					1
				);
			this.incrementTrigger =
				new InputPortVoid
				(
					"incrementTrigger",
					"IncrementTrigger",
					"When activated increments current value by the amount specified by IncrementValue port.",
					Increment
				);

			this.threshold =
				new InputPortInt
				(
					"thresholdValue",
					"ThresholdValue",
					"Determines the value that is a threshold that when passed passes true to AboveThresholdIndicator port.",
					SetThreshold
				);
			this.minLevel =
				new InputPortInt
				(
					"minLevelValue",
					"MinLevelValue",
					"Determines the value current value cannot go below.",
					SetLevel
				);
			this.maxLevel =
				new InputPortInt
				(
					"maxLevelValue",
					"MaxLevelValue",
					"Determines the value current value cannot go above.",
					SetLevel
				);

			this.currentCount =
				new OutputPortInt
				(
					"currentCountValue",
					"CurrentCountValue",
					"Outputs current counter value."
				);
			this.aboveThreshold =
				new OutputPortBool
				(
					"aboveThresholdIndicator",
					"AboveThresholdIndicator",
					"Outputs boolean value that indicates whether current counter value is above or on the threshold."
				);

			// Now we need to set all 4 of the following properties, not setting even 1 is bad.
			this.Inputs =
				new InputPort[]
				{
					this.decrementValue,
					this.decrementTrigger,
					this.incrementValue,
					this.incrementTrigger,
					this.minLevel,
					this.threshold,
					this.maxLevel
				};
			this.Outputs =
				new OutputPort[]
				{
					this.currentCount,
					this.aboveThreshold
				};
			this.Description =
				"This node is an analog of a Counter gate from Red Power mod for Minecraft, it stores" +
				" an integer value that can be manipulated.";
			this.Flags = FlowNodeFlags.Approved;			// Use at least this flag so the node can show up in the UI.
		}
		/// <summary>
		/// Processes ports that were activated at the same time.
		/// </summary>
		/// <param name="activatedPorts">A collection of ports that were activated.</param>
		public override void MultiActivate(ActivationSet activatedPorts)
		{
			if (activatedPorts.Pop(new InputPort[] { this.minLevel, this.maxLevel }))
			{
				if (this.minLevel.Value > this.maxLevel.Value)
				{
					Log.Error("FlowGraph:CounterNode: Minimal value cannot be higher then maximal value.", true);
				}

				this.Count = MathHelpers.Clamp(this.count, this.minLevel.Value, this.maxLevel.Value);
			}

			if (activatedPorts.Pop(new InputPort[] { this.decrementTrigger, this.incrementTrigger }))
			{
				this.Count += this.incrementValue.Value - this.decrementValue.Value;
			}
		}
		/// <summary>
		/// Save current value to the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>False, if an error has happened, otherwise true.</returns>
		public override bool Save(CryXmlNode node)
		{
			node.SetAttribute("CounterNodeCount", this.count);
			// We can dispose this node now because once this method returns the control will be back to
			// the native code.
			node.Dispose();
			return true;
		}
		/// <summary>
		/// Load current value to the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>False, if an error has happened, otherwise true.</returns>
		public override bool Load(CryXmlNode node)
		{
			node.GetAttribute("CounterNodeCount", out this.count);
			// We can dispose this node now because once this method returns the control will be back to
			// the native code.
			node.Dispose();
			return true;
		}
		/// <summary>
		/// Synchronizes the state of this node.
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronizing(CrySync sync)
		{
			sync.BeginGroup("CounterNodeState");

			sync.Sync("Count", ref this.count);

			sync.EndGroup();
		}
		#endregion
		#region Utilities
		private void Decrement()
		{
			this.Count -= this.decrementValue.Value;
		}
		private void Increment()
		{
			this.Count += this.incrementValue.Value;
		}

		private void SetThreshold(int obj)
		{
			this.aboveThreshold.Activate(this.count >= obj);
		}
		private void SetLevel(int obj)
		{
			if (this.minLevel.Value > this.maxLevel.Value)
			{
				Log.Error("FlowGraph:CounterNode: Minimal value cannot be higher then maximal value.", true);
			}

			this.Count = MathHelpers.Clamp(this.count, this.minLevel.Value, this.maxLevel.Value);
		}
		#endregion
	}
}