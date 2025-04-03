using Battleships.Domain;

namespace Battleships.Application
{
	/// <summary>
	/// Represents a battleship game configuration.
	/// </summary>
	public class GameConfiguration
	{
		/// <summary>
		/// The number of players who'll be participating in the game.
		/// </summary>
		public int NumberOfPlayers { get; init; }

		/// <summary>
		/// The type of each player in the game, keyed by player number.
		/// </summary>
		public required Dictionary<int, PlayerType> PlayerTypes { get; init; }

		/// <summary>
		/// For players who will have a game board, the dimensions of those boards, keyed by player number.
		/// </summary>
		public required Dictionary<int, (int Width, int Height)> PlayerBoardSizes { get; init; }

		/// <summary>
		/// For players who will have a game board, the ships to place on those boards, keyed by player number.
		/// </summary>
		public required Dictionary<int, List<ShipType>> PlayerShipTypes { get; init; }
	}
}
