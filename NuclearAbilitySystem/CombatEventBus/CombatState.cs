using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nuclear.AbilitySystem
{
    public sealed class CombatState : ICombatStateMutable
    {
        private readonly List<IUnitMutable> _units;
        private readonly IAbilityContextHolderMutable _abilityContextHolder;
        private CommandQueue _commandQueue;
        private CombatEventBus _combatEventBus;

        public CombatState(List<IUnitMutable> units, IAbilityContextHolderMutable abilityContextHolder)
        {
            _units = units;
            _abilityContextHolder = abilityContextHolder;
            _commandQueue = new CommandQueue();
            _combatEventBus = new CombatEventBus();
        }

        public IAbilityContextHolder AbilityContextHolder => _abilityContextHolder;
        public IAbilityExecutionResult ExecuteAbilityOnClone(UnitId sourceId, int abilityIndex, UnitId? targetId)
        {
            var resultState = DeepClone();
            resultState.Connect();
            var result =  resultState.ExecuteAbility(sourceId, abilityIndex, targetId);
            resultState.Disconnect();
            return result;
        }

        public IAbilityExecutionResult ExecuteAbility(UnitId sourceId, int abilityIndex, UnitId? targetId)
        {
            GetUnit(sourceId)
                .GetUnitFeature<IAbilitiesHolder>()
                .Abilities[abilityIndex]
                .Execute(sourceId, targetId, this);
            return new AbilityExecutionResult(CommandQueue.CalculateCommandQueue(), this);
        }

        public ICombatEventBus CombatEventBus => _combatEventBus;
        public ICommandQueue CommandQueue => _commandQueue;

        public IUnit GetUnit(UnitId unitId)
        {
            return _units.First(u => u.Id == unitId); 
        }

        public ReadOnlyCollection<IUnit> GetUnits() => _units.OfType<IUnit>().ToList().AsReadOnly();
        
        public void Connect()
        {
            _units.ForEach(u => u.Connect(this));
            _abilityContextHolder.Connect(this);
        }

        public void Disconnect()
        {
            _units.ForEach(u => u.Disconnect());
            _abilityContextHolder.Disconnect();
        }
        
        public ICombatStateMutable DeepClone()
        {
            return new CombatState(_units.Select(u => u.DeepClone()).ToList(),
                _abilityContextHolder.DeepClone());
        }

        public void Reset()
        {
            Disconnect();
            _commandQueue = new CommandQueue();
            _combatEventBus = new CombatEventBus();
            Connect();
        }
    }
}