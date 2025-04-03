using Battleships.Application.Interfaces;
using Battleships.Domain;
using Battleships.Domain.Interfaces;

namespace Battleships.Application
{
	/// <summary>
	/// Represents a factory for creating new games.
	/// </summary>
	/// <remarks>
	/// Responsible for creating games, players and boards, and wiring up events.
	/// </remarks>
	/// <param name="boardFactory">A factory for creating game boards.</param>
	/// <param name="turnInputHandler">An object for handling user input when taking a turn.</param>
	/// <param name="userOutputHandler">An object for handling user output.</param>
	public class GameFactory(IBoardFactory boardFactory, ITurnInputHandler turnInputHandler, IUserOutputHandler userOutputHandler) : IGameFactory
	{
		public Game CreateGame(GameConfiguration gameConfiguration)
		{
			Player[] players = new Player[gameConfiguration.NumberOfPlayers];

			for (int i = 0; i < gameConfiguration.NumberOfPlayers; i++)
			{
				int playerNumber = i + 1;

				// Has a board been specified for this player?
				Board? board = null;
				
				if (gameConfiguration.PlayerBoardSizes.TryGetValue(playerNumber, out (int Width, int Height) boardSize))
				{					
					if (!gameConfiguration.PlayerShipTypes.TryGetValue(playerNumber, out List<ShipType>? shipTypes))
					{
						throw new InvalidOperationException("A player board was specified, but no ships were specified for that player.");
					}

					board = boardFactory.CreateBoard(boardSize.Width, boardSize.Height, shipTypes);
				}

				if (!gameConfiguration.PlayerTypes.TryGetValue(playerNumber, out PlayerType playerType))
				{
					throw new InvalidOperationException($"No player type was specified for player {playerNumber}.");
				}

				players[i] = playerType switch
				{
					PlayerType.Human => new HumanPlayer(playerNumber, board),
					PlayerType.Computer => new ComputerPlayer(playerNumber, board),
					_ => throw new NotSupportedException()
				};
			}

			Game game = Game.CreateNew(players);
			turnInputHandler.WireUp(game);
			
			game.ShipHit += Game_ShipHit;
			game.ShipMissed += Game_ShipMiss;
			game.ShipSunk += Game_ShipSunk;
			game.PlayerWon += Game_PlayerWon;

			return game;
		}		

		private void Game_ShipMiss(object? sender, EventArgs e)
		{
			userOutputHandler.ReportMiss();
		}

		private void Game_ShipHit(object? sender, EventArgs e)
		{
			userOutputHandler.ReportHit();
		}

		private void Game_ShipSunk(object? sender, Ship ship)
		{
			userOutputHandler.ReportShipSunk(ship.Name);
		}

		private void Game_PlayerWon(object? sender, Player e)
		{
			userOutputHandler.ReportGameOver();
		}
	}
}
