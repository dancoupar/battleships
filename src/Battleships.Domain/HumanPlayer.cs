namespace Battleships.Domain
{
	/// <summary>
	/// Represents a human player.
	/// </summary>
	/// <param name="playerNumber">The player number.</param>
	/// <param name="board">The player's game board.</param>
	public class HumanPlayer(int playerNumber, Board? board) : Player(playerNumber, board)
	{
		/// <summary>
		/// Raised when the player is requested to take their turn and make a guess.
		/// </summary>
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
