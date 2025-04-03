using Battleships.Domain;

namespace Battleships.Application
{
	public class GameConfiguration
	{
		public int NumberOfPlayers { get; init; }

		public required Dictionary<int, PlayerType> PlayerTypes { get; init; }

		public required Dictionary<int, (int Width, int Height)> PlayerBoardSizes { get; init; }

		public required Dictionary<int, List<ShipType>> PlayerShipTypes { get; init; }
	}
}
