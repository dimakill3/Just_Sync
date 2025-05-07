using _Assets.Scripts.Game.CharacterBaseLogic.Collect;

namespace _Assets.Scripts.Game.Collectables
{
    public interface ICollectable
    {
        void Collect(ICollector collector);
    }
}