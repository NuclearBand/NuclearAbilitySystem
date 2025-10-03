namespace Nuclear.AbilitySystem
{
    public record DefendCombatCommand(UnitId DefenderId, UnitId TargetId, int Time) : ICombatCommand;
}