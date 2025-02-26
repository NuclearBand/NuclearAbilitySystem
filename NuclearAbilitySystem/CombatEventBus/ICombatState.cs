using System;
using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface ICombatState : IDisposable
    {
        IUnit GetUnit(IUnitId unitId);
        ReadOnlyCollection<IUnit> GetUnits();
        IAbilityContextHolder AbilityContextHolder { get; }
        ICombatEventBus CombatEventBus { get; }
        ICommandQueue CommandQueue { get; }
        ICombatState DeepClone();
    }
}