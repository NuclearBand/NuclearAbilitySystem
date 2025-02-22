using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nuclear.AbilitySystem
{
    public record ProjectileName(string Value);
    public sealed class CreateProjectileAbilityAction : IAbilityAction
    {
        public CreateProjectileAbilityAction(ProjectileName projectileName, 
            float velocity,
            ReadOnlyCollection<IAbilityAction>? onEnd = null)
        {
            ProjectileName = projectileName;
            Velocity = velocity;
            OnEnd = onEnd;
        }

        public ProjectileName ProjectileName { get; }
        public float Velocity { get; }
        public ReadOnlyCollection<IAbilityAction>? OnEnd { get; }
        public void Execute(IUnitId sourceId, IUnitId? targetId, 
            ICombatEventBus context, IAbilityContextHolder abilityContextHolder)
        {
            var source = context.GetUnit(sourceId);
            var target = context.GetUnit(targetId!);
            
            if (!source.GetCombatFeature<IDamageable>().CanInteract || 
                !target.GetCombatFeature<IDamageable>().CanInteract)
                return;

            var distanceContext = abilityContextHolder.GetContext<IDistanceBetweenUnitsAbilityContext>();
            var distance = distanceContext.GetDistanceBetween(sourceId, targetId!);
            var flyingTime = (int)(Math.Round(distance / Velocity)); 
            context.CommandQueue.Add(new CreateProjectileCombatCommand(sourceId, targetId!, flyingTime, context.CommandQueue.Time));

            if (OnEnd != null)
            {
                context.CommandQueue.AddTimeEvent(context.CommandQueue.Time + flyingTime, () =>
                {
                    foreach (var onEndAction in OnEnd)
                    {
                        onEndAction.Execute(sourceId, targetId, context, abilityContextHolder);
                    }
                });
            }
        }

        public IAbilityAction DeepClone()
        {
            return new CreateProjectileAbilityAction(ProjectileName, Velocity,
                OnEnd?.Select(a => a.DeepClone()).ToList().AsReadOnly());
        }
    }
}