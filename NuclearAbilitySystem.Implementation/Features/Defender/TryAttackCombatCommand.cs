namespace Nuclear.AbilitySystem
{
    public record TryAttackCombatCommand(UnitId AttackerId, UnitId TargetId, int Time) : ICombatCommand;
}