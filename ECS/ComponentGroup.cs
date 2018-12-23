using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ECS.Interfaces;

namespace ECS
{
    public sealed class ComponentGroup
    {
        private readonly Dictionary<Type,List<IComponent>> _buffers = new Dictionary<Type, List<IComponent>>();

        public ComponentGroup(IComponent[] components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            foreach (var component in components)
            {
                _buffers.Add(component.GetType(),new List<IComponent>()); //TODO make this dynamic
            }
        }

        public int AddEntity(IComponent[] components)
        {
            if( components.Length != _buffers.Keys.Count || components.Any(component => !_buffers.ContainsKey(component.GetType())) )
            {
                throw new InvalidDataException("Trying to add entity to wrong component group");
            }

            foreach (var component in components)
            {
                _buffers[component.GetType()].Add(component);
            }

            return _buffers.ElementAt(0).Value.Count -1;
        }

        public T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            if (!_buffers.ContainsKey(typeof(T)))
            {
                throw new InvalidDataException("Trying to access component that doesn't exist in this component group");
            }

            return (T)_buffers[typeof(T)][entity.RowId];
        }

        public void SetComponent<T>(Entity entity, T value) where T : struct, IComponent
        {
            if (!_buffers.ContainsKey(typeof(T)))
            {
                throw new InvalidDataException("Trying to access component that doesn't exist in this component group");
            }

            _buffers[typeof(T)][entity.RowId] = value;
        }

        public IComponent[] GetEntity(Entity entity)
        {
            return _buffers.Values.Select(buffer => buffer[entity.RowId]).ToArray();
        }

        private static void Remove(IList<IComponent> buffer, int index)
        {
            if (index < 0 || index >= buffer.Count)
            {
                throw new IndexOutOfRangeException();
            }

            buffer[index] = buffer[buffer.Count - 1];
            buffer.RemoveAt(buffer.Count-1);
        }

        public void RemoveEntity(Entity entity)
        {
            for (var i = 0; i < _buffers.Count; i++)
            {
                Remove(_buffers.ElementAt(i).Value, entity.RowId);
            }
        }
    }
}
