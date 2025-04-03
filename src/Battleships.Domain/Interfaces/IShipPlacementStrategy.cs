namespace Battleships.Domain.Interfaces
{
	public interface IShipPlacementStrategy
	{
		void PlaceShips(Board board, IEnumerable<Ship> ships);
	}
}
