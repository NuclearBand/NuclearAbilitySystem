namespace Nuclear.AbilitySystem
{
    public interface IAbilityCheck
    {
        bool CanExecute(IUnitId source, IUnitId? target, ICombatState state);
        void Execute(IUnitId source, IUnitId? target, ICombatState state);
        IAbilityCheck DeepClone();
    }
}