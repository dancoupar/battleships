using Battleships.Domain.Interfaces;

namespace Battleships.Domain
{
	public class BoardFactory(IShipFactory shipFactory, IShipPlacementStrategy shipPlacementStrategy) : IBoardFactory
	{
		public Board CreateBoard(int boardWidth, int boardHeight, IEnumerable<ShipType> shipTypes)
		{
			var board = new Board(boardWidth, boardHeight);
			shipPlacementStrategy.PlaceShips(board, shipFactory.CreateFleet(shipTypes));
			return board;
		}
	}
}
