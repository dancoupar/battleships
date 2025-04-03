using FluentAssertions;

namespace Battleships.Domain.Tests
{
	public class CoordinateTests
	{
		[Theory]
		[InlineData("A1", true)]
		[InlineData("a1", true)]		
		[InlineData(" A1", true)]
		[InlineData("A1 ", true)]
		[InlineData("Z99", true)]
		[InlineData(" Z99 ", true)]
		[InlineData("A0", false)]
		[InlineData("Z100", false)]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData(" ", false)]
		[InlineData("A", false)]
		[InlineData("AA", false)]
		[InlineData("1A", false)]
		public void Valid_coordinates_can_be_parsed(string? input, bool expectedResult)
		{
			// Arrange
			// Act
			bool result = Coordinate.TryParse(input!, out Coordinate actual);

			// Assert
			result.Should().Be(expectedResult);
			
			if (expectedResult)
			{
				actual.ToString().Should().Be(input!.Trim().ToUpper());
			}
		}
	}
}
