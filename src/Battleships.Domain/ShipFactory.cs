using Battleships.Domain.Interfaces;

namespace Battleships.Domain
{
	public class ShipFactory : IShipFactory
	{
		public IEnumerable<Ship> CreateFleet(IEnumerable<ShipType> shipTypes)
		{
			var ships = new List<Ship>();

			foreach (ShipType shipType in shipTypes)
			{
				ships.Add(shipType switch
				{
					ShipType.Destroyer => new Destroyer(),
					ShipType.Battleship => new Battleship(),
					_ => throw new NotSupportedException()
				});
			}

			return ships;
		}
	}
}
