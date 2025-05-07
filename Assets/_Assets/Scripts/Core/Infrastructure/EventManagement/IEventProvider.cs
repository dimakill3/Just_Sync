using System;

namespace _Assets.Scripts.Core.Infrastructure.EventManagement
{
    public interface IEventProvider
    {
        public void Invoke<T>(T arg) where T : IEvent;
        public void Subscribe<T>(Action<T> action) where T : IEvent;
        public void UnSubscribe<T>(Action<T> action) where T : IEvent;
    }
}