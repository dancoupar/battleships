namespace Battleships.Domain
{
	/// <summary>
	/// Represents a player.
	/// </summary>
	/// <param name="playerNumber">The player number.</param>
	/// <param name="board">The player's game board.</param>
	public abstract class Player(int playerNumber, Board? board)
	{
		/// <summary>
		/// The player number.
		/// </summary>
		public int PlayerNumber { get; } = playerNumber;

		/// <summary>
		/// The player's game board, if they have one.
		/// </summary>
		public Board? Board { get; } = board;

		/// <summary>
		/// Raised when an opponent makes a correct guess.
		/// </summary>
		public event EventHandler? ShipHit;

		/// <summary>
		/// Raised when an opponent makes a wrong guess.
		/// </summary>
		public event EventHandler? ShipMissed;

		/// <summary>
		/// Raised when an opponent makes a correct guess, resulting in one of the player's ships 
		/// being swunk.
		/// </summary>
		public event EventHandler<Ship>? ShipSunk;

		/// <summary>
		/// Prompts the player to take their turn and make a guess.
		/// </summary>
		/// <returns>The coordinate representing the player's guess.</returns>
		public abstract Coordinate MakeGuess();

		/// <summary>
		/// Responds to a guess made by another player.
		/// </summary>
		/// <param name="guess">The coordinate representing the other player's guess.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void RespondToGuess(Coordinate guess)
		{
			if (!this.HasBoard())
			{
				throw new InvalidOperationException("Player does not have a board.");
			}

			Ship? ship = this.Board!.GetShipAt(guess);

			if (ship is not null)
			{
				ship.RegisterHit(guess);
				this.OnShipHit();

				if (ship.IsSunk())
				{
					this.OnShipSunk(ship);
				}
			}
			else
			{
				this.OnShipMissed();
			}
		}

		/// <summary>
		/// Gets a value indicating whether this player has a game board.
		/// </summary>
		/// <returns>True if the player has a game board, false otherwise.</returns>
		public bool HasBoard()
		{
			return this.Board is not null;
		}		

		/// <summary>
		/// Counts the number of un-sunk ships the player has remaining.
		/// </summary>
		/// <returns>The number of un-sunk ships the player has remaining.</returns>
		public int CountNumberOfShipsRemaining()
		{
			if (!this.HasBoard())
			{
				return 0;	
			}

			return this.Board!.Ships.Count(s => !s.IsSunk());
		}

		public override string ToString()
		{
			return $"Player {this.PlayerNumber}";
		}

		private void OnShipHit()
		{
			ShipHit?.Invoke(this, EventArgs.Empty);
		}

		private void OnShipMissed()
		{
			ShipMissed?.Invoke(this, EventArgs.Empty);
		}

		private void OnShipSunk(Ship ship)
		{
			ShipSunk?.Invoke(this, ship);
		}
	}
}
