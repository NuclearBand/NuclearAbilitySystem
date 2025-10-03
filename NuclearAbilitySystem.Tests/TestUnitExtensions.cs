using Nuclear.AbilitySystem;

namespace Nuclear.AbilitySystem.Tests
{
    public static class TestUnitExtensions
    {
        public static TestUnit WithAbility(this TestUnit unit, IAbility ability)
        {
            unit.AddAbility(ability);
            return unit;
        }

        public static TestUnit WithBullyStatusEffect(this TestUnit unit)
        {
            unit.AddBullyStatusEffect();
            return unit;
        }

        public static TestUnit WithDefenderStatusEffect(this TestUnit unit)
        {
            unit.AddDefenderStatusEffect();
            return unit;
        }
    }
}
