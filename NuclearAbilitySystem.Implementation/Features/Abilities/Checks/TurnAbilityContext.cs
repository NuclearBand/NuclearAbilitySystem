namespace Nuclear.AbilitySystem
{
    public sealed class TurnAbilityContext : ITurnAbilityContext
    {
        public TurnAbilityContext(int turn)
        {
            Turn = turn;
        }

        public int Turn { get; private set; }
        public void NextTurn()
        {
            Turn++;
        }

        public IAbilityContext DeepClone()
        {
            return new TurnAbilityContext(Turn);
        }

        public void Connect(ICombatState combatState)
        {
        }

        public void Disconnect()
        {
        }
    }
}