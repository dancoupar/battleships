using Battleships.Application;
using Battleships.Application.Interfaces;
using Battleships.Console;
using Battleships.Domain;
using Battleships.Domain.Interfaces;
using Battleships.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IBoardFactory, BoardFactory>();
builder.Services.AddScoped<IShipFactory, ShipFactory>();
builder.Services.AddScoped<IRandomNumberGenerator, RandomNumberGenerator>();
builder.Services.AddScoped<IShipPlacementStrategy, RandomShipPlacementStrategy>();
builder.Services.AddScoped<ConsoleIOHandler>();
builder.Services.AddScoped<IUserInputHandler>(s => s.GetRequiredService<ConsoleIOHandler>());
builder.Services.AddScoped<IUserOutputHandler>(s => s.GetRequiredService<ConsoleIOHandler>());
builder.Services.AddScoped<ITurnInputHandler, TurnInputHandler>();
builder.Services.AddScoped<IGameFactory, GameFactory>();

using IHost host = builder.Build();

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

IGameFactory factory = host.Services.GetRequiredService<IGameFactory>();
Game game = factory.CreateGame(gameConfig);
game.Start();