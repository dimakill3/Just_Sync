using _Assets.Scripts.Game.CharacterBaseLogic.Health;
using _Assets.Scripts.Game.CharacterBaseLogic.Movement;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.CharacterBaseLogic.DamageDealer
{
    public class HeightDamageDealer : NetworkBehaviour
    {
        private float _damageableHeight;
        private HealthComponent _health;
        private IGroundLander _groundLander;

        public void Initialize(float damageableHeight, IGroundLander groundLander, HealthComponent health)
        {
            _damageableHeight = damageableHeight;
            _groundLander = groundLander;
            _health = health;

            if (Object.HasStateAuthority)
                _groundLander.Landed += OnLanded;
        }

        private void OnDestroy()
        {
            if (Object != null && Object.HasStateAuthority && _groundLander != null)
                _groundLander.Landed -= OnLanded;
        }

        private void OnLanded(float height)
        {
            if (!Object.HasStateAuthority)
                return;
            
            if (height >= _damageableHeight)
                _health.TakeDamage(height - _damageableHeight);
        }
    }
}