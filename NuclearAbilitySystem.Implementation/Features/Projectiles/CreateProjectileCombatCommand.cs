namespace Nuclear.AbilitySystem
{
    public record CreateProjectileCombatCommand(UnitId SourceId, UnitId TargetId, int FlyingTime, int Time) : ICombatCommand;
}