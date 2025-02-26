using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public sealed class Defender : IStatusEffect
    {
        private readonly IUnitId _unitId;
        private ICombatState? _combatState;


        public Defender(IUnitId unitId)
        {
            _unitId = unitId;
        }

        public void Subscribe(ICombatState combatState)
        {
            _combatState = combatState;
            _combatState.CombatEventBus.Subscribe<PreDamageEvent, DamageEventResult>(OnPreDamage);
        }

        public void UnSubscribe()
        {
            if (_combatState == null)
            {
                return;
            }
            _combatState.CombatEventBus.Unsubscribe<PreDamageEvent, DamageEventResult>(OnPreDamage);
            _combatState = null;
        }

        public IStatusEffect DeepClone()
        {
            return new Defender(_unitId);
        }

        private DamageEventResult? OnPreDamage(PreDamageEvent @event, DamageEventResult? previousResult)
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
            
            _combatState.CommandQueue.Add(new TryAttackCombatCommand(@event.Source.Id, @event.Target.Id, _combatState.CommandQueue.Time));
            _combatState.CommandQueue.Add(new DefendCombatCommand(_unitId, @event.Target.Id, _combatState.CommandQueue.Time));
            @event.Source.GetCombatFeature<IDamageable>().DealDamage(unit, 1);
            return new (false);
        }
    }
}