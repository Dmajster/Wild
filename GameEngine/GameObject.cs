using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameEngine.Components;

namespace GameEngine
{
    public class Component
    {
        public GameObject GameObject;

        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }

        public T AddComponent<T>(T value = null) where T : Component, new()
        {
            return GameObject.AddComponent<T>(value);
        }

        public void RemoveComponent<T>() where T : Component
        {
            GameObject.RemoveComponent<T>();
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }
    };

    public class GameObject
    {
        public static Scene Scene;
        public readonly TransformComponent Transform;
        private readonly List<Component> _components = new List<Component>();

        public GameObject()
        {
            Scene.AddGameObject(this);

            Transform = AddComponent(new TransformComponent());
        }

        public void Destroy()
        {
            Scene.RemoveGameObject(this);
        }

        public T AddComponent<T>(T value = null) where T : Component, new()
        {
            var component = value ?? new T();
            component.GameObject = this;
            _components.Add(component);
            component.OnLoad();
            return component;
        }

        public T GetComponent<T>() where T : Component
        {
            return (T) _components.FirstOrDefault(component => component.GetType() == typeof(T));
        }

        public void RemoveComponent<T>() where T : Component
        {
            _components.RemoveAll(component => component.GetType() == typeof(T));
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

        public void ENGINE_OnUpdate()
        {
            OnUpdate();

            foreach (var component in _components)
            {
                component.OnUpdate();
            }
        }

        public void ENGINE_OnFixedUpdate()
        {
            OnFixedUpdate();

            foreach (var component in _components)
            {
                component.OnFixedUpdate();
            }
        }
    }
}
