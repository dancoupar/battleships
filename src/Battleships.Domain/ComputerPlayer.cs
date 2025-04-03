namespace Battleships.Domain
{
	public class ComputerPlayer(Board? board) : Player(board)
	{
		public override Coordinate MakeGuess()
		{
			throw new NotImplementedException();
		}
	}
}
