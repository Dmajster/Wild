using ECS.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ECS
{
    public sealed class ComponentManager
    {
        public Entity AddEntity(IComponent[] components)
        {
            if (components.Length == 0)
            {
                throw new InvalidDataException("Entities without components are a no no!");
            }

            //Make new entity and calculate it's table id
            var entity = new Entity { TableId = TableId(components) };

            //If we don't have a component group make one
            if (!_components.ContainsKey(entity.TableId))
            {
                _components.Add(entity.TableId, new ComponentGroup(components.ToArray()));
            }

            //Add new entity and get the row id
            entity.RowId = _components[entity.TableId].AddEntity(components.ToArray());

            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            _components[entity.TableId].RemoveEntity(entity);
        }

        public void AddComponent<T>(Entity entity, T value) where T : struct, IComponent
        {
            if (!_components.ContainsKey(entity.TableId))
            {
                throw new InvalidDataException("Entity has invalid table Id");
            }

            //Collect all entity data in component group
            var components = _components[entity.TableId].GetEntity(entity).ToList();

            //Add the new component to the list
            components.Add(value);

            //Remove entity from current component group
            _components[entity.TableId].RemoveEntity(entity);

            //Calculate new table id
            entity.TableId = TableId(components);

            //If we don't have a component group make one
            if (!_components.ContainsKey(entity.TableId))
            {
                _components.Add(entity.TableId, new ComponentGroup(components.ToArray()));
            }

            //Add new entity and get the row id
            entity.RowId = _components[entity.TableId].AddEntity(components.ToArray());
        }

        public void RemoveComponent<T>(Entity entity) where T : struct, IComponent
        {
            if (!_components.ContainsKey(entity.TableId))
            {
                throw new InvalidDataException("Entity has invalid table Id");
            }

            //Collect all entity data in component group
            var components = _components[entity.TableId].GetEntity(entity).ToList();

            //Remove component
            components.RemoveAll(component => component.GetType() == typeof(T));

            if (components.Count == 0)
            {
                throw new InvalidDataException("Entities without components are a no no!");
            }

            //Remove entity from current component group
            _components[entity.TableId].RemoveEntity(entity);

            //Calculate new table id
            entity.TableId = TableId(components);

            //If we don't have a component group make one
            if (!_components.ContainsKey(entity.TableId))
            {
                _components.Add(entity.TableId, new ComponentGroup(components.ToArray()));
            }

            //Add new entity and get the row id
            entity.RowId = _components[entity.TableId].AddEntity(components.ToArray());
        }

        public T GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            if (!_components.ContainsKey(entity.TableId))
            {
                throw new InvalidDataException("Trying to access component that doesn't exist in this component group");
            }
            return _components[entity.TableId].GetComponent<T>(entity);
        }

        public void SetComponent<T>(Entity entity, T value) where T : struct, IComponent
        {
            if (!_components.ContainsKey(entity.TableId))
            {
                throw new InvalidDataException("Trying to access component that doesn't exist in this component group");
            }
            _components[entity.TableId].SetComponent(entity, value);
        }

        private Dictionary<int, ComponentGroup> _components = new Dictionary<int, ComponentGroup>();

        public int TableId(IEnumerable<IComponent> components) => components
                .Select(component => component.GetType().GUID.ToString())
                .OrderBy(component => component)
                .Aggregate((s, s1) => s + s1)
                .GetHashCode();

        public Entity GetEntities<T>(T group)
        {
            throw new NotImplementedException();
        }
    }
}
