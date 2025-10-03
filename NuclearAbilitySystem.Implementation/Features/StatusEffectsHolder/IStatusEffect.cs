namespace Nuclear.AbilitySystem
{
    public interface IStatusEffect 
    {
    }

    public interface IStatusEffectMutable : IStatusEffect
    {
        IStatusEffectMutable DeepClone();
        void Connect(ICombatStateMutable combatState);
        void Disconnect();
    }
}