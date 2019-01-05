using GameEngine.Extensions;
using Jitter.Dynamics;

namespace GameEngine.Components
{
    public class PhysicsComponent : Component
    {
        public RigidBody RigidBody;

        public override void OnLoad()
        {
            GameObject.Scene.AddRigidBody(ref RigidBody);
        }

        public override void OnFixedUpdate()
        {
            RigidBody.AffectedByGravity = true;
        }
    }
}
