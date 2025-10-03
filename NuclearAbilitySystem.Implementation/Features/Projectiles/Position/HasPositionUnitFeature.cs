using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public sealed class HasPositionUnitFeature : IHasPositionUnitFeature
    {
        public HasPositionUnitFeature(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; }

        public void Connect(ICombatStateMutable combatState) { }
        public void Disconnect() { }

        public IUnitFeatureMutable DeepClone()
        {
            return new HasPositionUnitFeature(Position);
        }
    }
}