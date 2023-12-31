using NUnit.Framework;

namespace HealthSystem.UnitTests
{
	[TestFixture]
	public class HealthTests
	{
		private const int MaxHP = 10;
		private const int OverflowingHP = 20;
		private const int NotKillingDamage = 5;
		private const int OverkillDamage = 25;
		private const int MinimumNonZeroHP = 1;
		private const int NotOverflowingHeal = 5;
		private const int OverflowingHeal = 25;
		private Health _health;

		[SetUp]
		public void SetUp()
		{
			_health = new Health(MaxHP);
		}

		#region Constructor Tests

		[Test]
		public void Constructor_GivenMaxHealthPoints_HasSameMaxHealthPoints()
		{
			AssertHealthPointsEqualMaxHP();
		}

		[Test]
		public void Constructor_GivenOnlyMaxHealthPoints_HasSameHealthPoints()
		{
			AssertHealthPointsEqualMaxHP();
		}

		[Test]
		public void Constructor_GivenOverflowingHealthPoints_HealthPointsAreClamped()
		{
			_health = new Health(MaxHP, OverflowingHP);
			AssertHealthPointsEqualMaxHP();
		}

		[Test]
		public void Constructor_GivenOverflowingHealthPointsAndNotAllowedToOverflow_HealthPointsDontOverflow()
		{
			_health = new Health(MaxHP, OverflowingHP, false);
			AssertHealthPointsEqualMaxHP();
		}

		[Test]
		public void Constructor_GivenOverflowingHealthPointsAndIsAllowedToOverflow_HealthPointsOverflow()
		{
			_health = new Health(MaxHP, OverflowingHP, true);
			AssertHealthPointsOverflow();
		}

		#endregion

		#region HealthPoints Tests

		[Test]
		public void HealthPoints_GivenNotAllowedToOverflowAndSetToOverflowingHealthPoints_HealthPointsDontOverflow()
		{
			GivenNotAllowedToOverflow();
			_health.HP = OverflowingHP;
			AssertHealthPointsEqualMaxHP();
		}

		[Test]
		public void HealthPoints_GivenAllowedToOverflowAndSetToOverflowingHealthPoints_HealthPointsDontOverflow()
		{
			GivenAllowedToOverflow();
			_health.HP = OverflowingHP;
			AssertHealthPointsOverflow();
		}

		#endregion

		#region TakeDamage Tests

		[Test]
		public void TakeDamage_GivenTakesDamage_HasLessHealthPoints()
		{
			const int expectedHealthPoints = MaxHP - NotKillingDamage;
			_health.TakeDamage(NotKillingDamage);
			Assert.That(expectedHealthPoints, Is.EqualTo(_health.HP));
		}
		[Test]
		public void TakeDamage_GivenTakesDamage_OnDamageIsCalled()
		{
			bool wasCalled = false;
			_health.OnDamage += (before, after) => wasCalled = true; 
			_health.TakeDamage(NotKillingDamage);
			Assert.That(wasCalled);
		}
		[Test]
		public void TakeDamage_GivenTakesDamage_OnDamageGivesCorrectValues()
		{
			int correctBefore = _health.HP;
			int actualBefore = -1;
			
			int correctAfter = correctBefore - NotKillingDamage;
			int actualAfter = -1;
			
			_health.OnDamage += (before, after) =>
			{
				actualBefore = before;
				actualAfter = after;
			};
			_health.TakeDamage(NotKillingDamage);
			Assert.That(correctBefore, Is.EqualTo(actualBefore));
			Assert.That(correctAfter, Is.EqualTo(actualAfter));
		}

		[Test]
		public void TakeDamage_GivenKillingDamage_HealthPointsAreZero()
		{
			_health.TakeDamage(OverkillDamage);
			Assert.That(0, Is.EqualTo(_health.HP));
		}

		[Test]
		public void TakeDamage_GivenKillingDamage_OnDeathIsCalled()
		{
			bool wasCalled = false;
			_health.OnDeath += () => wasCalled = true; 
			_health.TakeDamage(OverkillDamage);
			Assert.That(wasCalled);
		}

		[Test]
		public void TakeDamage_GivenKillingDamageAndAllowedOverflow_OnDeathIsCalled()
		{
			_health.AllowOverflow = true;
			bool wasCalled = false;
			_health.OnDeath += () => wasCalled = true; 
			_health.TakeDamage(OverkillDamage);
			Assert.That(wasCalled);
		}

		#endregion

		#region Heal Tests
			
			[Test]
			public void Heal_GivenHeals_HasMoreHP()
			{
				_health.HP = MinimumNonZeroHP;
				const int expectedHealthPoints = MinimumNonZeroHP + NotOverflowingHeal;
				_health.Heal(NotOverflowingHeal);
				Assert.That(expectedHealthPoints, Is.EqualTo(_health.HP));
			}
			
			[Test]
			public void Heal_GivenHeals_OnHealIsCalled()
			{
				bool wasCalled = false;
				_health.OnHeal += (before, after) => wasCalled = true; 
				_health.Heal(NotOverflowingHeal);
				Assert.That(wasCalled);
			}
			
			[Test]
			public void Heal_GivenHeals_OnHealGivesCorrectValues()
			{
				_health.HP = MinimumNonZeroHP;
				int healingValue = NotOverflowingHeal;
				
				int correctBefore = _health.HP;
				int actualBefore = -1;
				
				int correctAfter = correctBefore + healingValue;
				int actualAfter = -1;
				
				_health.OnHeal += (before, after) =>
				{
					actualBefore = before;
					actualAfter = after;
				};
				
				_health.Heal(healingValue);
				
				Assert.That(correctBefore, Is.EqualTo(actualBefore));
				Assert.That(correctAfter, Is.EqualTo(actualAfter));
			}
			
			[Test]
			public void Heal_GivenOverflowingHealAndIsAllowedToOverflow_HealthPointsOverflow()
			{
				GivenAllowedToOverflow();
				_health.Heal(OverflowingHeal);
				Assert.That(MaxHP + OverflowingHeal, Is.EqualTo(_health.HP));
			}

			[Test]
			public void Heal_GivenOverflowingHealAndNotAllowedToOverflow_HealthPointsAreMaxHP()
			{
				GivenNotAllowedToOverflow();
				_health.Heal(OverflowingHeal);
				AssertHealthPointsEqualMaxHP();
			}

		#endregion
			
		#region Given

		private void GivenAllowedToOverflow()
		{
			_health.AllowOverflow = true;
		}

		private void GivenNotAllowedToOverflow()
		{
			_health.AllowOverflow = false;
		}

		#endregion

		#region Then

		private void AssertHealthPointsOverflow()
		{
			Assert.That(OverflowingHP, Is.EqualTo(_health.HP));
		}

		private void AssertHealthPointsEqualMaxHP()
		{
			Assert.That(MaxHP, Is.EqualTo(_health.HP));
		}

		#endregion
	}
}