using System.Linq;

namespace Nuclear.AbilitySystem
{
    public static class DefenderExtensions
    {
        public static bool IsDefender(this IUnit unit)
        {
            return unit.GetUnitFeature<IStatusEffectsHolder>().StatusEffects.Any(s => s is Defender);
        }
        
        public static void AddDefenderStatusEffect(this IUnit unit)
        {
            if (!unit.IsDefender())
            {
                unit.GetUnitFeature<IStatusEffectsHolder>().AddStatusEffect(
                    new Defender(unit.Id));
            }
        }
    }
}