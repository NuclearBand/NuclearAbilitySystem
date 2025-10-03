using System;

namespace Nuclear.AbilitySystem
{
    public interface ICombatEventBus
    {
        // (event previous result) returns new result
        void Connect<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
        TResult? Raise<TEvent, TResult>(TEvent @event) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
        void Disconnect<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
    }
}