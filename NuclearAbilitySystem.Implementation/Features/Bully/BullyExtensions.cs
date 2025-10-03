using System.Linq;

namespace Nuclear.AbilitySystem
{
    public static class BullyExtensions
    {
        public static bool IsBully(this IUnit unit)
        {
            return unit.GetUnitFeature<IStatusEffectsHolder>().StatusEffects.Any(s => s is Bully);
        }
        
        public static void AddBullyStatusEffect(this IUnit unit)
        {
            if (!unit.IsBully())
            {
                unit.GetUnitFeature<IStatusEffectsHolder>().AddStatusEffect(
                    new Bully(unit.Id));
            }
        }
    }
}