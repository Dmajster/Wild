using GameEngine.Components;

namespace GameEngine
{
    public struct Entity
    {
        public TransformComponent Transform;
        public MeshComponent Mesh;
        public RigidBodyComponent RigidBody;
    }
}
