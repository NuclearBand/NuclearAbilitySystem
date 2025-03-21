namespace Nuclear.AbilitySystem
{
    public interface IAbilityContextHolder
    {
        T GetContext<T>() where T : IAbilityContext;
        IAbilityContextHolder DeepClone();
        void Subscribe(ICombatState combatState);
        void UnSubscribe();
    }
}