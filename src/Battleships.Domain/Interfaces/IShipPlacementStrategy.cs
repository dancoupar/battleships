namespace Battleships.Domain.Interfaces
{
	/// <summary>
	/// Describes a strategy for placing ships onto a player's board.
	/// </summary>
	public interface IShipPlacementStrategy
	{
		/// <summary>
		/// Places the specified ships onto the specified board.
		/// </summary>
		/// <param name="board">The board onto which ships are to be placed.</param>
		/// <param name="ships">The ships to place.</param>
		void PlaceShips(Board board, IEnumerable<Ship> ships);
	}
}
