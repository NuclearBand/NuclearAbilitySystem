using System.Collections.Generic;

namespace Nuclear.AbilitySystem
{
    public static class SimpleAbility
    {
        public static IAbility Create()
        {
            return new Ability(new List<IAbilityAction>()
                {
                    new DealDamageAbilityAction(AbilityActionTarget.FromSourceToTarget, 1)
                }.AsReadOnly(),
                new List<IAbilityCheck>()
                {
                }.AsReadOnly());
        }
    }
}