using CryCil.Engine.Data;
using CryCil.RunTime.Registration;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for object that handle transfer of data that is used in remote invocation of methods.
	/// </summary>
	public abstract class RmiParameters
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Synchronizes the RMI data.
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		[RuntimeInvoke("Invoked from underlying object to send/receive RMI arguments.")]
		protected abstract void Synchronize(CrySync sync);
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked by underlying framework to acquire a default object of the type that derives from this" +
						" class to receive incoming RMI data.")]
		private static RmiParameters AcquireReceptor(string name)
		{
			return RmiParametersRegistry.AcquireReceptor(name);
		}
		#endregion
	}
}