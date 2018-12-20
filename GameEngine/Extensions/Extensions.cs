using System.Numerics;

namespace GameEngine.Extensions
{
    public static class Extensions
    {
        public static Matrix4x4 Cast(this OpenTK.Matrix4 value)
        {
            return new Matrix4x4(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                value.M41, value.M42, value.M43, value.M44
            );
        }
        public static OpenTK.Matrix4 Cast(this Matrix4x4 value)
        {
            return new OpenTK.Matrix4(
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                value.M41, value.M42, value.M43, value.M44
            );
        }

        public static Vector3 Cast(this OpenTK.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static Vector3 Cast(this Jitter.LinearMath.JVector value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
    }
}