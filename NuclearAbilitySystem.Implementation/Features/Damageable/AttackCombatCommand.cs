namespace Nuclear.AbilitySystem
{
    public record AttackCombatCommand(UnitId AttackerId, UnitId TargetId, int Damage, int Time) : ICombatCommand;
}