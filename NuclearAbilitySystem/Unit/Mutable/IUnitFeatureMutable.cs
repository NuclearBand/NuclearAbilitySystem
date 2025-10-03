namespace Nuclear.AbilitySystem
{
    public interface IUnitFeatureMutable : IUnitFeature
    {
        void Connect(ICombatStateMutable combatState);
        void Disconnect();
        IUnitFeatureMutable DeepClone();
    }
}