namespace Nuclear.AbilitySystem
{
    public interface IAbilityAction
    {
        void Execute(UnitId sourceId, UnitId? targetId, ICombatStateMutable combatState);
        IAbilityAction DeepClone();
    }
}

