using System.Numerics;

namespace GameEngine.Camera
{
    public class PerspectiveCamera : ICamera
    {
        public Transform Transform = new Transform(); //TODO Create Transform class that deals with this nonsense
        public float AspectRatio = 16f / 9f;
        public float FieldOfView = 60f;
        public float Near = 0.1f;
        public float Far = 100f;

        public Matrix4x4 Projection
        {
            get
            {
                var view = Matrix4x4.CreateLookAt(
                    new Vector3(5, 5, 5),
                    new Vector3(0, 0, 0),
                    Vector3.UnitY
                );
                var projection = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * (float)System.Math.PI / 180f, AspectRatio, Near, Far);
                return view * projection;
            }
        }
    }
}
