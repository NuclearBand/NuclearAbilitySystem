using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public sealed class DistanceBetweenUnitsAbilityContext : IDistanceBetweenUnitsAbilityContext
    {

        public DistanceBetweenUnitsAbilityContext()
        {
        }

        public float GetDistanceBetween(ICombatState combatState, IUnitId unitId1, IUnitId unitId2)
        {
            var unit1PositionFeature = combatState.GetUnit(unitId1).GetCombatFeature<IHasPositionCombatFeature>();
            var unit2PositionFeature = combatState.GetUnit(unitId2).GetCombatFeature<IHasPositionCombatFeature>();
            
            return Vector2.Distance(unit1PositionFeature.Position, unit2PositionFeature.Position);
        }

        public IAbilityContext DeepClone()
        {
            return new DistanceBetweenUnitsAbilityContext();
        }
    }
}