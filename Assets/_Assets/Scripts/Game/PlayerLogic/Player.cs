using System;
using _Assets.Scripts.Game.CharacterBaseLogic.Collect;
using _Assets.Scripts.Game.CharacterBaseLogic.DamageDealer;
using _Assets.Scripts.Game.CharacterBaseLogic.Health;
using _Assets.Scripts.Game.Configs;
using _Assets.Scripts.Game.InputLogic;
using _Assets.Scripts.Game.PlayerLogic.Movement;
using _Assets.Scripts.Game.Utils;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.PlayerLogic
{
    public class Player : NetworkBehaviour
    {
        public event Action<PlayerRef> OnDeath;
        
        [SerializeField] private HealthComponent health;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerLooking looking;
        [SerializeField] private HealthView healthView;
        [SerializeField] private HeightDamageDealer heightDamageDealer;
        [SerializeField] private Collector collector;
        [SerializeField] private PlayerInputController inputController;
        [SerializeField] private HeroAnimator animator;
        
        public void InitializeOnLocal(PlayerConfig playerConfig, Transform cameraTransform, IInputService inputService)
        {
            healthView.Initialize(health);
            inputController.Initialize(inputService);
            movement.Initialize(playerConfig.MoveSpeed, playerConfig.JumpHeight, playerConfig.LookSensitivity);
            looking.Initialize(playerConfig.LookSensitivity, cameraTransform, inputService);
            looking.enabled = true;
            animator.Initialize(health);
        }

        public void InitializeOnOthers(PlayerConfig playerConfig)
        {
            health.Initialize(playerConfig.MaxHealth);
            healthView.Initialize(health);
            collector.Initialize(health);
            heightDamageDealer.Initialize(playerConfig.DamageableHeight, movement, health);
            looking.enabled = false;
            
            health.OnDeath += HandleDeath;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Respawn()
        {
            health.Initialize(health.MaxHealth);
            movement.Warp(MapUtil.GetRandomMapPosition());
        }

        private void OnDestroy()
        {
            health.OnDeath -= HandleDeath;
        }

        private void HandleDeath() =>
            OnDeath?.Invoke(Object.InputAuthority);
    }
}