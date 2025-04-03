namespace Battleships.Domain
{
	/// <summary>
	/// Models the game itself. Responsible for turn management, score tracking and knowing when the game ends.
	/// </summary>
	public class Game
	{
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

		/// <summary>
		/// The players participating in the game.
		/// </summary>
		public Player[] Players { get; }

		/// <summary>
		/// The player whose turn it is.
		/// </summary>
		public Player? CurrentPlayer { get; private set; }

		/// <summary>
		/// Whether the game has finished.
		/// </summary>
		public bool GameOver { get; private set; }

		/// <summary>
		/// Raised when a human player is requested to take their turn.
		/// </summary>

		public event Func<HumanPlayer, Player, Coordinate>? HumanPlayerTurnRequested;

		/// <summary>
		/// Raised when a guess is made that is not within the bounds of the game board.
		/// </summary>
		public event EventHandler? GuessOutOfBounds;

		/// <summary>
		/// Raised when a player makes a guess but misses.
		/// </summary>
		public event EventHandler? ShipMissed;

		/// <summary>
		/// Raised when a player makes a guess and hits.
		/// </summary>
		public event EventHandler? ShipHit;

		/// <summary>
		/// Raised when a player makes a guess and hits, resulting in the sinking of an opponent's ship.
		/// </summary>
		public event EventHandler<Ship>? ShipSunk;

		/// <summary>
		/// Raised when any player wins the game.
		/// </summary>
		public event EventHandler<Player>? PlayerWon;

		/// <summary>
		/// Creates a new game of battleships involving the specified players.
		/// </summary>
		/// <param name="players">The players of the game.</param>
		/// <returns>The new game instance.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static Game CreateNew(Player[] players)
		{
			if (players.Length < 2)
			{
				throw new InvalidOperationException("At least two players are required to create a game.");
			}

			return new Game(players);
		}

		/// <summary>
		/// Starts the game.
		/// </summary>
		public void Start()
		{
			do
			{
				foreach (Player player in this.Players)
				{
					this.CurrentPlayer = player;
					Player opponent = this.GetNextPlayer();

					if (opponent.HasBoard())
					{
						Coordinate guess = player.MakeGuess();
						opponent.RespondToGuess(guess);
					}

					if (this.GameOver)
					{
						break;
					}
				}
			}
			while (!this.GameOver);
		}

		/// <summary>
		/// Ends the game.
		/// </summary>
		public void EndGame()
		{
			this.GameOver = true;
		}

		private Player GetNextPlayer()
		{
			int playerIndex = Array.IndexOf(this.Players, this.CurrentPlayer);
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

			if (this.Players.Where(p => p != this.CurrentPlayer).All(p => p.CountNumberOfShipsRemaining() == 0))
			{
				this.GameOver = true;
				PlayerWon?.Invoke(this, this.CurrentPlayer!);
			}
		}		
	}
}
