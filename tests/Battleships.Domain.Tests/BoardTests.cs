using FluentAssertions;

namespace Battleships.Domain.Tests
{
	public class BoardTests
	{
		public static TheoryData<Coordinate, ShipOrientation, Coordinate, ShipOrientation> OverlappingShipTestData => new()
		{
			{ Coordinate.Parse("A1"), ShipOrientation.Vertical, Coordinate.Parse("A1"), ShipOrientation.Vertical },
			{ Coordinate.Parse("A1"), ShipOrientation.Vertical, Coordinate.Parse("A1"), ShipOrientation.Horizontal },
			{ Coordinate.Parse("A1"), ShipOrientation.Vertical, Coordinate.Parse("A2"), ShipOrientation.Vertical },
			{ Coordinate.Parse("E8"), ShipOrientation.Horizontal, Coordinate.Parse("I4"), ShipOrientation.Vertical },
			{ Coordinate.Parse("E3"), ShipOrientation.Vertical, Coordinate.Parse("A5"), ShipOrientation.Horizontal },
			{ Coordinate.Parse("E3"), ShipOrientation.Vertical, Coordinate.Parse("A5"), ShipOrientation.Horizontal }
		};

		public static TheoryData<Coordinate, ShipOrientation> ShipPlacementBoundaryTestData => new()
		{
			{ Coordinate.Parse("J10"), ShipOrientation.Horizontal },
			{ Coordinate.Parse("J10"), ShipOrientation.Vertical },
			{ Coordinate.Parse("G1"), ShipOrientation.Horizontal },
			{ Coordinate.Parse("A7"), ShipOrientation.Vertical }
		};

		public static TheoryData<Coordinate, bool> CoordinateBoundaryTestData => new()
		{
			{ Coordinate.Parse("A1"), true },
			{ Coordinate.Parse("J10"), true },
			{ Coordinate.Parse("K1"), false },
			{ Coordinate.Parse("A11"), false }
		};

		[Theory]
		[MemberData(nameof(ShipPlacementBoundaryTestData))]
		public void A_ship_can_only_be_placed_if_within_bounds(Coordinate startCoord, ShipOrientation orientation)
		{
			// Arrange
			var sut = new Board(10, 10);

			// Act
			Action act = () =>
			{
				sut.PlaceShip(new Battleship(), startCoord, orientation);
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();			
		}

		[Fact]
		public void A_ship_can_only_be_placed_on_a_board_once()
		{
			// Arrange
			var sut = new Board(10, 10);
			var ship = new Battleship();
			sut.PlaceShip(ship, Coordinate.Parse("A1"), ShipOrientation.Vertical);

			// Act
			Action act = () =>
			{
				sut.PlaceShip(ship, Coordinate.Parse("A1"), ShipOrientation.Vertical);
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}

		[Theory]
		[MemberData(nameof(OverlappingShipTestData))]
		public void Ships_cannot_overlap_one_another(
			Coordinate firstShipStartCoord,
			ShipOrientation firstShipOrientation,
			Coordinate secondShipStartCoord,
			ShipOrientation secondShipOrientation)
		{
			// Arrange
			var sut = new Board(10, 10);
			sut.PlaceShip(new Battleship(), firstShipStartCoord, firstShipOrientation);

			// Act
			Action act = () =>
			{
				sut.PlaceShip(new Battleship(), secondShipStartCoord, secondShipOrientation);
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void It_can_be_checked_whether_a_coordinate_is_occupied()
		{
			// Arrange
			var sut = new Board(10, 10);
			sut.PlaceShip(new Battleship(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);

			// Act
			// Assert
			var result = sut.IsCoordinateOccupied(Coordinate.Parse("A1"));
			result.Should().BeTrue();

			result = sut.IsCoordinateOccupied(Coordinate.Parse("J10"));
			result.Should().BeFalse();
		}

		[Theory]
		[MemberData(nameof(CoordinateBoundaryTestData))]
		public void It_can_be_checked_whether_a_coordinate_is_in_bounds(Coordinate coord, bool expectedResult)
		{
			// Arrange
			var sut = new Board(10, 10);

			// Act
			bool result = sut.IsCoordinateInBounds(coord);

			// Assert
			result.Should().Be(expectedResult);
		}
	}
}
