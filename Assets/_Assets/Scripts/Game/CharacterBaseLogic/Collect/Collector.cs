using _Assets.Scripts.Game.CharacterBaseLogic.Health;
using UnityEngine;

namespace _Assets.Scripts.Game.CharacterBaseLogic.Collect
{
    public class Collector : MonoBehaviour, ICollector
    {
        private HealthComponent _healthComponent;

        public void Initialize(HealthComponent healthComponent) =>
            _healthComponent = healthComponent;

        public void Heal(float healAmount) =>
            _healthComponent.TakeDamage(-healAmount);
    }
}