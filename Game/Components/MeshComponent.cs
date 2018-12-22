using ECS.Interfaces;
using GameEngine.Rendering.Models;

namespace Game.Components
{
    public struct MeshComponent : IComponent
    {
        public Mesh Value;
    }
}
