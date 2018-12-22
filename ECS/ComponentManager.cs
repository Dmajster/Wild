using System;
using System.Collections.Generic;
using ECS.Interfaces;

namespace ECS
{
    public sealed class ComponentManager
    {
        public void AddComponent<T>(T value) where T : IComponent { }

        public void RemoveComponent<T>(int entityId) where T : IComponent{ }

        public void GetComponent<T>(int entityId) where T : IComponent { }

        public void SetComponent<T>(int entityId, T value) where T : IComponent { }

        private Dictionary<Type, List<IComponent>> _components = new Dictionary<Type, List<IComponent>>();
    }
}
