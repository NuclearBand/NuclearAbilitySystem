namespace Nuclear.AbilitySystem
{
    public interface IUnitMutable : IUnit
    {
        void Connect(ICombatStateMutable combatState);
        void Disconnect();
        IUnitMutable DeepClone();
    }
}