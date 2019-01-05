using System;
using System.Numerics;

namespace GameEngine.Components
{
    public class CameraComponent : Component
    {
        public float FieldOfView = 60;
        public float Near = 0.1f;
        public float Far = 2000f;
        public float AspectRatio = 4/3f;


        public Vector3 CameraFront;

        public float DegToRad(float degrees)
        {
            return degrees * 3.14f / 180;
        }

        public Matrix4x4 Projection()
        {
            var front = new Vector3
            {
                X = (float)Math.Cos(DegToRad(GameObject.Transform.Rotation.X)) * (float)Math.Cos(DegToRad(GameObject.Transform.Rotation.Y)),
                Y = (float)Math.Sin(DegToRad(GameObject.Transform.Rotation.Y)),
                Z = (float)Math.Sin(DegToRad(GameObject.Transform.Rotation.X)) * (float)Math.Cos(DegToRad(GameObject.Transform.Rotation.Y))
            };
            CameraFront = Vector3.Normalize(front);

            //Console.WriteLine($"{GameObject.Transform.Position} {GameObject.Transform.Rotation}");

            var view = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * 3.14f / 180, AspectRatio, Near, Far);
            
            var projection = Matrix4x4.CreateLookAt(GameObject.Transform.Position, GameObject.Transform.Position + CameraFront, Vector3.UnitY);

            return  projection * view;
        }
    }
}
