using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface IStatusEffectsHolder : IUnitFeatureMutable
    {
        ReadOnlyCollection<IStatusEffect> StatusEffects { get; }

        void AddStatusEffect(IStatusEffectMutable statusEffectMutable);
        void RemoveStatusEffect(IStatusEffectMutable statusEffectMutable);

    }
}