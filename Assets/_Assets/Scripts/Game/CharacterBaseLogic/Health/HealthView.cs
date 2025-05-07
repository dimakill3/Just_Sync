using UnityEngine;
using UnityEngine.UI;

namespace _Assets.Scripts.Game.CharacterBaseLogic.Health
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image progress;
        
        private HealthComponent _health;

        public void Initialize(HealthComponent health)
        {
            _health = health;
            HandleHealthChanged(_health.CurrentHealth, _health.MaxHealth);
            
            _health.OnHealthChanged += HandleHealthChanged;
        }

        private void HandleHealthChanged(float currentHealth, float maxHealth) =>
            progress.fillAmount = currentHealth / maxHealth;
    }
}