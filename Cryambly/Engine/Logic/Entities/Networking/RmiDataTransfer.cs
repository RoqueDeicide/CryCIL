namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of possible ways RMI data can be transferred when invoking methods.
	/// </summary>
	public enum RmiDataTransfer
	{
		/// <summary>
		/// Specifies that RMI data should be transferred with the entity aspect data and be processed
		/// before it.
		/// </summary>
		/// <remarks>
		/// Methods that use this flag will be invoked before
		/// <see cref="MonoNetEntity.SynchronizeWithNetwork"/> method.
		/// </remarks>
		PreAttach = 1,
		/// <summary>
		/// Specifies that RMI data should be transferred with the entity aspect data and be processed
		/// after it.
		/// </summary>
		/// <remarks>
		/// Methods that use this flag will be invoked after
		/// <see cref="MonoNetEntity.SynchronizeWithNetwork"/> method.
		/// </remarks>
		PostAttach = 2,
		/// <summary>
		/// Specifies that RMI data should be transferred independently from entity aspect data.
		/// </summary>
		/// <remarks>
		/// Methods that use this flag will be invoked either before or after
		/// <see cref="MonoNetEntity.SynchronizeWithNetwork"/> method and there is not way to predict
		/// whether the order of arrival.
		/// </remarks>
		NoAttach = 3,
		/// <summary>
		/// Specifies that RMI data should be transfered at the start of the synchronization frame.
		/// </summary>
		/// <remarks>
		/// Methods that use this flag will be invoked before
		/// <see cref="MonoNetEntity.SynchronizeWithNetwork"/> method is invoked on any of the entities.
		/// </remarks>
		Urgent = 4,
		/// <summary>
		/// Specifies that RMI data should be transfered independently from entity aspect data but unlike
		/// <see cref="NoAttach"/> this one will be sent and not discarded when all RMI data is flushed.
		/// </summary>
		Independent = 5
	}
}