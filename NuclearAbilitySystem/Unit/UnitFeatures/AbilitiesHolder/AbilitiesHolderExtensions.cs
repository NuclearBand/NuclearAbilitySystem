namespace Nuclear.AbilitySystem
{
    public static class AbilitiesHolderExtensions
    {
        public static void AddAbility(this IUnit unit, IAbility ability)
        {
            unit.GetUnitFeature<IAbilitiesHolder>().AddAbility(ability);
        }
    }
}