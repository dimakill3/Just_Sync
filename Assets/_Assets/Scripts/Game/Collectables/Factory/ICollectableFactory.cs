using _Assets.Scripts.Game.Configs;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Game.Collectables.Factory
{
    public interface ICollectableFactory
    {
        UniTask<Collectable> GetCollectablePrefab(CollectableConfig collectableConfig);
        Collectable CreateCollectable(Collectable collectablePrefab);
    }
}