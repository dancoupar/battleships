namespace Battleships.Domain.Interfaces
{
	/// <summary>
	/// Describes a pseudo-random number generator.
	/// </summary>
	public interface IRandomNumberGenerator
	{
		/// <summary>
		/// Generates and returns a new pseudo-random number between the specified min and max values. 
		/// </summary>
		/// <param name="minValue">The minimum value, inclusive.</param>
		/// <param name="maxValue">The maximum value, exclusive.</param>
		/// <returns>The generated pseudo-random number.</returns>
		public int GetRandomNumber(int minValue, int maxValue);
	}
}
