using System;
using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public sealed class AbilityContextHolder : IAbilityContextHolderMutable
    {
        private readonly Dictionary<Type, IAbilityContext> _contexts = new();

        public AbilityContextHolder()
        {
            _contexts.Add(typeof(ITurnAbilityContext), new TurnAbilityContext(0));
            _contexts.Add(typeof(IDistanceBetweenUnitsAbilityContext), new DistanceBetweenUnitsAbilityContext());
        }
        
        private AbilityContextHolder(AbilityContextHolder abilityContextHolder)
        {
            foreach (var (type, context) in abilityContextHolder._contexts)
            {
                _contexts.Add(type, context.DeepClone());
            }
        }
        public T GetContext<T>() where T : IAbilityContext
        {
            return (T)_contexts[typeof(T)];
        }

        public IAbilityContextHolderMutable DeepClone()
        {
            return new AbilityContextHolder(this);
        }

        public void Connect(ICombatState combatState)
        {
            foreach (var context in _contexts.Values)
            {
                context.Connect(combatState);
            }
        }

        public void Disconnect()
        {
            foreach (var context in _contexts.Values)
            {
                context.Disconnect();
            }
        }
    }
}