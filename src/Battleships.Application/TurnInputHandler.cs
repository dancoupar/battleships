using Battleships.Application.Interfaces;
using Battleships.Domain;

namespace Battleships.Application
{
	public class TurnInputHandler(IUserInputHandler userInputHandler, IUserOutputHandler userOutputHandler) : ITurnInputHandler
	{
		public void WireUp(Game game)
		{
			game.HumanPlayerTurnRequested += Game_HumanPlayerTurnRequested;
			game.GuessOutOfBounds += Game_GuessOutOfBounds;
		}

		private void Game_GuessOutOfBounds(object? sender, EventArgs e)
		{
			userOutputHandler.ReportInvalidGuess();
		}

		private Coordinate Game_HumanPlayerTurnRequested(HumanPlayer player, Player opponent)
		{
			return this.PromptForGuess();
		}

		private Coordinate PromptForGuess()
		{
			string? input = userInputHandler.PromptForGuess();

			if (Coordinate.TryParse(input, out Coordinate result))
			{
				return result;
			}

			userOutputHandler.ReportInvalidGuess();
			return this.PromptForGuess();
		}
	}
}
