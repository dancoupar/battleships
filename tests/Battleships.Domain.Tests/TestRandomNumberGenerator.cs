using Battleships.Domain.Interfaces;

namespace Battleships.Domain.Tests
{
	internal class TestRandomNumberGenerator : IRandomNumberGenerator
	{
		private static readonly Random _random = new();

		public int GetRandomNumber(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}
	}
}
