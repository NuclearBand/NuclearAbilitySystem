namespace Nuclear.AbilitySystem
{
    public interface ICombatFeature
    {
        ICombatFeature DeepClone();
        void Subscribe(ICombatState combatState);
        void UnSubscribe();
    }
}