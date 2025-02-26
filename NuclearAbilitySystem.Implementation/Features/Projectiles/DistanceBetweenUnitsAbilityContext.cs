using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public sealed class DistanceBetweenUnitsAbilityContext : IDistanceBetweenUnitsAbilityContext
    {
        private ICombatState? _combatState;

        public DistanceBetweenUnitsAbilityContext()
        {
        }

        public float GetDistanceBetween(IUnitId unitId1, IUnitId unitId2)
        {
            if (_combatState == null)
            {
                return 0f;
            }
            
            var unit1PositionFeature = _combatState.GetUnit(unitId1).GetCombatFeature<IHasPositionCombatFeature>();
            var unit2PositionFeature = _combatState.GetUnit(unitId2).GetCombatFeature<IHasPositionCombatFeature>();
            
            return Vector2.Distance(unit1PositionFeature.Position, unit2PositionFeature.Position);
        }

        public IAbilityContext DeepClone()
        {
            return new DistanceBetweenUnitsAbilityContext();
        }

        public void Subscribe(ICombatState combatState)
        {
            _combatState = combatState;
        }

        public void UnSubscribe()
        {
            _combatState = null;
        }
    }
}