using System;

namespace _Assets.Scripts.Game.CharacterBaseLogic.Health
{
    public interface IDamageable
    {
        event Action<float, float> OnHealthChanged;
        event Action OnDeath;
        
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsAlive { get; }
    
        void TakeDamage(float damage);
    }
}