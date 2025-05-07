using UnityEngine;

namespace _Assets.Scripts.Game.Configs
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        public int MaxCollectablesOnMap = 10;
    }
}