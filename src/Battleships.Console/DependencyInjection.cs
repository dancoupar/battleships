using Battleships.Application.Interfaces;
using Battleships.Application;
using Battleships.Domain.Interfaces;
using Battleships.Domain;
using Microsoft.Extensions.DependencyInjection;
using Battleships.Infrastructure;

namespace Battleships.Console
{
	public static class DependencyInjection
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<IBoardFactory, BoardFactory>();
			services.AddScoped<IShipFactory, ShipFactory>();
			services.AddScoped<IRandomNumberGenerator, RandomNumberGenerator>();
			services.AddScoped<IShipPlacementStrategy, RandomShipPlacementStrategy>();
			services.AddScoped<ConsoleIOHandler>();
			services.AddScoped<IUserInputHandler>(s => s.GetRequiredService<ConsoleIOHandler>());
			services.AddScoped<IUserOutputHandler>(s => s.GetRequiredService<ConsoleIOHandler>());
			services.AddScoped<ITurnInputHandler, TurnInputHandler>();
			services.AddScoped<IGameFactory, GameFactory>();
		}
	}
}
