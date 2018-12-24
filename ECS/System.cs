using ECS.Interfaces;

namespace ECS
{
    public abstract class ComponentSystem
    {
        protected ComponentManager ComponentManager;

        public static IComponent[] GetEntities<T>(T value)
        {


            return new IComponent[0];
        }

        public abstract void OnUpdate();
    }
}
