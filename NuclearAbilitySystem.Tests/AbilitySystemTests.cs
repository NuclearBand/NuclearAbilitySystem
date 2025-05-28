using System.Collections.Generic;
using System.Numerics;
using Nuclear.AbilitySystem;
using NUnit.Framework;

namespace AbilitySystemTests
{
    public sealed class AbilitySystemTests
    {
        [Test]
        public void DoubleStrikeWithOneBully_Full()
        {
            // Setup
            var attackerA = new TestUnit("A", 5, 1, new Vector2(0, 0));
            var targetB = new TestUnit("B", 5, 0, new Vector2(0, 2));
            var bullyC = new TestUnit("C", 5, 1, new Vector2(0, 0));
            
            bullyC.AddBullyStatusEffect();
            attackerA.AddAbility(DoubleStrikeAbility.Create());
            
            // Ability execution
            ICombatState state = new CombatState(new() {attackerA, targetB, bullyC}, new AbilityContextHolder());
            var doubleStrikeAbility = state.GetUnit(attackerA.Id).GetCombatFeature<IAbilitiesHolder>().Abilities[0];
            state.AbilityContextHolder.GetContext<ITimeAbilityContext>().NextTurn();
            
            Assert.AreEqual(true, doubleStrikeAbility.CanExecute(null!, null!, state));
            doubleStrikeAbility.Execute(
                attackerA.Id,
                targetB.Id,
                state);
            
            var result = state.CommandQueue.CalculateCommandQueue();
            
            // Tests
            Assert.AreEqual(6, result.Count); 
            Assert.Contains(new CreateProjectileCombatCommand(attackerA.Id, targetB.Id, 2, 0), result);
            Assert.Contains(new AttackCombatCommand(attackerA.Id, targetB.Id, attackerA.Damage, 2), result);
            Assert.Contains(new AttackCombatCommand(bullyC.Id, targetB.Id, bullyC.Damage, 2), result);
            Assert.Contains(new CreateProjectileCombatCommand(attackerA.Id, targetB.Id, 2, 1), result);
            Assert.Contains(new AttackCombatCommand(attackerA.Id, targetB.Id, attackerA.Damage, 3), result);
            Assert.Contains(new AttackCombatCommand(bullyC.Id, targetB.Id, bullyC.Damage, 3), result);
            
            Assert.AreEqual(false, doubleStrikeAbility.CanExecute(null!, null!, state));
            Assert.AreEqual(3, state.CommandQueue.FinalTime);
            state.Dispose();
        }
        
        [Test]
        public void DoubleStrikeWithOneBully_OneShot()
        {
            var attackerA = new TestUnit("A", 5, 10, new Vector2(0, 0));
            var targetB = new TestUnit("B", 5, 0, new Vector2(0, 0));
            var bullyC = new TestUnit("C", 5, 1, new Vector2(0, 0));
            bullyC.AddBullyStatusEffect();

            ICombatState state = new CombatState(new() {attackerA, targetB, bullyC}, new AbilityContextHolder());
            
            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);
            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);
            
            // Anything above can be changed, but the result must be correct:
            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(2, result.Count); 
            Assert.AreEqual(new AttackCombatCommand(attackerA.Id, targetB.Id, 5, 0), result[0]);
            Assert.AreEqual(new DeathCombatCommand(targetB.Id, 0), result[1]);
            state.Dispose();
        }
        
        [Test]
        public void DoubleStrikeWithTwoBully_Full()
        {
            
            var attackerA = new TestUnit("A", 5, 1, new Vector2(0, 0));
            var targetB = new TestUnit("B", 5, 0, new Vector2(0, 0));
            var bullyC = new TestUnit("C", 5, 1, new Vector2(0, 0));
            
            var bullyD = new TestUnit("D", 5, 1, new Vector2(0, 0));
            bullyC.AddBullyStatusEffect();
            bullyD.AddBullyStatusEffect();

            ICombatState state = new CombatState(new List<IUnit>{attackerA, targetB, bullyC, bullyD}, new AbilityContextHolder());
            

            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);
            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);


            // Anything above can be changed, but the result must be correct:
            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(new AttackCombatCommand(attackerA.Id, targetB.Id, attackerA.Damage, 0), result[0]); // 4 хп
            Assert.AreEqual(new AttackCombatCommand(bullyC.Id, targetB.Id, bullyC.Damage, 0), result[1]); // 3 хп
            Assert.AreEqual(new AttackCombatCommand(bullyD.Id, targetB.Id, bullyD.Damage, 0), result[2]); // 2 хп
            Assert.AreEqual(new AttackCombatCommand(bullyC.Id, targetB.Id, bullyC.Damage, 0), result[3]); // 1 хп
            Assert.AreEqual(new AttackCombatCommand(bullyD.Id, targetB.Id, bullyD.Damage, 0), result[4]); // 0 хп
            Assert.AreEqual(new DeathCombatCommand(targetB.Id, 0), result[5]);
            state.Dispose();
        }
        
        [Test]
        public void SingleStrikeWithOneDefender_Full()
        {
            var attackerA = new TestUnit("A", 5, 1, new Vector2(0, 0));
            var targetB = new TestUnit("B", 5, 0, new Vector2(0, 0));
            var defenderE = new TestUnit("E", 5, 1, new Vector2(0, 0));
            defenderE.AddDefenderStatusEffect();
            
            ICombatState state = new CombatState(new List<IUnit>{attackerA, targetB, defenderE}, new AbilityContextHolder());
            

            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);
            
            
            // Anything above can be changed, but the result must be correct:
            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(3, result.Count); 
            Assert.AreEqual(new TryAttackCombatCommand(attackerA.Id, targetB.Id, 0), result[0]);
            Assert.AreEqual(new DefendCombatCommand(defenderE.Id, targetB.Id, 0), result[1]);
            Assert.AreEqual(new AttackCombatCommand(attackerA.Id, defenderE.Id, attackerA.Damage, 0), result[2]);
            state.Dispose();
        }
        
        [Test]
        public void SingleStrikeWithOneBullyAndOneDefender()
        {
            var attackerA = new TestUnit("A", 5, 1, new Vector2(0, 0));
            var targetB = new TestUnit("B", 5, 0, new Vector2(0, 0));
            var bullyC = new TestUnit("C", 5, 1, new Vector2(0, 0));
            var defenderD = new TestUnit("D", 5, 1, new Vector2(0, 0));
            
            bullyC.AddBullyStatusEffect();
            defenderD.AddDefenderStatusEffect();
            
            ICombatState state = new CombatState(new List<IUnit> { attackerA, targetB, bullyC, defenderD.DeepClone() }, new AbilityContextHolder());
            
            state.GetUnit(attackerA.Id).GetCombatFeature<IDamageable>().DealDamage(state.GetUnit(targetB.Id), 1);
            
            var result = state.CommandQueue.CalculateCommandQueue();
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(new TryAttackCombatCommand(attackerA.Id, targetB.Id, 0), result[0]);
            Assert.AreEqual(new DefendCombatCommand(defenderD.Id, targetB.Id, 0), result[1]);
            Assert.AreEqual(new AttackCombatCommand(attackerA.Id, defenderD.Id, attackerA.Damage, 0), result[2]);
            Assert.AreEqual(new AttackCombatCommand(bullyC.Id, defenderD.Id, bullyC.Damage, 0), result[3]);
            Assert.AreEqual(5, ((TestUnit)state.GetUnit(targetB.Id)).Health);
            Assert.AreEqual(3, ((TestUnit)state.GetUnit(defenderD.Id)).Health);
            Assert.AreEqual(5, defenderD.Health);
            state.Dispose();
        }
    }
}
