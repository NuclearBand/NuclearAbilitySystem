using System.Numerics;
using Nuclear.AbilitySystem;
using Nuclear.AbilitySystem.Tests;
using NUnit.Framework;

namespace AbilitySystemTests
{
    public sealed class AbilitySystemTests
    {
        [Test]
        public void DoubleStrikeWithOneBully_Full()
        {
            var doubleStrikeAbility = DoubleStrikeAbility.Create();
            var state = new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 1, new Vector2(0, 0)).WithAbility(doubleStrikeAbility))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 2)))
                .WithUnit(new TestUnit("C", 5, 1, new Vector2(0, 0)).WithBullyStatusEffect())
                .Build();

            state.AbilityContextHolder.GetContext<ITurnAbilityContext>().NextTurn();

            Assert.AreEqual(true, doubleStrikeAbility.CanExecute(null!, null!, state));
            var result = state.ExecuteAbility(new StringUnitId("A"), 0, new StringUnitId("B"));

            // Tests
            Assert.AreEqual(6, result.Commands.Count);
            Assert.Contains(new CreateProjectileCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 2, 0),
                result.Commands);
            Assert.Contains(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 1, 2), result.Commands);
            Assert.Contains(new AttackCombatCommand(new StringUnitId("C"), new StringUnitId("B"), 1, 2), result.Commands);
            Assert.Contains(new CreateProjectileCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 2, 1),
                result.Commands);
            Assert.Contains(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 1, 3), result.Commands);
            Assert.Contains(new AttackCombatCommand(new StringUnitId("C"), new StringUnitId("B"), 1, 3), result.Commands);

            //Assert.AreEqual(false, doubleStrikeAbility.CanExecute(null!, null!, result.ResultState));
            Assert.AreEqual(3, result.Commands[^1].Time);
        }

        [Test]
        public void DoubleStrikeWithOneBully_OneShot()
        {
            var state = (ICombatStateMutable)new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 10, new Vector2(0, 0)))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 0)))
                .WithUnit(new TestUnit("C", 5, 1, new Vector2(0, 0)).WithBullyStatusEffect())
                .Build();

            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);
            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);

            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 5, 0), result[0]);
            Assert.AreEqual(new DeathCombatCommand(new StringUnitId("B"), 0), result[1]);
        }

        [Test]
        public void DoubleStrikeWithTwoBully_Full()
        {
            var state = (ICombatStateMutable)new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 1, new Vector2(0, 0)))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 0)))
                .WithUnit(new TestUnit("C", 5, 1, new Vector2(0, 0)).WithBullyStatusEffect())
                .WithUnit(new TestUnit("D", 5, 1, new Vector2(0, 0)).WithBullyStatusEffect())
                .Build();

            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);
            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);

            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 1, 0),
                result[0]); // 4 hp
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("C"), new StringUnitId("B"), 1, 0),
                result[1]); // 3 hp
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("D"), new StringUnitId("B"), 1, 0),
                result[2]); // 2 hp
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("C"), new StringUnitId("B"), 1, 0),
                result[3]); // 1 hp
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("D"), new StringUnitId("B"), 1, 0),
                result[4]); // 0 hp
            Assert.AreEqual(new DeathCombatCommand(new StringUnitId("B"), 0), result[5]);
        }

        [Test]
        public void SingleStrikeWithOneDefender_Full()
        {
            var state = (ICombatStateMutable)new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 1, new Vector2(0, 0)))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 0)))
                .WithUnit(new TestUnit("E", 5, 1, new Vector2(0, 0)).WithDefenderStatusEffect())
                .Build();

            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);

            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new TryAttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 0), result[0]);
            Assert.AreEqual(new DefendCombatCommand(new StringUnitId("E"), new StringUnitId("B"), 0), result[1]);
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("E"), 1, 0), result[2]);
        }

        [Test]
        public void SingleStrikeWithOneBullyAndOneDefender()
        {
            var state = (ICombatStateMutable)new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 1, new Vector2(0, 0)))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 0)))
                .WithUnit(new TestUnit("C", 5, 1, new Vector2(0, 0)).WithBullyStatusEffect())
                .WithUnit(new TestUnit("D", 5, 1, new Vector2(0, 0)).WithDefenderStatusEffect())
                .Build();

            state.GetUnit(new StringUnitId("A")).GetUnitFeature<IDamageable>()
                .DealDamage(state.GetUnit(new StringUnitId("B")), 1);

            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(new TryAttackCombatCommand(new StringUnitId("A"), new StringUnitId("B"), 0), result[0]);
            Assert.AreEqual(new DefendCombatCommand(new StringUnitId("D"), new StringUnitId("B"), 0), result[1]);
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("A"), new StringUnitId("D"), 1, 0), result[2]);
            Assert.AreEqual(new AttackCombatCommand(new StringUnitId("C"), new StringUnitId("D"), 1, 0), result[3]);
            Assert.AreEqual(5, ((TestUnit) state.GetUnit(new StringUnitId("B"))).Health);
            Assert.AreEqual(3, ((TestUnit) state.GetUnit(new StringUnitId("D"))).Health);
        }

        [Test]
        public void TwoAbilities()
        {
            var state = new CombatStateBuilder()
                .WithUnit(new TestUnit("A", 5, 1, new Vector2(0, 0)).WithAbility(SimpleAbility.Create()))
                .WithUnit(new TestUnit("B", 5, 0, new Vector2(0, 2)))
                .Build();

             state.ExecuteAbility(new StringUnitId("A"), 0, new StringUnitId("B"));
            state.ExecuteAbility(new StringUnitId("A"), 0, new StringUnitId("B"));
        }
    }
}