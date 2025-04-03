using Battleships.Application.Interfaces;
using Battleships.Domain;
using Battleships.Domain.Interfaces;

namespace Battleships.Application
{
	public class GameFactory(GameConfiguration gameConfiguration, IBoardFactory boardFactory, ITurnInputHandler turnInputHandler, IUserOutputHandler userOutputHandler)
	{
		public Game CreateGame()
		{
			Player[] players = new Player[gameConfiguration.NumberOfPlayers];

			for (int i = 0; i < gameConfiguration.NumberOfPlayers; i++)
			{
				// Has a board been specified for this player?
				Board? board = null;
				
				if (gameConfiguration.PlayerBoardSizes.TryGetValue(i, out (int Width, int Height) boardSize))
				{					
					if (!gameConfiguration.PlayerShipTypes.TryGetValue(i, out List<ShipType>? shipTypes))
					{
						throw new InvalidOperationException("A player board was specified, but no ships were specified for that player.");
					}

					board = boardFactory.CreateBoard(boardSize.Width, boardSize.Height, shipTypes);
				}

				int playerNumber = i + 1;

				players[i] = gameConfiguration.PlayerTypes[i] switch
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
