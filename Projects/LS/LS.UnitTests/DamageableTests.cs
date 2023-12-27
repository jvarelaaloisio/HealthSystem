using NUnit.Framework;

namespace LS.UnitTests
{
	[TestFixture]
	public class DamageableTests
	{
		private const int MAX_LIFE_POINTS = 10;
		private const int OVERFLOWING_LIFE_POINTS = 20;
		private const int NOT_KILLING_DAMAGE = 5;
		private const int OVERKILL_DAMAGE = 25;
		private Damageable _damageable;

		[SetUp]
		public void SetUp()
		{
			_damageable = new Damageable(MAX_LIFE_POINTS);
		}

		#region Constructor Tests

		[Test]
		public void Constructor_GivenMaxLifePoints_HasSameMaxLifePoints()
		{
			Assert.AreEqual(MAX_LIFE_POINTS, _damageable.MaxLifePoints);
		}

		[Test]
		public void Constructor_GivenOnlyMaxLifePoints_HasSameLifePoints()
		{
			Assert.AreEqual(MAX_LIFE_POINTS, _damageable.LifePoints);
		}

		[Test]
		public void Constructor_GivenOverflowingLifePoints_LifePointsAreClamped()
		{
			_damageable = new Damageable(MAX_LIFE_POINTS, OVERFLOWING_LIFE_POINTS);
			Assert.AreEqual(MAX_LIFE_POINTS, _damageable.LifePoints);
		}

		[Test]
		public void Constructor_GivenOverflowingLifePointsAndNotAllowedToOverflow_LifePointsDontOverflow()
		{
			_damageable = new Damageable(MAX_LIFE_POINTS, OVERFLOWING_LIFE_POINTS, false);
			LifePointsDontOverflow();
		}

		[Test]
		public void Constructor_GivenOverflowingLifePointsAndIsAllowedToOverflow_LifePointsOverflow()
		{
			_damageable = new Damageable(MAX_LIFE_POINTS, OVERFLOWING_LIFE_POINTS, true);
			LifePointsOverflow();
		}

		#endregion

		#region LifePoints Tests

		[Test]
		public void LifePoints_GivenNotAllowedToOverflowAndSetToOverflowingLifePoints_LifePointsDontOverflow()
		{
			GivenNotAllowedToOverflow();
			_damageable.LifePoints = OVERFLOWING_LIFE_POINTS;
			LifePointsDontOverflow();
		}

		[Test]
		public void LifePoints_GivenAllowedToOverflowAndSetToOverflowingLifePoints_LifePointsDontOverflow()
		{
			GivenAllowedToOverflow();
			_damageable.LifePoints = OVERFLOWING_LIFE_POINTS;
			LifePointsOverflow();
		}

		#endregion

		#region TakeDamage Tests

		[Test]
		public void TakeDamage_GivenTakesDamage_HasLessLifePoints()
		{
			const int EXPECTED_LIFE_POINTS = MAX_LIFE_POINTS - NOT_KILLING_DAMAGE;
			_damageable.TakeDamage(NOT_KILLING_DAMAGE);
			Assert.AreEqual(EXPECTED_LIFE_POINTS, _damageable.LifePoints);
		}

		[Test]
		public void TakeDamage_GivenKillingDamage_LifePointsAreZero()
		{
			_damageable.TakeDamage(OVERKILL_DAMAGE);
			Assert.AreEqual(0, _damageable.LifePoints);
		}

		[Test]
		public void TakeDamage_GivenKillingDamage_OnDeathIsCalled()
		{
			bool wasCalled = false;
			_damageable.OnDeath += () => wasCalled = true; 
			_damageable.TakeDamage(OVERKILL_DAMAGE);
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void TakeDamage_GivenKillingDamageAndAllowedOverflow_OnDeathIsCalled()
		{
			_damageable.AllowOverflow = true;
			bool wasCalled = false;
			_damageable.OnDeath += () => wasCalled = true; 
			_damageable.TakeDamage(OVERKILL_DAMAGE);
			Assert.IsTrue(wasCalled);
		}

		#endregion

		#region Given

		private void GivenAllowedToOverflow()
		{
			_damageable.AllowOverflow = true;
		}

		private void GivenNotAllowedToOverflow()
		{
			_damageable.AllowOverflow = false;
		}

		#endregion

		#region Then

		private void LifePointsOverflow()
		{
			Assert.AreEqual(OVERFLOWING_LIFE_POINTS, _damageable.LifePoints);
		}

		private void LifePointsDontOverflow()
		{
			Assert.AreEqual(MAX_LIFE_POINTS, _damageable.LifePoints);
		}

		#endregion
	}
}