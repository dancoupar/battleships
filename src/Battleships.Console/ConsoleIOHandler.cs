using Battleships.Application.Interfaces;

namespace Battleships.Console
{
	public class ConsoleIOHandler : IUserInputHandler, IUserOutputHandler
	{
		public string? PromptForGuess()
		{
			System.Console.Write("Enter coordinate: ");
			return System.Console.ReadLine();
		}

		public void ReportHit()
		{
			System.Console.WriteLine("Hit");
		}

		public void ReportInvalidGuess()
		{
			System.Console.WriteLine("Invalid coordinate");
		}

		public void ReportMiss()
		{
			System.Console.WriteLine("Miss");
		}

		public void ReportShipSunk(string ship)
		{
			System.Console.WriteLine($"Sunk {ship}");
		}

		public void ReportGameOver()
		{
			System.Console.WriteLine("Game over man, game over");
		}
	}
}
