namespace Battleships.Domain
{
	public class ComputerPlayer(int playerNumber, Board? board) : Player(playerNumber, board)
	{
		public override Coordinate MakeGuess()
		{
			throw new NotImplementedException();
		}
	}
}
