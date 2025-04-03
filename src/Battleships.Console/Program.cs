using Battleships.Application;
using Battleships.Console;
using Battleships.Domain;
using Battleships.Infrastructure;

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
		{ 2, (10, 10) },
	},
	PlayerShipTypes = new Dictionary<int, List<ShipType>>()
	{
		{ 2, new List<ShipType>() { ShipType.Battleship, ShipType.Destroyer, ShipType.Destroyer } }
	}
};

var ioHandler = new ConsoleIOHandler();

var factory = new GameFactory(
	gameConfig,
	new BoardFactory(
		new ShipFactory(),
		new RandomShipPlacementStrategy(new RandomNumberGenerator())
	),
	new TurnInputHandler(ioHandler, ioHandler),	
	ioHandler);

Game game = factory.CreateGame();
game.Start();