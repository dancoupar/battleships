namespace Battleships.Domain
{
	public abstract class Player(int playerNumber, Board? board)
	{
		public int PlayerNumber { get; } = playerNumber;

		public Board? Board { get; } = board;

		public event EventHandler? ShipHit;

		public event EventHandler? ShipMissed;

		public event EventHandler<Ship>? ShipSunk;

		public abstract Coordinate MakeGuess();

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

		public bool HasBoard()
		{
			return this.Board is not null;
		}		

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
