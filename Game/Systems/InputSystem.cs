using ECS;
using Game.Components;

namespace Game.Systems
{
    public struct Group
    {
        public InputComponent Input;
    }

    public class InputSystem : ComponentSystem
    {
        public override void OnUpdate()
        {
            ComponentManager.GetEntities(typeof(Group));

            throw new System.NotImplementedException();
        }
    }
}
