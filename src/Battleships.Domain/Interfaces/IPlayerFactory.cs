namespace Battleships.Domain.Interfaces
{
	public interface IPlayerFactory
	{
		public IEnumerable<Player> CreatePlayers();
	}
}
