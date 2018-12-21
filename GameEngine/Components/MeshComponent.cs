using GameEngine.Models;

namespace GameEngine.Components
{
    public struct MeshComponent : IComponent<MeshComponent>
    {
        public Mesh Value;
    }
}
