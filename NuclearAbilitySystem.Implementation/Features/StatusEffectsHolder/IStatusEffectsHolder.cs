﻿using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface IStatusEffectsHolder : ICombatFeature
    {
        ReadOnlyCollection<IStatusEffect> StatusEffects { get; }

        void AddStatusEffect(IStatusEffect statusEffect);
        void RemoveStatusEffect(IStatusEffect statusEffect);

    }
}