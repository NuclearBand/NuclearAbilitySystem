namespace Nuclear.AbilitySystem
{
    public record DeathCombatCommand(UnitId TargetId, int Time) : ICombatCommand;
}