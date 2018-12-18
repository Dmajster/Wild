﻿using GameEngine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Models.Gltf
{
    public class BasicInstancedMaterial : GlMaterial
    {
        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330
            layout(location = 0) in vec3 position;
            layout(location = 1) in mat4 model;

            uniform mat4 uViewProjection;

            void main(void)
            {  
                gl_Position = uViewProjection * model * vec4(position,1);
            }
        ";

        //uniform mat4 uViewProjection;

        // A simple fragment shader. Just a constant red color.
        const string FragmentShaderSource = @"
            #version 330
            out vec4 outputColor;
            void main(void)
            {
                outputColor = vec4(1.0, 0.0, 0.0, 1.0);
            }
        ";


        public GlBuffer VertexBuffer;
        public GlBuffer MatrixBuffer;

        public BasicInstancedMaterial()
        {
            var vertexShader = new GlShader(ShaderType.VertexShader);
            vertexShader.Load(VertexShaderSource);

            var fragmentShader = new GlShader(ShaderType.FragmentShader);
            fragmentShader.Load(FragmentShaderSource);

            Bind();
            LoadShader(vertexShader);
            LoadShader(fragmentShader);

            VertexBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            VertexBuffer.Bind();

            MatrixBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            MatrixBuffer.Bind();

            var vertexBufferLayout = new GlBufferLayout();
            vertexBufferLayout.Add(VertexAttribPointerType.Float, 3); //Vector3 3 floats

            var matrixBufferLayout = new GlBufferLayout();
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1); //Matrix 16 floats
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);

            var VertexArray = new GlVertexArray();
            VertexArray.AddBuffer(VertexBuffer, vertexBufferLayout);
            VertexArray.AddBuffer(MatrixBuffer, matrixBufferLayout);
        }
        
        public void Render(Model model, Matrix4[] matrices)
        {
            foreach( var mesh in model.Meshes)
            {
                Render(mesh, matrices);
            }
        }

        public void Render(Mesh mesh, Matrix4[] matrices )
        {
            Bind();

            VertexBuffer.Bind();
            VertexBuffer.SetData(mesh.Attributes["POSITION"].BufferData);

            MatrixBuffer.Bind();
            MatrixBuffer.SetData(matrices);

            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, mesh.Attributes["INDEX"].BufferData.Length, DrawElementsType.UnsignedShort, mesh.Attributes["INDEX"].BufferData, matrices.Length, 0);
        }
    }
}