using System;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Rendering
{
    public sealed class GlShader : IDisposable
    {
        public readonly int ShaderId;
        public readonly ShaderType Type;

        public GlShader(ShaderType type)
        {
            ShaderId = GL.CreateShader(type);
            Type = type;
        }

        ~GlShader()
        {
            Dispose();
        }

        public void Load(string source)
        {
            GL.ShaderSource(ShaderId, source);
            GL.CompileShader(ShaderId);
            GL.GetShader(ShaderId, ShaderParameter.CompileStatus, out var success);

            if (success == 1) return;

            GL.GetShaderInfoLog(ShaderId, out var error);
            GL.DeleteShader(ShaderId);
            throw new Exception($"GlShader of type {Type} failed to compile! Error: {error}");
        }

        public void LoadFromFile(string path)
        {
            //TODO
        }

        public void Dispose()
        {
            GL.DeleteShader(ShaderId);
        }
    }
}
