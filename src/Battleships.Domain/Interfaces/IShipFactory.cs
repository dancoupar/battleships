namespace Battleships.Domain.Interfaces
{
	/// <summary>
	/// Describes a factory for creating ship instances.
	/// </summary>
	public interface IShipFactory
	{
		/// <summary>
		/// Creates a fleet of ships to the specification indicated by the provided list.
		/// </summary>
		/// <param name="shipTypes">The list of ship types to create.</param>
		/// <returns>The newly created ship instances.</returns>
		IEnumerable<Ship> CreateFleet(IEnumerable<ShipType> shipTypes);
	}
}
