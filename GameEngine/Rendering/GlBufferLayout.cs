using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Rendering
{
    public struct GlBufferLayoutElement
    {
        public VertexAttribPointerType Type;
        public int Count;
        public bool Normalized;
        public int Divisor;
    }

    public sealed class GlBufferLayout
    {
        public readonly List<GlBufferLayoutElement> BufferElements = new List<GlBufferLayoutElement>();

        public int Stride { get; private set; }

        public void Add(VertexAttribPointerType type, int count = 1, bool normalized = false, int divisor = 0)
        {
            BufferElements.Add(new GlBufferLayoutElement()
            {
                Type = type,
                Count = count,
                Normalized = normalized,
                Divisor = divisor
            });

            Stride += GetAttributeSize(type) * count;
        }

        public static int GetAttributeSize(VertexAttribPointerType attribute)
        {
            switch (attribute)
            {
                case VertexAttribPointerType.Float:
                    return 4;

                case VertexAttribPointerType.Byte:
                    return 1;
            }
            
            throw new Exception($"Vertex attribute pointer type not implemented yet! {attribute}");
        }
    }
}
