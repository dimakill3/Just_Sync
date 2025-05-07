using _Assets.Scripts.Game.PlayerLogic;

namespace _Assets.Scripts.Networking.Services
{
    public interface IObjectsInitializer
    {
        void InitializePlayer(Player player);
    }
}