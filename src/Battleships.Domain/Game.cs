namespace Battleships.Domain
{
	/// <summary>
	/// Models the game itself. Responsible for turn management, score tracking and knowing when the game ends.
	/// </summary>
	public class Game
	{
		private Player? _currentPlayer;
		private bool _gameOver;

		private Game(Player[] players)
		{
			this.Players = players;

			foreach (Player player in this.Players)
			{
				player.ShipHit += OnShipHit;
				player.ShipMissed += OnShipMiss;
				player.ShipSunk += OnShipSunk;

				if (player is HumanPlayer humanPlayer)
				{
					humanPlayer.TurnRequested += OnHumanPlayerTurnRequested;
				}
			}
		}

		public Player[] Players { get; }

		public event Func<HumanPlayer, Player, Coordinate>? HumanPlayerTurnRequested;

		public event EventHandler? GuessOutOfBounds;

		public event EventHandler? ShipMissed;

		public event EventHandler? ShipHit;

		public event EventHandler<Ship>? ShipSunk;

		public event EventHandler<Player>? PlayerWon;

		public static Game CreateNew(Player[] players)
		{
			if (players.Length < 2)
			{
				throw new InvalidOperationException("At least two players are required to create a game.");
			}

			return new Game(players);
		}

		public void Start()
		{
			do
			{
				foreach (Player player in this.Players)
				{
					_currentPlayer = player;
					Player opponent = this.GetNextPlayer();

					if (opponent.HasBoard())
					{
						Coordinate guess = player.MakeGuess();
						opponent.RespondToGuess(guess);
					}

					if (_gameOver)
					{
						break;
					}
				}
			}
			while (!_gameOver);
		}

		public void EndGame()
		{
			_gameOver = true;
		}

		private Player GetNextPlayer()
		{
			int playerIndex = Array.IndexOf(this.Players, _currentPlayer);
			return this.Players[(playerIndex + 1) % this.Players.Length];
		}

		private Coordinate OnHumanPlayerTurnRequested(HumanPlayer player)
		{
			Player opponent = this.GetNextPlayer();

			if (!opponent.HasBoard())
			{
				throw new InvalidOperationException("Human player unable to take turn; opponent has no board.");
			}

			Coordinate guess;
			bool validGuessMade = false;

			do
			{
				guess = HumanPlayerTurnRequested?.Invoke(player, this.GetNextPlayer()) ?? throw new ApplicationException("Unable to request human player input; no handler has been wired up.");
				
				// Was the guess a valid coordinate?
				validGuessMade = opponent.Board!.IsCoordinateInBounds(guess);

				if (!validGuessMade)
				{
					GuessOutOfBounds?.Invoke(this, EventArgs.Empty);
				}
			}
			while (!validGuessMade);

			return guess;
		}

		private void OnShipHit(object? sender, EventArgs e)
		{
			ShipHit?.Invoke(sender, EventArgs.Empty);
		}

		private void OnShipMiss(object? sender, EventArgs e)
		{
			ShipMissed?.Invoke(sender, e);
		}

		private void OnShipSunk(object? sender, Ship ship)
		{
			ShipSunk?.Invoke(this, ship);

			if (this.Players.Where(p => p != _currentPlayer).All(p => p.CountNumberOfShipsRemaining() == 0))
			{
				_gameOver = true;
				PlayerWon?.Invoke(this, _currentPlayer!);
			}
		}		
	}
}
