using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface ICombatState
    {
        IUnit GetUnit(UnitId unitId);
        ReadOnlyCollection<IUnit> GetUnits();
        IAbilityContextHolder AbilityContextHolder { get; }
        IAbilityExecutionResult ExecuteAbilityOnClone(UnitId sourceId, int abilityIndex, UnitId? targetId);
    }

    public interface ICombatStateMutable : ICombatState
    {
        ICombatEventBus CombatEventBus { get; }
        ICommandQueue CommandQueue { get; }
        void Connect();
        void Disconnect();
        IAbilityExecutionResult ExecuteAbility(UnitId sourceId, int abilityIndex, UnitId? targetId);
        ICombatStateMutable DeepClone();
    }
}