using System.Collections.Generic;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a special collection that contains input ports that were activated and values that were
	/// assigned to them.
	/// </summary>
	public class ActivationSet
	{
		internal readonly SortedList<InputPort, bool> ActivatedPorts;
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="activatedPorts">A collection of activated ports.</param>
		internal ActivationSet(SortedList<InputPort, bool> activatedPorts)
		{
			this.ActivatedPorts = activatedPorts;
		}
		/// <summary>
		/// Determines whether a port was activated without marking it as processed.
		/// </summary>
		/// <param name="port">Port to check.</param>
		/// <returns>True, if port was activated, otherwise false.</returns>
		public bool Peek(InputPort port)
		{
			return this.ActivatedPorts.ContainsKey(port);
		}
		/// <summary>
		/// Determines whether a port was activated and marks it as processed if it was.
		/// </summary>
		/// <param name="port">Port to check.</param>
		/// <returns>True, if port was activated, otherwise false.</returns>
		public bool Pop(InputPort port)
		{
			if (this.ActivatedPorts.ContainsKey(port))
			{
				this.ActivatedPorts[port] = true;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Determines whether specified ports were activated and marks them as processed if they all were.
		/// </summary>
		/// <param name="ports">Ports to check.</param>
		/// <returns>True, if all ports were activated, otherwise false.</returns>
		public bool Pop(InputPort[] ports)
		{
			if (ports.All(this.ActivatedPorts.ContainsKey))
			{
				ports.ForEach(port => this.ActivatedPorts[port] = true);
				return true;
			}
			return false;
		}
	}
}