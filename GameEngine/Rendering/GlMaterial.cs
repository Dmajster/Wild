using System;
using System.Collections.Generic;
using System.Numerics;
using GameEngine.Rendering.Models;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Rendering
{
    public class GlMaterial : IDisposable
    {
        public readonly int ProgramId;
        public readonly Dictionary<ShaderType, GlShader> LoadedShaders = new Dictionary<ShaderType, GlShader>();

        public GlMaterial()
        {
            ProgramId = GL.CreateProgram();
        }

        ~GlMaterial()
        {
            Dispose();
        }

        public void Link()
        {
            foreach (var loadedShader in LoadedShaders.Values)
            {
                GL.AttachShader(ProgramId, loadedShader.ShaderId);
            }

            GL.LinkProgram(ProgramId);
            GL.GetProgram(ProgramId, GetProgramParameterName.LinkStatus, out var successful);
            if (successful != 1)
            {
                GL.GetProgramInfoLog(ProgramId, out var error);
                GL.DeleteProgram(ProgramId);
                throw new Exception($"Program failed to link! Error: {error}");
            }

            foreach (var loadedShader in LoadedShaders.Values)
            {
                GL.DetachShader(ProgramId, loadedShader.ShaderId);
            }
        }

        public void Bind()
        {
            GL.UseProgram(ProgramId);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void LoadShader(GlShader glShader)
        {
            //DELETE OLD SHADER ON REWRITE
            if (LoadedShaders.ContainsKey(glShader.Type))
            {
                var oldShader = LoadedShaders[glShader.Type];
                GL.DetachShader(ProgramId, oldShader.ShaderId);
                oldShader.Dispose();

                LoadedShaders.Remove(glShader.Type);
            }

            LoadedShaders.Add(glShader.Type, glShader);
            GL.AttachShader(ProgramId, glShader.ShaderId);
        }

        public void Dispose()
        {
            foreach (var loadedShaders in LoadedShaders.Values)
            {
                GL.DetachShader(ProgramId, loadedShaders.ShaderId);
                loadedShaders.Dispose();
            }

            GL.DeleteProgram(ProgramId);
        }

        public virtual void Render(Mesh model, Camera camera, Matrix4x4[] matrices)
        {
        }
    }
}
