namespace Nuclear.AbilitySystem
{
    public interface ICooldownAbilityCheck : IAbilityCheck
    {
        int GetCooldownTimer(ITurnAbilityContext context);
    }
}