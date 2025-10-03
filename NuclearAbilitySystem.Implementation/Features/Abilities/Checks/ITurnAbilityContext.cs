namespace Nuclear.AbilitySystem
{
    public interface ITurnAbilityContext : IAbilityContext
    {
        int Turn { get; }
        void NextTurn();
    }
}