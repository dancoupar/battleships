namespace Battleships.Domain
{
	/// <summary>
	/// Represents a coordinate on a player's board.
	/// </summary>
	/// <param name="column">The horizontal component.</param>
	/// <param name="row">The vertical component.</param>
	public readonly struct Coordinate(char column, int row)
	{
		/// <summary>
		/// The horizontal component of the coordinate.
		/// </summary>
		public char Column { get; } = column;

		/// <summary>
		/// The vertical component of the coordinate.
		/// </summary>
		public int Row { get; } = row;

		/// <summary>
		/// Parses a string representation of a coordinate. 
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <returns>The resulting coordinate value.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static Coordinate Parse(string s)
		{
			if (!TryParse(s, out Coordinate result))
			{
				throw new ArgumentException($"{s} is not a valid coordinate for the specified board.");
			}
			
			return result;
		}

		/// <summary>
		/// Attempts to parse a string representation of a coordinate and returns a value indicating
		/// whether parsing was successful.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <param name="result">If successful, the resulting coordinate value.</param>
		/// <returns>True if parsing was successful, false otherwise</returns>
		public static bool TryParse(string? s, out Coordinate result)
		{
			result = new Coordinate();

			if (string.IsNullOrWhiteSpace(s))
			{
				return false;
			}

			s = s.Trim().ToUpper();

			if (s.Length > 3)
			{
				return false;
			}
			
			char[] input = s.ToCharArray();
			char column = input[0];

			if (column < 'A' || column > 'Z')
			{
				return false;
			}

			if (!int.TryParse(s.AsSpan(1), out int row))
			{
				return false;
			}

			if (row == 0)
			{
				return false;
			}

			result = new Coordinate(column, row);
			return true;
		}

		public override string ToString()
		{
			return $"{this.Column}{this.Row}";
		}
	}
}
