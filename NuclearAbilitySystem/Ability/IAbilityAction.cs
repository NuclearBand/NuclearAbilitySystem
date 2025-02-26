namespace Nuclear.AbilitySystem
{
    public interface IAbilityAction
    {
        void Execute(IUnitId sourceId, IUnitId? targetId, ICombatState combatState);
        IAbilityAction DeepClone();
    }
}

