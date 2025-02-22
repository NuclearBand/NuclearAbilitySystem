﻿using System;
using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface ICombatEventBus : IDisposable
    {
        public ICommandQueue CommandQueue { get; }
        IUnit GetUnit(IUnitId unitId);
        ReadOnlyCollection<IUnit> GetUnits();
        
        // (event previous result) returns new result
        void Subscribe<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
        TResult? Raise<TEvent, TResult>(TEvent @event) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
        void Unsubscribe<TEvent, TResult>(Func<TEvent, TResult?, TResult?> func) 
            where TEvent : ICombatEvent
            where TResult : ICombatEventResult;
    }
}