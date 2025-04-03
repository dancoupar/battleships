using FluentAssertions;
using FluentAssertions.Events;

namespace Battleships.Domain.Tests
{
	public class PlayerTests
	{
		[Fact]
		public void A_player_that_does_not_have_a_board_cannot_respond_to_guesses()
		{
			// Arrange
			var sut = new HumanPlayer(null);

			// Act
			Action act = () =>
			{
				sut.RespondToGuess(Coordinate.Parse("A1"));
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}

		[Fact]
		public void A_player_reports_a_hit_on_a_correct_guess()
		{
			// Arrange
			var board = new Board(10, 10);
			var sut = new HumanPlayer(board);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);
			IMonitor<HumanPlayer> monitor = sut.Monitor();

			// Act
			sut.RespondToGuess(Coordinate.Parse("A1"));

			// Assert
			monitor.Should().Raise(nameof(sut.ShipHit));
			monitor.Should().NotRaise(nameof(sut.ShipMissed));
			monitor.Should().NotRaise(nameof(sut.ShipSunk));
		}

		[Fact]
		public void A_player_reports_a_miss_on_an_incorrect_guess()
		{
			// Arrange
			var board = new Board(10, 10);
			var sut = new HumanPlayer(board);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);
			IMonitor<HumanPlayer> monitor = sut.Monitor();

			// Act
			sut.RespondToGuess(Coordinate.Parse("J10"));

			// Assert
			monitor.Should().Raise(nameof(sut.ShipMissed));
			monitor.Should().NotRaise(nameof(sut.ShipHit));
			monitor.Should().NotRaise(nameof(sut.ShipSunk));
		}

		[Fact]
		public void A_player_reports_a_when_their_ship_has_been_sunk()
		{
			// Arrange
			var board = new Board(10, 10);
			var sut = new HumanPlayer(board);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);
			IMonitor<HumanPlayer> monitor = sut.Monitor();

			// Act
			sut.RespondToGuess(Coordinate.Parse("A1"));
			sut.RespondToGuess(Coordinate.Parse("B1"));
			sut.RespondToGuess(Coordinate.Parse("C1"));
			sut.RespondToGuess(Coordinate.Parse("D1"));

			// Assert			
			monitor.Should().Raise(nameof(sut.ShipSunk));
			monitor.Should().NotRaise(nameof(sut.ShipMissed));
		}

		[Fact]
		public void A_player_can_count_the_number_of_ships_they_have_remaining()
		{
			// Arrange
			var board = new Board(10, 10);
			var sut = new HumanPlayer(board);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A2"), ShipOrientation.Horizontal);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A3"), ShipOrientation.Horizontal);

			// Act
			var result = sut.CountNumberOfShipsRemaining();

			// Assert
			result.Should().Be(3);
		}

		[Fact]
		public void The_number_of_ships_remaining_reduces_by_one_when_a_ship_is_sunk()
		{
			// Arrange
			var board = new Board(10, 10);
			var sut = new HumanPlayer(board);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Horizontal);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A2"), ShipOrientation.Horizontal);
			board.PlaceShip(new Destroyer(), Coordinate.Parse("A3"), ShipOrientation.Horizontal);

			sut.RespondToGuess(Coordinate.Parse("A1"));
			sut.RespondToGuess(Coordinate.Parse("B1"));
			sut.RespondToGuess(Coordinate.Parse("C1"));
			sut.RespondToGuess(Coordinate.Parse("D1"));

			// Act
			var result = sut.CountNumberOfShipsRemaining();

			// Assert
			result.Should().Be(2);
		}
	}
}
