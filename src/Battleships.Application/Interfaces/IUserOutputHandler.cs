namespace Battleships.Application.Interfaces
{
	public interface IUserOutputHandler
	{
		void ReportInvalidGuess();

		void ReportMiss();

		void ReportHit();

		void ReportShipSunk(string ship);

		void ReportGameOver();
	}
}
