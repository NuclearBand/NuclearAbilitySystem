using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nuclear.AbilitySystem
{
    public sealed class StatusEffectsHolder : IStatusEffectsHolder
    {
        private readonly List<IStatusEffectMutable> _statusEffects = new();
        private readonly UnitId _unitId;
        
        private ICombatStateMutable? _combatState;

        public ReadOnlyCollection<IStatusEffect> StatusEffects => _statusEffects.OfType<IStatusEffect>().ToList().AsReadOnly();

        public StatusEffectsHolder(UnitId unitId)
        {
            _unitId = unitId;
        }

        public void Connect(ICombatStateMutable combatState)
        {
            _combatState = combatState;
            var unit = _combatState.GetUnit(_unitId);
            foreach (var statusEffect in _statusEffects)
            {
                statusEffect.Connect(_combatState);
            }
        }

        public void Disconnect()
        {
            _combatState = null;
            foreach (var statusEffect in _statusEffects)
            {
                statusEffect.Disconnect();
            }
        }

        public void AddStatusEffect(IStatusEffectMutable statusEffectMutable)
        {
            _statusEffects.Add(statusEffectMutable);
        }

        public void RemoveStatusEffect(IStatusEffectMutable statusEffectMutable)
        {
            _statusEffects.Remove(statusEffectMutable);
            statusEffectMutable.Disconnect();
        }

        public IUnitFeatureMutable DeepClone()
        {
            var result = new StatusEffectsHolder(_unitId);
            foreach (var statusEffect in _statusEffects)
            {
                result._statusEffects.Add(statusEffect.DeepClone());
            }
            return result;
        }
    }
}