using System;
using UnityEngine;

namespace _Assets.Scripts.Core.Infrastructure.Mono
{
    public class MonoService : MonoBehaviour
    {
        public event Action OnTick;

        private void Update() =>
            OnTick?.Invoke();
    }
}