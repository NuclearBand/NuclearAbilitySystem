using System;
using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    internal sealed class CombatEventBus : ICombatEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _combatEvents = new();

        public void Connect<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult
        {
            if (_combatEvents.TryGetValue(typeof(TEvent), out var list))
            {
                list.Add(func);
            }
            else
            {
                _combatEvents.Add(typeof(TEvent), new List<Delegate>(){func});
            }
        }

        public TResult? Raise<TEvent, TResult>(TEvent @event) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult
        {
            var result = default(TResult);
            if (_combatEvents.TryGetValue(typeof(TEvent), out var list))
            {
                foreach (var subscriber in list)
                {
                    result = ((Func<TEvent, TResult?, TResult?>)subscriber).Invoke(@event, result);
                }
            }

            return result;
        }

        public void Disconnect<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult
        {
            if (_combatEvents.TryGetValue(typeof(TEvent), out var list))
            {
                list.Remove(func);
                if (list.Count == 0)
                {
                    _combatEvents.Remove(typeof(TEvent));
                }
            }
        }
    }
}