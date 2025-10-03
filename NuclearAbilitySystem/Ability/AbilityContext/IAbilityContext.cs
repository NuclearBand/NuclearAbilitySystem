namespace Nuclear.AbilitySystem
{
    public interface IAbilityContext
    {
        IAbilityContext DeepClone();
        void Connect(ICombatState combatState);
        void Disconnect();
    }
}