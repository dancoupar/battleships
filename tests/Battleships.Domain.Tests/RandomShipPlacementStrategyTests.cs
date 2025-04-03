using Battleships.Tests.Common;
using FluentAssertions;

namespace Battleships.Domain.Tests
{
	public class RandomShipPlacementStrategyTests
	{
		[Fact]
		public void All_ships_are_placed()
		{
			// Arrange
			var sut = new RandomShipPlacementStrategy(new TestRandomNumberGenerator());
			var ships = new List<Ship>()
			{
				new Battleship(),
				new Battleship(),
				new Destroyer(),
				new Destroyer(),
				new Destroyer()
			};

			var board = new Board(10, 10);

			// Act
			sut.PlaceShips(board, ships);

			// Assert
			board.Ships.Should().BeEquivalentTo(ships);
			board.Ships.Should().AllSatisfy(s => s.HasBeenPlaced().Should().BeTrue());
		}

		[Fact]
		public void All_coordinates_have_a_chance_of_being_occupied()
		{
			// Arrange
			var sut = new RandomShipPlacementStrategy(new TestRandomNumberGenerator());
			var boardWidth = 10;
			var boardHeight = 10;
						
			var occupyFrequencies = new Dictionary<Coordinate, int>();

			for (char column = 'A'; column < 'A' + boardWidth; column++)
			{
				for (int row = 1; row <= boardHeight; row++)
				{
					occupyFrequencies[new Coordinate(column, row)] = 0;
				}
			}

			var ships = new List<Ship>()
			{
				new Battleship(),
				new Destroyer(),
				new Destroyer()
			};

			// Act
			for (int i = 0; i < 1000; i++)
			{
				var board = new Board(boardWidth, boardHeight);
				sut.PlaceShips(board, ships);

				foreach (Coordinate coord in occupyFrequencies.Keys)
				{
					if (board.IsCoordinateOccupied(coord))
					{
						occupyFrequencies[coord]++;
					}
				}
			}

			// Assert
			occupyFrequencies.Should().AllSatisfy(f => f.Value.Should().BeGreaterThan(0));
		}
	}
}
