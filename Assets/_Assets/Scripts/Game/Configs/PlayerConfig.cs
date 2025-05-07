using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Assets.Scripts.Game.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public float MaxHealth = 100f;
        public float MoveSpeed = 5f;
        public float JumpHeight = 3f;
        public float LookSensitivity = 2;
        public float DamageableHeight = 10;

        public AssetReference AddressableId;
    }
}