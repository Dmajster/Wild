using System;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Rendering
{
    public sealed class GlVertexArray : IDisposable
    {
        public readonly int Id;
        public int LastIndex { get; private set; }

        public GlVertexArray()
        {
            Id = GL.GenVertexArray();
        }

        public void Bind()
        {
            GL.BindVertexArray(Id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(Id);
        }

        public void AddBuffer(GlBuffer glBuffer, GlBufferLayout layout)
        {
            Bind();
            glBuffer.Bind();
            
            int offset = 0;
            for (var i = 0; i < layout.BufferElements.Count; i++)
            {
                var element = layout.BufferElements[i];
                GL.EnableVertexAttribArray(LastIndex);
                GL.VertexAttribDivisor(LastIndex,element.Divisor);
                GL.VertexAttribPointer(LastIndex,
                    element.Count,
                    element.Type,
                    element.Normalized,
                    layout.Stride,
                    offset
                );

                LastIndex++;
                offset += element.Count * GlBufferLayout.GetAttributeSize(element.Type);
            }
        }
    }
}
