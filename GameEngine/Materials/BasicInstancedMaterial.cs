using System.Drawing;
using System.Numerics;
using GameEngine.Components;
using GameEngine.Extensions;
using GameEngine.Rendering;
using GameEngine.Rendering.Models;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Materials
{
    public class BasicInstancedMaterial : GlMaterial
    {
        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330
            layout(location = 0) in vec3 position;
            layout(location = 1) in vec3 normal;
            layout(location = 2) in mat4 model;

            uniform mat4 uViewProjection;
            

            out vec3 normals;

            void main(void)
            {  
                normals = normal;
                gl_Position = uViewProjection * model * vec4(position,1);
            }
        ";

        //uniform mat4 uViewProjection;

        // A simple fragment shader. Just a constant red color.
        const string FragmentShaderSource = @"
            #version 330
            in vec3 normals;

            uniform vec3 uColor = vec3(1.0,0,0);

            out vec4 outputColor;
            void main(void)
            {
                outputColor = vec4(uColor, 1.0) * vec4(normals, 1.0);
            }
        ";

        // vec4(1.0, 0.0, 0.0, 1.0); vec4(1.0, 0.0, 0.0, 1.0) * vec4( normals, 1.0 ) *

        public GlVertexArray VertexArray;

        public GlBuffer PositionBuffer;
        public GlBuffer NormalBuffer;
        public GlBuffer MatrixBuffer;

        public Vector3 Color = new Vector3(1,0,0);

        public BasicInstancedMaterial()
        {
            var vertexShader = new GlShader(ShaderType.VertexShader);
            vertexShader.LoadSource(VertexShaderSource);

            var fragmentShader = new GlShader(ShaderType.FragmentShader);
            fragmentShader.LoadSource(FragmentShaderSource);

            Bind();
            LoadShader(vertexShader);
            LoadShader(fragmentShader);
            Link();

            PositionBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            PositionBuffer.Bind();

            NormalBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            NormalBuffer.Bind();

            MatrixBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            MatrixBuffer.Bind();

            var positionBufferLayout = new GlBufferLayout();
            positionBufferLayout.Add(VertexAttribPointerType.Float, 3); //Vector3 3 floats

            var normalBufferLayout = new GlBufferLayout();
            normalBufferLayout.Add(VertexAttribPointerType.Float, 3);

            var matrixBufferLayout = new GlBufferLayout();
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1); //Matrix 16 floats
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);

            VertexArray = new GlVertexArray();
            VertexArray.AddBuffer(PositionBuffer, positionBufferLayout);
            VertexArray.AddBuffer(NormalBuffer, normalBufferLayout);
            VertexArray.AddBuffer(MatrixBuffer, matrixBufferLayout);
        }

        public override void Render(Mesh mesh, Camera camera, Matrix4x4[] matrices)
        {
            Bind();
            VertexArray.Bind();

            PositionBuffer.Bind();
            PositionBuffer.SetData(mesh.Attributes["POSITION"].BufferData);

            NormalBuffer.Bind();
            NormalBuffer.SetData(mesh.Attributes["NORMAL"].BufferData);

            MatrixBuffer.Bind();
            MatrixBuffer.SetData(matrices);
            
            var projectionCast = camera.GetComponent<CameraComponent>().Projection().Cast();
            var projectionViewLocation = GL.GetUniformLocation(ProgramId, "uViewProjection");
            GL.ProgramUniformMatrix4(ProgramId, projectionViewLocation, false, ref projectionCast);

            var colorLocation = GL.GetUniformLocation(ProgramId, "uColor");
            var colorCast = Color.CastRendering();
            GL.ProgramUniform3(ProgramId,colorLocation,ref colorCast);

            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, mesh.Attributes["INDEX"].BufferData.Length / 2, DrawElementsType.UnsignedShort, mesh.Attributes["INDEX"].BufferData, matrices.Length, 0);
        }
    }
}
