namespace Battleships.Domain
{
	public readonly struct Coordinate(char column, int row)
	{
		public char Column { get; } = column;

		public int Row { get; } = row;

		public static Coordinate Parse(string s)
		{
			if (!TryParse(s, out Coordinate result))
			{
				throw new ArgumentException($"{s} is not a valid coordinate for the specified board.");
			}

			return result;
		}

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
