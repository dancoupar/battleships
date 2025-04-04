namespace Battleships.Domain
{
	/// <summary>
	/// Represents a player's board onto which ships can be placed.
	/// </summary>
	/// <param name="width">The width of the board.</param>
	/// <param name="height">The height of the board.</param>
	public class Board(int width, int height)
	{
		private readonly List<Ship> _ships = [];

		/// <summary>
		/// The width of the board.
		/// </summary>
		public int Width { get; } = width;

		/// <summary>
		/// The height of the board.
		/// </summary>
		public int Height { get; } = height;

		/// <summary>
		/// The ships currently placed on the board.
		/// </summary>
		public IReadOnlyCollection<Ship> Ships
		{
			get
			{
				return _ships.AsReadOnly();
			}
		}

		/// <summary>
		/// Places the specified ship on the board at the specified starting coordinate and with the
		/// specified orientation.
		/// </summary>
		/// <param name="ship">The ship to place.</param>
		/// <param name="startCoordinate">The starting coordinate of the ship.</param>
		/// <param name="orientation">The orientation of the ship.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void PlaceShip(Ship ship, Coordinate startCoordinate, ShipOrientation orientation)
		{
			if (this.Ships.Contains(ship))
			{
				throw new InvalidOperationException("The specified ship has already been placed on this board.");
			}			

			if (!this.CanPlaceShip(ship, startCoordinate, orientation, out List<Coordinate> coordinates))
			{
				throw new InvalidOperationException("One or more of the specified coordinates is already occupied or out of bounds.");
			}

			_ships.Add(ship);
			ship.Place(coordinates, orientation);
		}

		/// <summary>
		/// Gets a value indicating whether the specified ship can be placed on the board at the
		/// specified starting coordinate and with the specified orientation.
		/// </summary>
		/// <param name="ship">The ship to place.</param>
		/// <param name="startCoordinate">The starting coordinate of the ship.</param>
		/// <param name="orientation">The orientation of the ship.</param>
		/// <returns>True if the ship can be placed in this position, false otherwise.</returns>
		public bool CanPlaceShip(Ship ship, Coordinate startCoordinate, ShipOrientation orientation)
		{
			return this.CanPlaceShip(ship, startCoordinate, orientation, out _);
		}

		private bool CanPlaceShip(Ship ship, Coordinate startCoordinate, ShipOrientation orientation, out List<Coordinate> coordinates)
		{
			coordinates = GetProspectiveShipCoordinates(ship, startCoordinate, orientation);
			return coordinates.All(c => this.IsCoordinateInBounds(c) && !this.IsCoordinateOccupied(c));
		}	

		/// <summary>
		/// Gets the ship at the specified coordinate, or null if no ship occupies that coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate to check.</param>
		/// <returns>The ship that occupies the specified coordinate, or null if no ship occupies it.</returns>
		public Ship? GetShipAt(Coordinate coordinate)
		{
			return this.Ships.FirstOrDefault(s => s.HasBeenPlaced() && s.Coordinates!.Contains(coordinate));
		}

		/// <summary>
		/// Gets a value indicating whether there is currently a ship occupying the specified coordinate.
		/// </summary>
		/// <param name="coordinate">The coordiante to check.</param>
		/// <returns>True if the coordinate is occupied, false otherwise.</returns>
		public bool IsCoordinateOccupied(Coordinate coordinate)
		{
			return this.GetShipAt(coordinate) is not null;
		}		

		/// <summary>
		/// Gets a value indicating whether the specified coordinate is within the bounds of the board.
		/// </summary>
		/// <param name="coordinate">The coordinate to check.</param>
		/// <returns>True if the coordinate is within bounds, false otherwise.</returns>
		public bool IsCoordinateInBounds(Coordinate coordinate)
		{
			return coordinate.Column >= 'A'
				&& coordinate.Column < 'A' + this.Width
				&& coordinate.Row >= 1
				&& coordinate.Row <= this.Height;
		}

		private static List<Coordinate> GetProspectiveShipCoordinates(Ship ship, Coordinate startCoordinate, ShipOrientation orientation)
		{
			var coordinates = new List<Coordinate>
			{
				startCoordinate
			};

			if (orientation == ShipOrientation.Horizontal)
			{
				for (int i = 1; i < ship.Length; i++)
				{
					coordinates.Add(new Coordinate((char)(startCoordinate.Column + i), startCoordinate.Row));
				}
			}
			else
			{
				for (int i = 1; i < ship.Length; i++)
				{
					coordinates.Add(new Coordinate(startCoordinate.Column, startCoordinate.Row + i));
				}
			}

			return coordinates;
		}
	}
}
