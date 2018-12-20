using System.Numerics;

namespace GameEngine.Camera
{
    public interface ICamera
    {
        Matrix4x4 Projection { get; }
    }
}
