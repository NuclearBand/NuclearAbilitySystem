using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    internal sealed class CommandQueue : ICommandQueue
    {
        private readonly List<ICombatCommand> _commands = new();
        private readonly TimeEvents _timeEvents;

        public int Time { get; private set; }
        public int FinalTime { get; private set; }

        internal CommandQueue()
        {
            _timeEvents = new TimeEvents();
        }

        public void Add(ICombatCommand combatCommand)
        {
            _commands.Add(combatCommand);
        }

        public void AddTimeEvent(int time, Action action)
        {
            _timeEvents.AddTimeEvent(time, action);
        }

        public void UpdateFinalTime(int time)
        {
            FinalTime = Math.Max(FinalTime, time);
        }

        public ReadOnlyCollection<ICombatCommand> CalculateCommandQueue()
        {
            while (!_timeEvents.IsMaxTime(Time))
            {
                UpdateTime();
            }
            return _commands.AsReadOnly();
        }

        private void UpdateTime()
        {
            Time += 1;
            UpdateFinalTime(Time);
           _timeEvents.UpdateTime(Time);
        }
    }
}