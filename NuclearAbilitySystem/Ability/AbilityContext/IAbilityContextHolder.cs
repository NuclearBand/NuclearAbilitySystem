namespace Nuclear.AbilitySystem
{
    public interface IAbilityContextHolder
    {
        T GetContext<T>() where T : IAbilityContext;
    }

    public interface IAbilityContextHolderMutable : IAbilityContextHolder
    {
        IAbilityContextHolderMutable DeepClone();
        void Connect(ICombatState combatState);
        void Disconnect();
    }
}