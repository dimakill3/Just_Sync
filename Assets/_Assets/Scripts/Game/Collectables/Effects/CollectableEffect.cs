using _Assets.Scripts.Game.CharacterBaseLogic.Collect;
using UnityEngine;

namespace _Assets.Scripts.Game.Collectables.Effects
{
    public abstract class CollectableEffect : ScriptableObject
    {
        public abstract void Apply(ICollector collector);
    }
}