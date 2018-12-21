using System.Numerics;

namespace GameEngine
{
    public struct Transform
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public static implicit operator Matrix4x4(Transform value)
        {
            return 
                Matrix4x4.CreateTranslation(value.Position) *
                Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(value.Rotation.X, value.Rotation.Y, value.Rotation.Z)) *
                Matrix4x4.CreateScale(value.Scale);
        }
    }
}
