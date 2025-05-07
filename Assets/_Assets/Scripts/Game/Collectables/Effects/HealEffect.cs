using _Assets.Scripts.Game.CharacterBaseLogic.Collect;
using UnityEngine;

namespace _Assets.Scripts.Game.Collectables.Effects
{
    [CreateAssetMenu(fileName = "HealEffectConfig", menuName = "Configs/HealEffectConfig")]
    public class HealEffect : CollectableEffect
    {
        public float HealAmount;

        public override void Apply(ICollector collector) =>
            collector.Heal(HealAmount);
    }
}