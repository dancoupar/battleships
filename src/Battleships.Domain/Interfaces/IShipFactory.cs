namespace Battleships.Domain.Interfaces
{
	public interface IShipFactory
	{
		IEnumerable<Ship> CreateFleet(IEnumerable<ShipType> shipTypes);
	}
}
