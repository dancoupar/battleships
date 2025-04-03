using System.Text;

namespace Battleships.Domain
{
	public class Board(int width, int height)
	{
		public int Width { get; } = width;

		public int Height { get; } = height;

		public List<Ship> Ships { get; } = [];

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

			this.Ships.Add(ship);
			ship.Place(coordinates, orientation);
		}

		public bool CanPlaceShip(Ship ship, Coordinate startCoordinate, ShipOrientation orientation)
		{
			return this.CanPlaceShip(ship, startCoordinate, orientation, out _);
		}

		private bool CanPlaceShip(Ship ship, Coordinate startCoordinate, ShipOrientation orientation, out List<Coordinate> coordinates)
		{
			coordinates = GetProspectiveShipCoordinates(ship, startCoordinate, orientation);
			return coordinates.All(c => this.IsCoordinateInBounds(c) && !this.IsCoordinateOccupied(c));
		}	

		public Ship? GetShipAt(Coordinate coordinate)
		{
			return this.Ships.FirstOrDefault(s => s.HasBeenPlaced() && s.Coordinates!.Contains(coordinate));
		}

		public bool IsCoordinateOccupied(Coordinate coordinate)
		{
			return this.GetShipAt(coordinate) is not null;
		}		

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

		public override string ToString()
		{
			var builder = new StringBuilder();

			for (int row = 1; row <= this.Height; row++)
			{
				for (char column = 'A'; column < 'A' + this.Width; column++)
				{
					if (this.IsCoordinateOccupied(new Coordinate(column, row)))
					{
						builder.Append(" X ");
					}
					else
					{
						builder.Append(" O ");
					}
				}

				builder.AppendLine();
			}

			return builder.ToString();
		}
	}
}
