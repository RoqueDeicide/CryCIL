using CryCil.Utilities;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that can handle symbolic input events.
	/// </summary>
	/// <param name="symbol">Symbol that has been entered.</param>
	/// <returns>
	/// True, if this method must be the last method to handle the event. If <c>true</c> is returned
	/// propagation of the event will stop.
	/// </returns>
	public delegate bool SymbolicInputHandler(Utf32Char symbol);
}