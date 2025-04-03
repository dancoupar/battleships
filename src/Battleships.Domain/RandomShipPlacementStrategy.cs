using Battleships.Domain.Interfaces;

namespace Battleships.Domain
{
	public class RandomShipPlacementStrategy(IRandomNumberGenerator randomNumberGenerator) : IShipPlacementStrategy
	{
		public void PlaceShips(Board board, IEnumerable<Ship> ships)
		{
			foreach (Ship ship in ships)
			{
				List<(Coordinate, ShipOrientation)> validStartPositions = CalculateAllValidStartPositions(board, ship);

				if (validStartPositions.Count == 0)
				{
					throw new ApplicationException("There are no remaining valid positions for this ship.");
				}

				(Coordinate, ShipOrientation) startPosition = PickRandomStartPosition(validStartPositions);
				board.PlaceShip(ship, startPosition.Item1, startPosition.Item2);
			}
		}

		private static List<(Coordinate, ShipOrientation)> CalculateAllValidStartPositions(Board board, Ship ship)
		{
			var validPositions = new List<(Coordinate, ShipOrientation)>();

			for (char column = 'A'; column < 'A' + board.Width; column++)
			{
				for (int row = 1; row <= board.Height; row++)
				{
					var startCoord = new Coordinate(column, row);

					if (board.CanPlaceShip(ship, startCoord, ShipOrientation.Horizontal))
					{
						validPositions.Add((startCoord, ShipOrientation.Horizontal));
					}

					if (board.CanPlaceShip(ship, startCoord, ShipOrientation.Vertical))
					{
						validPositions.Add((startCoord, ShipOrientation.Vertical));
					}
				}
			}

			return validPositions;
		}

		private (Coordinate, ShipOrientation) PickRandomStartPosition(List<(Coordinate, ShipOrientation)> validStartPositions)
		{
			return validStartPositions[randomNumberGenerator.GetRandomNumber(0, validStartPositions.Count)];
		}
	}
}
