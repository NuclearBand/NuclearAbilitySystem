using System;
using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public sealed class TestUnit : Unit
    {
        public string Name { get; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
        public bool IsDead => Health <= 0;
        
        public TestUnit(string name, int health, int damage, Vector2 position) : base(new UnitId(name))
        {
            Name = name;
            Health = health;
            Damage = damage;
            
            _features.Add(typeof(IDamageable), new Damageable(Id, u =>
                {
                    var unit = (TestUnit) u;
                    return !unit.IsDead;
                },
                (u, dmg) =>
                {
                    var unit = (TestUnit) u;
                    var realDamage = Math.Min(unit.Health, dmg);
                    unit.Health -= realDamage;
                    return realDamage;
                },
                (u) =>
                {
                    var unit = (TestUnit) u;
                    return unit.Damage;
                }));
            _features.Add(typeof(IStatusEffectsHolder), new StatusEffectsHolder(Id));
            _features.Add(typeof(IAbilitiesHolder), new AbilitiesHolder());
            _features.Add(typeof(IHasPositionCombatFeature), new HasPositionCombatFeature(position));
        }

        private TestUnit(TestUnit testUnit) : base(testUnit)
        {
            Name = testUnit.Name;
            Health = testUnit.Health;
            Damage = testUnit.Damage;
        }


        public override IUnit DeepClone()
        {
            return new TestUnit(this);
        }
    }
}
