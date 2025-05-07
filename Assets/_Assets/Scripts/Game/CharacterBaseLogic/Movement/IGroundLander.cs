using System;

namespace _Assets.Scripts.Game.CharacterBaseLogic.Movement
{
    public interface IGroundLander
    {
        public event Action<float> Landed;
    }
}