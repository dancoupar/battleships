namespace Battleships.Domain.Interfaces
{
	public interface IRandomNumberGenerator
	{
		public int GetRandomNumber(int minValue, int maxValue);
	}
}
