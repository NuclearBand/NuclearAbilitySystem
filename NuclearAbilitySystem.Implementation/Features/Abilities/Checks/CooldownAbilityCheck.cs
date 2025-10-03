using System;

namespace Nuclear.AbilitySystem
{
    public sealed class CooldownAbilityCheck : ICooldownAbilityCheck
    {
        private readonly int _cooldown;
        private int _lastCastTime;

        public CooldownAbilityCheck(int cooldown, int? startValue = null)
        {
            _cooldown = cooldown;
            _lastCastTime = -startValue ?? -cooldown;
        }

        public bool CanExecute(UnitId source, UnitId? target, ICombatState context)
        {
            var timeContext = context.AbilityContextHolder.GetContext<ITurnAbilityContext>();
            return GetCooldownTimer(timeContext) == 0;
        }

        public void Execute(UnitId source, UnitId? target, ICombatState context)
        {
            var timeContext = context.AbilityContextHolder.GetContext<ITurnAbilityContext>();
            _lastCastTime = timeContext.Turn;
        }

        public int GetCooldownTimer(ITurnAbilityContext turnContext)
        {
            return Math.Max(0, _lastCastTime + _cooldown + 1 - turnContext.Turn);
        }

        public IAbilityCheck DeepClone()
        {
            var result = new CooldownAbilityCheck(_cooldown);
            result._lastCastTime = _lastCastTime;
            return result;
        }
    }
}