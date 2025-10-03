namespace Nuclear.AbilitySystem
{
    public interface IUnit
    {
        UnitId Id { get; }
        T GetUnitFeature<T>() where T : IUnitFeature;
    }
}