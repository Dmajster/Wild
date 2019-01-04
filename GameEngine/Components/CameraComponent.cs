using System.Numerics;

namespace GameEngine.Components
{
    public class CameraComponent : Component
    {
        public float FieldOfView;
        public float Near;
        public float Far;
        public float AspectRatio;
        public Matrix4x4 Projection = new Matrix4x4();
    }
}
