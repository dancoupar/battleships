namespace Battleships.Domain.Interfaces
{
	public interface IBoardFactory
	{
		Board CreateBoard(int boardWidth, int boardHeight, IEnumerable<ShipType> shipTypes);
	}
}
