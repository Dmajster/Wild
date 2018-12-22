using System.Numerics;

namespace GameEngine
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public static implicit operator Matrix4x4(Transform value)
        {
            return 
                Matrix4x4.CreateTranslation(value.Position) *
                Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(value.Rotation.X, value.Rotation.Y, value.Rotation.Z)) *
                Matrix4x4.CreateScale(value.Scale);
        }
    }
}
