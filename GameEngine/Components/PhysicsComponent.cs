using GameEngine.Extensions;
using Jitter.Dynamics;

namespace GameEngine.Components
{
    public class PhysicsComponent : Component
    {
        public RigidBody RigidBody;

        public override void OnLoad()
        {
            GameObject.Scene.AddRigidBody(RigidBody);
        }

        public override void OnFixedUpdate()
        {
        }
    }
}
