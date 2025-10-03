using System.Collections.Generic;

namespace Nuclear.AbilitySystem.Tests
{
    public class CombatStateBuilder
    {
        private readonly List<IUnitMutable> _units = new();
        private IAbilityContextHolderMutable? _abilityContextHolder;

        public CombatStateBuilder WithUnit(IUnitMutable unit)
        {
            _units.Add(unit);
            return this;
        }

        public CombatStateBuilder WithAbilityContext(IAbilityContextHolderMutable contextHolder)
        {
            _abilityContextHolder = contextHolder;
            return this;
        }

        public ICombatState Build()
        {
            return new CombatState(_units, _abilityContextHolder ?? new AbilityContextHolder());
        }
    }
}
