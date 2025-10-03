using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public sealed class Bully : IStatusEffectMutable
    {
        private readonly UnitId _unitId;
        private ICombatStateMutable? _combatState;

        public Bully(UnitId unitId)
        {
            _unitId = unitId;
        }
        
        public void Connect(ICombatStateMutable combatState)
        {
            _combatState = combatState;
            _combatState.CombatEventBus.Connect<AfterDamageEvent, DamageEventResult>(OnAfterDamage);
        }

        public void Disconnect()
        {
            if (_combatState == null)
            {
                return;
            }
            _combatState.CombatEventBus.Disconnect<AfterDamageEvent, DamageEventResult>(OnAfterDamage);
            _combatState = null;
        }

        public IStatusEffectMutable DeepClone()
        {
            return new Bully(_unitId);
        }

        private DamageEventResult? OnAfterDamage(AfterDamageEvent @event, DamageEventResult? previousResult)
        {
            if (previousResult is {ContinueExecution: false})
            {
                return previousResult;
            }
            if (_combatState == null)
            {
                throw new();
            }
            
            if (_unitId == @event.Source.Id || _unitId == @event.Target.Id)
            {
                return previousResult ?? new (true);
            }

            var unit = _combatState.GetUnit(_unitId);
            
            unit.GetUnitFeature<IDamageable>().DealDamage(@event.Target, 1);
            return new(false);
        }
    }
}