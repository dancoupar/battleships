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
				new HumanPlayer(new Board(10, 10)),
				new HumanPlayer(new Board(10, 10))
			];

			int turnNumber = 0;
			var sut = Game.CreateNew(players);
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
				new HumanPlayer(player1Board),
				new HumanPlayer(player2Board)
			];

			int roundNumber = 0;
			var sut = Game.CreateNew(players);
			sut.HumanPlayerTurnRequested += Sut_HumanPlayerTurnRequested;
			sut.PlayerWon += Sut_PlayerWon;
			Player? winningPlayer = null;

			// Act
			sut.Start();

			// Assert
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

			winningPlayer.Should().Be(players[0]);
		}		
	}
}
