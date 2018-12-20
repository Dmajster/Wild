using System.Numerics;

namespace GameEngine.Camera
{
    public class PerspectiveCamera : ICamera
    {
        public Matrix4x4 Transform; //TODO Create Transform class that deals with this nonsense
        public float AspectRatio = 16f/9f;
        public float FieldOfView = 60f;
        public float Near = 0.1f;
        public float Far = 100f;

        public Matrix4x4 Projection
        {
            get
            {
                var projection = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * (float)System.Math.PI / 180f, AspectRatio, Near, Far);
                var view = Matrix4x4.CreateLookAt(
                    new Vector3(10, 10, 10), //TODO FIX THIS MESS
                    new Vector3(0, 0, 0),
                    Vector3.UnitY
                );
                return view * projection;
            }
        }
    }
}
