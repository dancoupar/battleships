namespace Battleships.Domain
{
	/// <summary>
	/// Represents a computer/non-human player.
	/// </summary>
	/// <param name="playerNumber">The player number.</param>
	/// <param name="board">The player's game board.</param>
	public class ComputerPlayer(int playerNumber, Board? board) : Player(playerNumber, board)
	{
		public override Coordinate MakeGuess()
		{
			throw new NotImplementedException();
		}
	}
}
