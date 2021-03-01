using System;

namespace LS
{
	public class Damageable
	{
		public int MaxLifePoints { get; set; }
		public bool AllowOverflow { get; set; }

		private int _lifePoints;
		public Action OnDeath;

		public int LifePoints
		{
			get => _lifePoints;
			set => _lifePoints = AllowOverflow ? value : Clamp(value, 0, MaxLifePoints);
		}

		public Damageable(int maxLifePoints)
		{
			MaxLifePoints = maxLifePoints;
			LifePoints = maxLifePoints;
		}

		public Damageable(int maxLifePoints, int lifePoints)
		{
			MaxLifePoints = maxLifePoints;
			LifePoints = lifePoints;
		}

		public Damageable(int maxLifePoints, int lifePoints, bool allowOverflow)
		{
			MaxLifePoints = maxLifePoints;
			AllowOverflow = allowOverflow;
			LifePoints = lifePoints;
		}

		public void TakeDamage(int damagePoints)
		{
			LifePoints -= damagePoints;
			if(LifePoints.Equals(0))
				OnDeath?.Invoke();
		}

		private int Clamp(int value, int min, int max)
		{
			if (min > max)
				min = max;
			if (value > max)
				value = max;
			else if (value < min)
				value = min;
			return value;
		}
	}
}