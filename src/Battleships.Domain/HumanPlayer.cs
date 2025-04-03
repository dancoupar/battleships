namespace Battleships.Domain
{
	public class HumanPlayer(Board? board) : Player(board)
	{
		public event Func<HumanPlayer, Coordinate>? TurnRequested;

		public override Coordinate MakeGuess()
		{
			return this.OnTurnRequested();
		}

		private Coordinate OnTurnRequested()
		{
			return TurnRequested?.Invoke(this) ?? throw new ApplicationException("Unable to request human player input; no handler has been wired up.");
		}
	}
}
