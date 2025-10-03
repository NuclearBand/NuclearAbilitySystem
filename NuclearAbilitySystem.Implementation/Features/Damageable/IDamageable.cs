namespace Nuclear.AbilitySystem
{
    public interface IDamageable : IUnitFeatureMutable
    {
        bool CanInteract { get; }
        int TakeDamage(int damage);
        int DealDamage(IUnit target, float multiplier);
    }
}