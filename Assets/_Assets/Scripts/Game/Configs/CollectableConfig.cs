using System.Collections.Generic;
using _Assets.Scripts.Game.Collectables;
using _Assets.Scripts.Game.Collectables.Effects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace _Assets.Scripts.Game.Configs
{
    [CreateAssetMenu(fileName = "CollectableConfig", menuName = "Configs/CollectableConfig")]
    public class CollectableConfig : ScriptableObject
    {
        public CollectableType CollectableType;
        public List<CollectableEffect> Effects = new();
        public AssetReference AddressableId;
    }
}