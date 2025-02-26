using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nuclear.AbilitySystem
{
    public sealed class CombatState : ICombatState
    {
        private readonly List<IUnit> _units;
        private readonly IAbilityContextHolder _abilityContextHolder;
        private readonly CommandQueue _commandQueue;
        private readonly CombatEventBus _combatEventBus;

        public CombatState(List<IUnit> units, IAbilityContextHolder abilityContextHolder)
        {
            _units = units;
            _abilityContextHolder = abilityContextHolder;
            _commandQueue = new CommandQueue();
            _combatEventBus = new CombatEventBus();

            Subscribe();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        public IAbilityContextHolder AbilityContextHolder => _abilityContextHolder;
        public ICombatEventBus CombatEventBus => _combatEventBus;
        public ICommandQueue CommandQueue => _commandQueue;

        public IUnit GetUnit(IUnitId unitId)
        {
            return _units.First(u => EqualityComparer<IUnitId>.Default.Equals(u.Id, unitId)); 
        }

        public ReadOnlyCollection<IUnit> GetUnits() => _units.AsReadOnly();
        
        private void Subscribe()
        {
            _units.ForEach(u => u.Subscribe(this));
        }

        private void UnSubscribe()
        {
            _units.ForEach(u => u.UnSubscribe());
        }
        
        public ICombatState DeepClone()
        {
            return new CombatState(_units.Select(u => u.DeepClone()).ToList(),
                _abilityContextHolder.DeepClone());
        }
    }
}