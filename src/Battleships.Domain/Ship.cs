namespace Battleships.Domain
{
	/// <summary>
	/// Represents a ship.
	/// </summary>
	/// <param name="name">The name of the ship.</param>
	/// <param name="length">The length of the ship in terms of the number of coordinates it will occupy.</param>
	public abstract class Ship(string name, int length)
	{
		/// <summary>
		/// The name of the ship.
		/// </summary>
		public string Name { get; init; } = name;

		/// <summary>
		/// The length of the ship in terms of the number of coordinates it will occupy.
		/// </summary>
		public int Length { get; } = length;

		/// <summary>
		/// The coordinates that the ship currently occupies, if it's been placed.
		/// </summary>
		public List<Coordinate>? Coordinates { get; private set; }

		/// <summary>
		/// Whether the ship is oriented horizontally or vertically, if it's been placed.
		/// </summary>
		public ShipOrientation? Orientation { get; private set; }

		/// <summary>
		/// Hits that have been recorded against the ship.
		/// </summary>
		public List<Coordinate> Hits { get; } = [];

		/// <summary>
		/// Places the ship onto the specified coordinates in the specified orientation.
		/// </summary>
		/// <param name="coordinates">
		/// The coordinates which the ship will occupy.
		/// </param>
		/// <param name="orientation">
		/// Whether the ship is oriented horizontally or vertically.
		/// </param>
		/// <exception cref="ArgumentException"></exception>
		public void Place(List<Coordinate> coordinates, ShipOrientation orientation)
		{			
			if (!this.AreCoordinatesValid(coordinates, orientation))
			{
				throw new ArgumentException("The specified coordinates are not valid. The number of coordinates must match the length of the ship and must be contiguous according tothe orientation specified.");
			}

			this.Coordinates = [.. coordinates];
			this.Orientation = orientation;
		}

		/// <summary>
		/// Registers a hit against the ship at the specified coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate that's been hit.</param>
		/// <exception cref="InvalidOperationException"></exception>
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

		/// <summary>
		/// Gets a value indicating whether the ship has been placed on a player's board.
		/// </summary>
		/// <returns>True if the ship has been placed on a player's board, false otherwise.</returns>
		public bool HasBeenPlaced()
		{
			return this.Coordinates is not null;
		}

		/// <summary>
		/// Gets a value indicating whether the ship has been sunk.
		/// </summary>
		/// <returns>True if the ship has been sunk, false otherwise.</returns>
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
