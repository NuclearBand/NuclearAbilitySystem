namespace Nuclear.AbilitySystem
{
    public sealed class DealDamageAbilityAction : IAbilityAction
    {
        private readonly AbilityActionTarget _target;
        private readonly float _multiplier;

        public DealDamageAbilityAction(AbilityActionTarget target, float multiplier)
        {
            _target = target;
            _multiplier = multiplier;
        }
        public void Execute(UnitId source, UnitId? target, ICombatStateMutable context)
        {
            AbilityActionTargetExtensions.UpdateAbilityActionTarget(_target, source, target, 
                out var abilitySource, out var abilityTarget);
            
            context.GetUnit(abilitySource!).GetUnitFeature<IDamageable>().DealDamage(
                context.GetUnit(abilityTarget!), _multiplier
            );
        }

        public IAbilityAction DeepClone()
        {
            return new DealDamageAbilityAction(_target, _multiplier);
        }
    }
}