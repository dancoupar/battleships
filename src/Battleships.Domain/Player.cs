namespace Battleships.Domain
{
	public abstract class Player(Board? board)
	{
		public Board? Board { get; } = board;

		public event EventHandler? ShipHit;

		public event EventHandler? ShipMissed;

		public event EventHandler<Ship>? ShipSunk;
		
		public void RespondToGuess(Coordinate guess)
		{
			if (!this.HasBoard())
			{
				throw new InvalidOperationException();
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

		public abstract Coordinate MakeGuess();

		public int CountNumberOfShipsRemaining()
		{
			if (!this.HasBoard())
			{
				return 0;	
			}

			return this.Board!.Ships.Count(s => !s.IsSunk());
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
