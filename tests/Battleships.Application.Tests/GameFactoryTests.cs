using Battleships.Application.Interfaces;
using Battleships.Domain;
using Battleships.Tests.Common;
using FluentAssertions;
using Moq;

namespace Battleships.Application.Tests
{
	public class GameFactoryTests
	{
		[Fact]
		public void A_game_can_be_created_to_specification()
		{
			// Arrange
			var sut = new GameFactory(
				new BoardFactory(
					new ShipFactory(),
					new RandomShipPlacementStrategy(
						new TestRandomNumberGenerator()
					)
				),
				new Mock<ITurnInputHandler>().Object,
				new Mock<IUserOutputHandler>().Object);

			var gameConfig = new GameConfiguration()
			{
				NumberOfPlayers = 2,
				PlayerTypes = new Dictionary<int, PlayerType>()
				{
					{ 1, PlayerType.Human },
					{ 2, PlayerType.Computer }
				},
				PlayerBoardSizes = new Dictionary<int, (int Width, int Height)>()
				{
					{ 1, (5, 5) },
					{ 2, (10, 10) },
				},
				PlayerShipTypes = new Dictionary<int, List<ShipType>>()
				{
					{ 1, new List<ShipType>() { ShipType.Battleship } },
					{ 2, new List<ShipType>() { ShipType.Destroyer, ShipType.Battleship } }
				}
			};

			// Act
			Game result = sut.CreateGame(gameConfig);

			// Assert
			result.Players.Length.Should().Be(2);

			result.Players[0].Should().BeOfType<HumanPlayer>();
			result.Players[0].Board.Should().NotBeNull();
			result.Players[0].Board!.Width.Should().Be(5);
			result.Players[0].Board!.Height.Should().Be(5);
			result.Players[0].Board!.Ships.Count.Should().Be(1);
			result.Players[0].Board!.Ships[0].Should().BeOfType<Battleship>();

			result.Players[1].Should().BeOfType<ComputerPlayer>();
			result.Players[1].Board.Should().NotBeNull();
			result.Players[1].Board!.Width.Should().Be(10);
			result.Players[1].Board!.Height.Should().Be(10);
			result.Players[1].Board!.Ships.Count.Should().Be(2);
			result.Players[1].Board!.Ships[0].Should().BeOfType<Destroyer>();
			result.Players[1].Board!.Ships[1].Should().BeOfType<Battleship>();
		}
	}
}
