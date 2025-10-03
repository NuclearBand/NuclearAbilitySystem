namespace Nuclear.AbilitySystem
{
    public interface IDistanceBetweenUnitsAbilityContext : IAbilityContext
    {
        float GetDistanceBetween(UnitId unitId1, UnitId unitId2);
    }
}