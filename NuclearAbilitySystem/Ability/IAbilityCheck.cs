namespace Nuclear.AbilitySystem
{
    public interface IAbilityCheck
    {
        bool CanExecute(UnitId source, UnitId? target, ICombatState state);
        void Execute(UnitId source, UnitId? target, ICombatState state);
        IAbilityCheck DeepClone();
    }
}