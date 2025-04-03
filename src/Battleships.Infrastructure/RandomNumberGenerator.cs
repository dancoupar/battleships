using Battleships.Domain.Interfaces;

namespace Battleships.Infrastructure
{
	public class RandomNumberGenerator : IRandomNumberGenerator
	{
		private static readonly Random _random = new();

		public int GetRandomNumber(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}
	}
}
