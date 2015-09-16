namespace CryCil.Engine.Data
{
	/// <summary>
	/// Defines interface of objects that can be synchronized.
	/// </summary>
	public interface ISynchronizable
	{
		/// <summary>
		/// Synchronizes this object.
		/// </summary>
		/// <param name="s">An object that serves as synchronization medium.</param>
		void Sync(CrySync s);
	}
}