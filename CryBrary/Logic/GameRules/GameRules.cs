namespace CryEngine.Logic.GameRules
{
	/// <summary>
	/// This is the base GameRules interface. All game rules must implement this.
	/// </summary>
	public abstract class GameRules
	{
		/// <summary>
		/// Gets the currently active game rules instance.
		/// </summary>
		public static GameRules Current { get; internal set; }
	}
}