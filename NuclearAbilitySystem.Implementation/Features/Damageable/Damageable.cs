using System;

namespace Nuclear.AbilitySystem
{
    public sealed class Damageable : IDamageable
    {
        private readonly Func<IUnit, bool> _canInteract;
        private readonly Func<IUnit, int, int> _doDamage;
        private readonly Func<IUnit, int> _getDamage;
        
        private ICombatState? _combatState;
        private IUnit? _unit;

        public Damageable(IUnitId unitId,
            Func<IUnit, bool> canInteract,
            Func<IUnit, int, int> doDamage,
            Func<IUnit, int> getDamage)
        {
            UnitId = unitId;
            
            _canInteract = canInteract;
            _doDamage = doDamage;
            _getDamage = getDamage;
        }

        public void Subscribe(ICombatState combatState)
        {
            _combatState = combatState;
            _unit = _combatState.GetUnit(UnitId);
        }

        public void UnSubscribe()
        {
            _combatState = null;
            _unit = null;
        }

        public IUnitId UnitId { get; }
        
        public bool CanInteract
        {
            get
            {
                if (_unit == null)
                {
                    throw new();
                }
                return _canInteract.Invoke(_unit);
            }
        }

        public int TakeDamage(int damage)
        {
            if (!CanInteract || _unit == null)
            {
                throw new();
            }
            
            return _doDamage.Invoke(_unit, damage);
        }

        public int DealDamage(IUnit target, float multiplier)
        {
            if (_combatState == null || _unit == null)
            {
                throw new();
            }
            
            var targetDamageable = target.GetCombatFeature<IDamageable>();
            if (!CanInteract || !targetDamageable.CanInteract)
            {
                return 0;
            }

            var damage = (int)MathF.Round(_getDamage.Invoke(_unit) * multiplier);
            
            if (_combatState.CombatEventBus.Raise<PreDamageEvent, DamageEventResult>(new PreDamageEvent(_unit, target, damage)) 
                is { ContinueExecution: false})
            {
                return 0;
            }
            
            var result = targetDamageable.TakeDamage(damage);
            
            // по-хорошему перенести выше и ввести CalcTakeDamage damage?
            _combatState.CommandQueue.Add(new AttackCombatCommand(_unit.Id, targetDamageable.UnitId, result, _combatState.CommandQueue.Time)); 
            if (!targetDamageable.CanInteract) // а это убрать внутрь TakeDamage
            {
                _combatState.CommandQueue.Add(new DeathCombatCommand(targetDamageable.UnitId, _combatState.CommandQueue.Time));
            }
            
            _combatState.CombatEventBus.Raise<AfterDamageEvent, DamageEventResult>(new AfterDamageEvent(_unit, target, result));
            return result;
        }

        public ICombatFeature DeepClone()
        {
            return new Damageable(UnitId, _canInteract, _doDamage, _getDamage);
        }
    }
}