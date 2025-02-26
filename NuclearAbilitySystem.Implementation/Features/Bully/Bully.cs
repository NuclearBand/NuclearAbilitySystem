using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public sealed class Bully : IStatusEffect
    {
        private readonly IUnitId _unitId;
        private ICombatState? _combatState;

        public Bully(IUnitId unitId)
        {
            _unitId = unitId;
        }
        
        public void Subscribe(ICombatState combatState)
        {
            _combatState = combatState;
            _combatState.CombatEventBus.Subscribe<AfterDamageEvent, DamageEventResult>(OnAfterDamage);
        }

        public void UnSubscribe()
        {
            if (_combatState == null)
            {
                return;
            }
            _combatState.CombatEventBus.Unsubscribe<AfterDamageEvent, DamageEventResult>(OnAfterDamage);
            _combatState = null;
        }

        public IStatusEffect DeepClone()
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
            
            if (EqualityComparer<IUnitId>.Default.Equals(_unitId, @event.Source.Id) ||
                EqualityComparer<IUnitId>.Default.Equals(_unitId, @event.Target.Id))
            {
                return previousResult ?? new (true);
            }

            var unit = _combatState.GetUnit(_unitId);
            
            unit.GetCombatFeature<IDamageable>().DealDamage(@event.Target, 1);
            return new(false);
        }
    }
}