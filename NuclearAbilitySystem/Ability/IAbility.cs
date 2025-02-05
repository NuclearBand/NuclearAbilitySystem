using System.Collections.ObjectModel;
using System.Linq;

namespace Nuclear.AbilitySystem
{
    public interface IAbility
    {
        ReadOnlyCollection<IAbilityAction> Actions { get; }
        ReadOnlyCollection<IAbilityCheck> Checks { get; }
        bool CanExecute(IUnitId sourceId, IUnitId? targetId, IAbilityContextHolder abilityContext);
        void Execute(IUnitId sourceId, IUnitId? targetId, ICombatEventBus context, IAbilityContextHolder abilityContext);
        IAbility DeepClone();
    }

    public class Ability : IAbility
    {
        private readonly ReadOnlyCollection<IAbilityAction> _abilityActions;
        private readonly ReadOnlyCollection<IAbilityCheck> _abilityChecks;

        public Ability(ReadOnlyCollection<IAbilityAction> abilityActions, 
            ReadOnlyCollection<IAbilityCheck> abilityChecks)
        {
            _abilityActions = abilityActions;
            _abilityChecks = abilityChecks;
        }

        public ReadOnlyCollection<IAbilityAction> Actions => _abilityActions;
        public ReadOnlyCollection<IAbilityCheck> Checks => _abilityChecks;

        public bool CanExecute(IUnitId sourceId, IUnitId? targetId, IAbilityContextHolder abilityContext)
        {
            return _abilityChecks.All(a => a.CanExecute(sourceId, targetId, abilityContext));
        }

        public void Execute(IUnitId sourceId, IUnitId? targetId, ICombatEventBus context, IAbilityContextHolder abilityContext)
        {
            foreach (var abilityCheck in _abilityChecks)
            {
                abilityCheck.Execute(sourceId, targetId, abilityContext);
            }
            foreach (var abilityAction in _abilityActions)
            {
                abilityAction.Execute(sourceId, targetId, context, abilityContext);
            }
        }

        public IAbility DeepClone()
        {
            return new Ability(_abilityActions.Select(a => a.DeepClone()).ToList().AsReadOnly(),
                _abilityChecks.Select(a => a.DeepClone()).ToList().AsReadOnly());
        }
    }
}