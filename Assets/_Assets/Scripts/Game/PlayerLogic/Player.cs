using System;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Game.CharacterBaseLogic.Collect;
using _Assets.Scripts.Game.CharacterBaseLogic.DamageDealer;
using _Assets.Scripts.Game.CharacterBaseLogic.Health;
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
        
        public void Initialize(GameConfig gameConfig, Transform cameraTransform, IInputService inputService)
        {
            var heroConfig = gameConfig.PlayerConfig;

            inputController.Initialize(inputService);
            health.Initialize(heroConfig.MaxHealth);
            movement.Initialize(heroConfig.MoveSpeed, heroConfig.JumpHeight);
            looking.Initialize(heroConfig.LookSensitivity, cameraTransform);
            healthView.Initialize(health);
            heightDamageDealer.Initialize(heroConfig.DamageableHeight, movement, health);
            collector.Initialize(health);
            animator.Initialize(health);

            health.OnDeath += HandleDeath;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Respawn()
        {
            health.Initialize(health.MaxHealth);
            movement.Warp(MapUtil.GetRandomMapPosition());
        }

        private void OnDestroy() =>
            health.OnDeath -= HandleDeath;

        private void HandleDeath() =>
            OnDeath?.Invoke(Object.InputAuthority);
    }
}