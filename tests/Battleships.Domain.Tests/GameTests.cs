using FluentAssertions;

namespace Battleships.Domain.Tests
{
	public class GameTests
	{
		[Fact]
		public void Players_take_turns()
		{
			// Arrange
			Player[] players =
			[
				new HumanPlayer(1, new Board(10, 10)),
				new HumanPlayer(2, new Board(10, 10))
			];
			
			var sut = Game.CreateNew(players);
			int turnNumber = 0;
			sut.HumanPlayerTurnRequested += Sut_HumanPlayerTurnRequested;

			// Act
			sut.Start();

			// Assert
			Coordinate Sut_HumanPlayerTurnRequested(HumanPlayer currentPlayer, Player opponent)
			{
				currentPlayer.Should().Be(players[turnNumber]);
				turnNumber++;

				if (turnNumber == 1)
				{
					// End game early
					sut.EndGame();
				}

				return Coordinate.Parse("A1");
			}
		}

		[Fact]
		public void A_game_continues_until_one_player_has_all_their_ships_sunk()
		{
			// Arrange
			var player1Board = new Board(10, 10);
			player1Board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Vertical);

			var player2Board = new Board(10, 10);
			player2Board.PlaceShip(new Destroyer(), Coordinate.Parse("A1"), ShipOrientation.Vertical);

			Player[] players =
			[
				new HumanPlayer(1, player1Board),
				new HumanPlayer(2, player2Board)
			];
			
			var sut = Game.CreateNew(players);
			int roundNumber = 0;
			sut.HumanPlayerTurnRequested += Sut_HumanPlayerTurnRequested;
			sut.PlayerWon += Sut_PlayerWon;
			Player? winningPlayer = null;

			// Act
			sut.Start();

			Coordinate Sut_HumanPlayerTurnRequested(HumanPlayer currentPlayer, Player opponent)
			{
				if (currentPlayer == players[0])
				{
					roundNumber++;
				}

				return new Coordinate('A', roundNumber);
			}

			void Sut_PlayerWon(object? sender, Player e)
			{
				winningPlayer = e;
			}

			// Assert
			winningPlayer.Should().Be(players[0]);
		}

		[Fact]
		public void A_player_is_asked_to_make_another_guess_if_their_guess_was_not_in_bounds()
		{
			// Arrange
			Player[] players =
			[
				new HumanPlayer(1, new Board(10, 10)),
				new HumanPlayer(2, new Board(10, 10)),
			];

			var sut = Game.CreateNew(players);
			int turnRequestNumber = 0;
			sut.HumanPlayerTurnRequested += Sut_HumanPlayerTurnRequested;

			// Act
			sut.Start();

			Coordinate Sut_HumanPlayerTurnRequested(HumanPlayer currentPlayer, Player opponent)
			{
				currentPlayer.Should().Be(players[0]);
				turnRequestNumber++;

				if (turnRequestNumber == 1)
				{
					return Coordinate.Parse("Z1");
				}
				else
				{
					sut.EndGame();
					return Coordinate.Parse("A1");
				}
			}

			// Assert
			turnRequestNumber.Should().Be(2);
		}

		[Fact]
		public void At_least_two_players_are_required_to_start_a_game()
		{
			// Arrange
			Player[] players =
			[
				new HumanPlayer(1, new Board(10, 10))
			];

			// Act
			Action act = () =>
			{
				var sut = Game.CreateNew(players);
			};

			// Assert
			act.Should().Throw<InvalidOperationException>();
		}
	}
}
