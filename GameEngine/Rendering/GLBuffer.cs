using System;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Rendering
{
    public sealed class GlBuffer : IDisposable
    {
        public readonly int Id;
        public readonly BufferTarget Type;

        public GlBuffer(BufferTarget type)
        {
            Id = GL.GenBuffer();
            GL.BindBuffer(type, Id);
            Type = type;
        }

        public void Bind()
        {
            GL.BindBuffer(Type,Id);
        }

        public void Unbind()
        {
            GL.BindBuffer(Type, 0);
        }

        public void SetData<T>(T[] data, BufferUsageHint usageHint = BufferUsageHint.StaticDraw) where T : struct
        {
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), data, usageHint);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
