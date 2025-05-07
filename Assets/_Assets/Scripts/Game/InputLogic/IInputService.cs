using System;
using UnityEngine;

namespace _Assets.Scripts.Game.InputLogic
{
    public interface IInputService : IDisposable
    {
        public event Action<Vector2> MoveInput;
        public event Action<Vector2> LookInput;
        public event Action<bool> JumpInput;
    }
}