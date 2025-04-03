using Battleships.Domain;

namespace Battleships.Application.Interfaces
{
	/// <summary>
	/// Describes a factory for creating new games.
	/// </summary>
	public interface IGameFactory
	{
		/// <summary>
		/// Creates and returns a new game with the specified configuration.
		/// </summary>
		/// <param name="gameConfiguration">The game configuration.</param>
		/// <returns>The newly created game, unstarted.</returns>
		Game CreateGame(GameConfiguration gameConfiguration);
	}
}
