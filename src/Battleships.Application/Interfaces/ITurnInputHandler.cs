using Battleships.Domain;

namespace Battleships.Application.Interfaces
{
	/// <summary>
	/// Describes an object for handling user input when taking a turn.
	/// </summary>
	public interface ITurnInputHandler
	{
		/// <summary>
		/// Wires up events relating to turn user input handling.
		/// </summary>
		/// <param name="game">The current game instance.</param>
		void WireUp(Game game);
	}
}
