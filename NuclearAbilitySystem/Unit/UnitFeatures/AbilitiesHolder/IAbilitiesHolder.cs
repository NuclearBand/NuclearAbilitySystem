using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface IAbilitiesHolder : IUnitFeatureMutable
    {
        void AddAbility(IAbility ability);
        ReadOnlyCollection<IAbility> Abilities { get; }
    }

    public sealed class AbilitiesHolder : IAbilitiesHolder
    {
        private readonly List<IAbility> _abilities = new();

        void IUnitFeatureMutable.Connect(ICombatStateMutable combatState)
        {
        }

        void IUnitFeatureMutable.Disconnect()
        {
        }

        public void AddAbility(IAbility ability)
        {
            _abilities.Add(ability);
        }

        public ReadOnlyCollection<IAbility> Abilities => _abilities.AsReadOnly();

        IUnitFeatureMutable IUnitFeatureMutable.DeepClone()
        {
            var result = new AbilitiesHolder();
            foreach (var ability in _abilities)
            {
                result._abilities.Add(ability.DeepClone());
            }
            return result;
        }
    }
}