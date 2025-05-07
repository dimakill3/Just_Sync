using System;
using _Assets.Scripts.Game.CharacterBaseLogic.Collect;
using _Assets.Scripts.Game.Configs;
using Fusion;
using UnityEngine;

namespace _Assets.Scripts.Game.Collectables
{
    public class Collectable : NetworkBehaviour, ICollectable
    {
        public event Action<Collectable> Collected;
        
        public CollectableType CollectableType => _collectableConfig.CollectableType;
        [Networked] private bool IsCollected { get; set; }
        private CollectableConfig _collectableConfig;
        
        public void Initialize(CollectableConfig config) =>
            _collectableConfig = config;

        public void Collect(ICollector collector)
        {
            if (!Object.HasStateAuthority || IsCollected)
                return;
            
            IsCollected = true;
            foreach (var effect in _collectableConfig.Effects)
                effect.Apply(collector);
            
            RPC_OnCollected();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Object.HasInputAuthority)
                return;
            
            var collector = other.GetComponent<ICollector>();
            if (collector != null)
                Collect(collector);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnCollected() =>
            Collected?.Invoke(this);
    }
}