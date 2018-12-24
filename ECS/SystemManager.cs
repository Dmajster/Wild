using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS
{
    public sealed class SystemManager
    {
        private readonly List<ISystem> _systems = new List<ISystem>();
        private readonly ComponentManager _componentManager;

        public IEnumerable<Type> GetSystemTypes => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ISystem).IsAssignableFrom(p));

        public SystemManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }
        
        public void RegisterSystems()
        {
            foreach (var systemType in GetSystemTypes)
            {
                _systems.Add((ISystem)Activator.CreateInstance(systemType));
            }
        }

        public void UpdateSystems()
        {   
            foreach (var system in _systems)
            {
                system.OnUpdate();
            }
        }
    }
}
