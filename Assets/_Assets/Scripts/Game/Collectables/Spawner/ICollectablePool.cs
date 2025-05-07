using System;
using _Assets.Scripts.Game.Configs;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Game.Collectables.Spawner
{
    public interface ICollectablePool : IDisposable
    {
        UniTask<Collectable> GetCollectable(CollectableConfig config);
    }
}