namespace Nuclear.AbilitySystem
{
    public interface IUnit 
    {
        IUnitId Id { get; }
        T GetCombatFeature<T>() where T : ICombatFeature;
        IUnit DeepClone();
        void Subscribe(ICombatState combatState);
        void UnSubscribe();
    }

    public interface IUnitId { }
}