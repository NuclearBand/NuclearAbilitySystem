namespace Nuclear.AbilitySystem
{
    public interface IDistanceBetweenUnitsAbilityContext : IAbilityContext
    {
        float GetDistanceBetween(ICombatState combatState,  IUnitId unitId1, IUnitId unitId2);
    }
}