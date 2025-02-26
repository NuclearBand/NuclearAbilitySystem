namespace Nuclear.AbilitySystem
{
    public interface IStatusEffect
    {
        IStatusEffect DeepClone();
        void Subscribe(ICombatState combatState);
        void UnSubscribe();
    }
}