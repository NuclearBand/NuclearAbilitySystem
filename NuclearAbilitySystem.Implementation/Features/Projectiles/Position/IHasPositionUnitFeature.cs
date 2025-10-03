using System.Numerics;

namespace Nuclear.AbilitySystem
{
    public interface IHasPositionUnitFeature : IUnitFeatureMutable
    {
        Vector2 Position { get; }
    }
}