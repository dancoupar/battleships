using FluentAssertions;

namespace Battleships.Domain.Tests
{
	public class ShipTests
	{
		[Fact]	
		public void Battleships_occupy_5_coordinates()
		{
			// Arrange
			var sut = new Battleship();

			// Act
			int result = sut.Length;

			// Assert
			result.Should().Be(5);
		}

		[Fact]
		public void Destroyers_occupy_4_coordinates()
		{
			// Arrange
			var sut = new Destroyer();

			// Act
			int result = sut.Length;

			// Assert
			result.Should().Be(4);
		}		

		[Fact]
		public void A_ship_can_be_placed()
		{
			// Arrange
			var sut = new Battleship();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4"),
				Coordinate.Parse("A5"),
			};

			// Act
			sut.Place(coordinates, ShipOrientation.Vertical);

			// Assert
			sut.Coordinates.Should().BeEquivalentTo(coordinates);
			sut.HasBeenPlaced().Should().BeTrue();
			sut.IsSunk().Should().BeFalse();
		}

		[Fact]
		public void Coordinates_must_match_the_length_of_the_ship_when_placing()
		{
			// Arrange
			var sut = new Battleship();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4")
			};

			// Act
			Action act = () =>
			{
				sut.Place(coordinates, ShipOrientation.Vertical);
			};

			// Assert
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void Coordinates_must_be_contiguous_when_placing()
		{
			// Arrange
			var sut = new Battleship();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A4"),
				Coordinate.Parse("A5"),
				Coordinate.Parse("A6")
			};

			// Act
			Action act = () =>
			{
				sut.Place(coordinates, ShipOrientation.Vertical);
			};

			// Assert
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void Coordinates_must_match_the_orientation_when_placing()
		{
			// Arrange
			var sut = new Battleship();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4"),
				Coordinate.Parse("A5")
			};

			// Act
			Action act = () =>
			{
				sut.Place(coordinates, ShipOrientation.Horizontal);
			};

			// Assert
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void A_ship_that_has_not_been_placed_has_no_coordinates_or_orientation()
		{
			// Arrange
			var sut = new Battleship();

			// Act			
			// Assert
			sut.Coordinates.Should().BeNull();
			sut.Orientation.Should().BeNull();
			sut.HasBeenPlaced().Should().BeFalse();			
		}

		[Fact]
		public void A_ship_can_be_hit_by_an_opponent()
		{
			// Arrange
			var sut = new Destroyer();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4")
			};

			sut.Place(coordinates, ShipOrientation.Vertical);

			// Act
			sut.RegisterHit(Coordinate.Parse("A1"));

			// Assert
			sut.Coordinates!.Count.Should().Be(3);
		}

		[Fact]
		public void A_hit_cannot_be_registered_if_a_ship_has_not_been_placed()
		{
			// Arrange
			var sut = new Destroyer();

			// Act
			Action act = () =>
			{
				sut.RegisterHit(Coordinate.Parse("A1"));
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void A_hit_cannot_be_registered_if_a_ship_does_not_occupy_that_coordinate()
		{
			// Arrange
			var sut = new Destroyer();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4")
			};

			sut.Place(coordinates, ShipOrientation.Vertical);

			// Act
			Action act = () =>
			{
				sut.RegisterHit(Coordinate.Parse("B1"));
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void A_ship_can_be_sunk_by_an_opponent()
		{
			// Arrange
			var sut = new Destroyer();
			var coordinates = new List<Coordinate>()
			{
				Coordinate.Parse("A1"),
				Coordinate.Parse("A2"),
				Coordinate.Parse("A3"),
				Coordinate.Parse("A4")
			};

			sut.Place(coordinates, ShipOrientation.Vertical);

			// Act
			// Assert
			sut.RegisterHit(Coordinate.Parse("A1"));
			sut.IsSunk().Should().BeFalse();

			sut.RegisterHit(Coordinate.Parse("A2"));
			sut.IsSunk().Should().BeFalse();

			sut.RegisterHit(Coordinate.Parse("A3"));
			sut.IsSunk().Should().BeFalse();

			sut.RegisterHit(Coordinate.Parse("A4"));
			sut.IsSunk().Should().BeTrue();
			sut.Coordinates.Should().BeEmpty();
		}		
	}
}
