using System;

namespace Nuclear.AbilitySystem
{
    public enum AbilityActionTarget
    {
        FromSourceToTarget,
        FromTargetToSource,
        FromTargetToTarget,
        FromSourceToSource
    }
    
    public static class AbilityActionTargetExtensions
    {
        public static void UpdateAbilityActionTarget(AbilityActionTarget skillActionTarget, 
            UnitId? source, UnitId? target,
            out UnitId? effectSource, out UnitId? effectTarget)
        {
            switch (skillActionTarget)
            {
                case AbilityActionTarget.FromSourceToTarget:
                    effectTarget = target;
                    effectSource = source;
                    return;
                case AbilityActionTarget.FromTargetToSource:
                    effectTarget = source;
                    effectSource = target;
                    return;
                case AbilityActionTarget.FromTargetToTarget:
                    effectTarget = target;
                    effectSource = target;
                    return;
                case AbilityActionTarget.FromSourceToSource:
                    effectTarget = source;
                    effectSource = source;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}