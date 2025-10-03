using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public sealed class DistanceBetweenUnitsAbilityContext : IDistanceBetweenUnitsAbilityContext
    {
        private ICombatState? _combatState;

        public DistanceBetweenUnitsAbilityContext()
        {
        }

        public float GetDistanceBetween(UnitId unitId1, UnitId unitId2)
        {
            if (_combatState == null)
            {
                return 0f;
            }
            
            var unit1PositionFeature = _combatState.GetUnit(unitId1).GetUnitFeature<IHasPositionUnitFeature>();
            var unit2PositionFeature = _combatState.GetUnit(unitId2).GetUnitFeature<IHasPositionUnitFeature>();
            
            return Vector2.Distance(unit1PositionFeature.Position, unit2PositionFeature.Position);
        }

        public IAbilityContext DeepClone()
        {
            return new DistanceBetweenUnitsAbilityContext();
        }

        public void Connect(ICombatState combatState)
        {
            _combatState = combatState;
        }

        public void Disconnect()
        {
            _combatState = null;
        }
    }
}