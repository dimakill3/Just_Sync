using System;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.CharacterBaseLogic.Health
{
    public class HealthComponent : NetworkBehaviour, IDamageable
    {
        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsAlive => _currentHealth > 0;
        
        private float _currentHealth;
        private float _maxHealth;

        public void Initialize(float maxHealth)
        {
            if (Object.HasStateAuthority)
            {
                _maxHealth = maxHealth;
                _currentHealth = maxHealth;
                RPC_UpdateHealth(_currentHealth, _maxHealth);
            }
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive)
                return;
            
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0f, _maxHealth);
            RPC_UpdateHealth(_currentHealth, _maxHealth);
            
            if (_currentHealth <= 0)
                RPC_OnDeath();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_UpdateHealth(float newCurrent, float newMax)
        {
            _maxHealth = newMax;
            _currentHealth = newCurrent;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnDeath() =>
            OnDeath?.Invoke();
    }
}