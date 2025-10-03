using System;
using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public abstract class Unit : IUnitMutable
    {
        protected readonly Dictionary<Type, IUnitFeatureMutable> _features = new();
        protected ICombatStateMutable? _combatState;
        
        protected Unit(UnitId id)
        {
            Id = id;
        }

        protected Unit(Unit unit)
        {
            Id = unit.Id;
            foreach (var (type, feature) in unit._features)
            {
                _features.Add(type, feature.DeepClone());
            }
        }

        public UnitId Id { get; }

        public T GetUnitFeature<T>() where T : IUnitFeature
        {
            return (T)_features[typeof(T)];
        }

        public abstract IUnitMutable DeepClone();

        public void Connect(ICombatStateMutable combatState)
        {
            _combatState = combatState;

            foreach (var combatFeature in _features)
            {
                combatFeature.Value.Connect(combatState);
            }
        }

        public void Disconnect()
        {
            _combatState = null;
            
            foreach (var combatFeature in _features)
            {
                combatFeature.Value.Disconnect();
            }
        }
    }
}
