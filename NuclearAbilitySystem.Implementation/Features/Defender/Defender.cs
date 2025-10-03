namespace Nuclear.AbilitySystem
{
    public sealed class Defender : IStatusEffectMutable
    {
        private readonly UnitId _unitId;
        private ICombatStateMutable? _combatState;


        public Defender(UnitId unitId)
        {
            _unitId = unitId;
        }

        public void Connect(ICombatStateMutable combatState)
        {
            _combatState = combatState;
            _combatState.CombatEventBus.Connect<PreDamageEvent, DamageEventResult>(OnPreDamage);
        }

        public void Disconnect()
        {
            if (_combatState == null)
            {
                return;
            }
            _combatState.CombatEventBus.Disconnect<PreDamageEvent, DamageEventResult>(OnPreDamage);
            _combatState = null;
        }

        public IStatusEffectMutable DeepClone()
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
            if (_unitId == @event.Source.Id || _unitId == @event.Target.Id)
            {
                return previousResult ?? new (true);
            }

            var unit = _combatState.GetUnit(_unitId);
            
            _combatState.CommandQueue.Add(new TryAttackCombatCommand(@event.Source.Id, @event.Target.Id, _combatState.CommandQueue.Time));
            _combatState.CommandQueue.Add(new DefendCombatCommand(_unitId, @event.Target.Id, _combatState.CommandQueue.Time));
            @event.Source.GetUnitFeature<IDamageable>().DealDamage(unit, 1);
            return new (false);
        }
    }
}