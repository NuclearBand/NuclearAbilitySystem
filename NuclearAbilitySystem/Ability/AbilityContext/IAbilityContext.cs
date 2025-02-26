namespace Nuclear.AbilitySystem
{
    public interface IAbilityContext
    {
        IAbilityContext DeepClone();
        void Subscribe(ICombatState combatState);
        void UnSubscribe();
    }
}