namespace Battleships.Domain
{
	public abstract class Ship(string name, int length)
	{
		public string Name { get; init; } = name;

		public int Length { get; } = length;

		public List<Coordinate>? Coordinates { get; private set; }

		public ShipOrientation? Orientation { get; private set; }

		public List<Coordinate> Hits { get; } = [];

		public void Place(List<Coordinate> coordinates, ShipOrientation orientation)
		{			
			if (!this.AreCoordinatesValid(coordinates, orientation))
			{
				throw new ArgumentException("The specified coordinates are not valid. The number of coordinates must match the length of the ship and must be contiguous according tothe orientation specified.");
			}

			this.Coordinates = [.. coordinates];
			this.Orientation = orientation;
		}

		public void RegisterHit(Coordinate coordinate)
		{
			if (!this.HasBeenPlaced())
			{
				throw new InvalidOperationException("This ship has not been placed.");
			}

			if (!Coordinates!.Contains(coordinate))
			{
				throw new InvalidOperationException($"The ship does not occupy the coordinate {coordinate}.");
			}

			this.Hits.Add(coordinate);
			this.Coordinates.Remove(coordinate);
		}

		public bool HasBeenPlaced()
		{
			return this.Coordinates is not null;
		}

		public bool IsSunk()
		{
			if (!this.HasBeenPlaced())
			{
				return false;
			}

			return this.Coordinates!.Count == 0;
		}

		private bool AreCoordinatesValid(List<Coordinate> coordinates, ShipOrientation orientation)
		{
			ArgumentNullException.ThrowIfNull(coordinates);

			if (coordinates.Count != this.Length)
			{
				return false;
			}

			// Ensure the coordinates are contiguous
			Coordinate startCoord = coordinates.First();

			if (orientation == ShipOrientation.Horizontal)
			{
				for (int i = 1; i < coordinates.Count; i++)
				{
					if (!coordinates[i].Equals(new Coordinate((char)(startCoord.Column + i), startCoord.Row)))
					{
						return false;
					}
				}
			}
			else
			{
				for (int i = 1; i < coordinates.Count; i++)
				{
					if (!coordinates[i].Equals(new Coordinate(startCoord.Column, startCoord.Row + i)))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
