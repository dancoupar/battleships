namespace Battleships.Domain.Interfaces
{
	/// <summary>
	/// Describes a factory for creating game boards.
	/// </summary>
	public interface IBoardFactory
	{
		/// <summary>
		/// Creates a new game board with the specified dimensions and containing the specified ships.
		/// </summary>
		/// <param name="boardWidth">The width of the board.</param>
		/// <param name="boardHeight">The height of the board.</param>
		/// <param name="shipTypes">The types of ships that should be placed on the board.</param>
		/// <returns>The newly created board.</returns>
		Board CreateBoard(int boardWidth, int boardHeight, IEnumerable<ShipType> shipTypes);
	}
}
