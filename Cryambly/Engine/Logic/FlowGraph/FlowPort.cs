using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for flow node ports.
	/// </summary>
	public abstract class FlowPort : IComparable<FlowPort>, IEquatable<FlowPort>
	{
		#region Fields
		/// <summary>
		/// Internal name of the port.
		/// </summary>
		public readonly string Name;
		/// <summary>
		/// Display name of the port.
		/// </summary>
		public readonly string DisplayName;
		/// <summary>
		/// Description of the port.
		/// </summary>
		public readonly string Description;
		/// <summary>
		/// Type of the data that can be transfered via this port.
		/// </summary>
		public readonly FlowDataType DataType;
		#endregion
		#region Properties
		// Gets description of this port for the CryEngine flow system.
		internal abstract FlowPortConfig Config { get; }
		internal byte PortId { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Initializes common fields of objects that are represented by classes that derive from this one.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		/// <param name="dataType">   Type of the data that can be transfered via this port.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		protected FlowPort(string name, string displayName, string description, FlowDataType dataType)
		{
			if (!name.IsValidFlowGraphName())
			{
				throw new ArgumentException(
					"Given name is not valid for flow node port, because it's not valid for Xml attribute name.");
			}
			this.Name = name;
			this.DisplayName = displayName;
			this.Description = description;
			this.DataType = dataType;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Compares this port to another.
		/// </summary>
		/// <param name="other">Another port.</param>
		/// <returns>Position of this port relative to another in the list of ports in the node.</returns>
		public int CompareTo(FlowPort other)
		{
			return this.PortId.CompareTo(other.PortId);
		}
		/// <summary>
		/// Compares this port to another.
		/// </summary>
		/// <param name="other">Another port.</param>
		/// <returns>True, these 2 ports can be identified as 1.</returns>
		public bool Equals(FlowPort other)
		{
			return this.PortId == other.PortId;
		}
		#endregion
		#region Utilities
		#endregion
	}
}