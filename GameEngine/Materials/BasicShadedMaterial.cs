using System.Numerics;
using GameEngine.Components;
using GameEngine.Extensions;
using GameEngine.Rendering;
using GameEngine.Rendering.Models;
using OpenTK.Graphics.OpenGL4;

namespace GameEngine.Materials
{
    public class BasicShadedMaterial : GlMaterial
    {
        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330
            layout(location = 0) in vec3 position;
            layout(location = 1) in vec3 normal;
            layout(location = 2) in mat4 model;

            uniform mat4 uViewProjection;
            uniform mat4 uDepthMVP;
            uniform mat4 uDepthBias;

            
            out vec3 FragPos;
            out vec3 Normal;
            void main(void)
            {  
                gl_Position = uViewProjection * model * vec4(position,1);
                FragPos = vec3(model * vec4(position, 1.0));
                Normal = normal;
            }
        ";

        //uniform mat4 uViewProjection;

        // A simple fragment shader. Just a constant red color.
        const string FragmentShaderSource = @"
            #version 330
            in vec3 FragPos;  
            in vec3 Normal;

            uniform vec3 uColor = vec3(1.0,0,0);

            out vec4 FragColor;
            void main(void)
            {
                vec3 norm = normalize(Normal);
                vec3 lightPos = vec3(0,1000,100);
                vec3 lightColor = vec3(0.8,0.95,1);

                float ambientStrength = 0.1;
                vec3 ambient = ambientStrength * lightColor;

                vec3 lightDir = normalize(lightPos - FragPos);  

                float diff = max(dot(norm, lightDir), 0.0);
                vec3 diffuse = diff * lightColor;

                vec3 result = (ambient + diffuse) * uColor;
                FragColor = vec4(result, 1.0);
            }
        ";

        // vec4(1.0, 0.0, 0.0, 1.0); vec4(1.0, 0.0, 0.0, 1.0) * vec4( normals, 1.0 ) *

        public GlVertexArray VertexArray;

        public GlBuffer PositionBuffer;
        public GlBuffer NormalBuffer;
        public GlBuffer MatrixBuffer;

        public Vector3 Color = new Vector3(1,0,0);

        public BasicShadedMaterial()
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
            
            Vector3 lightInvDir = new Vector3(0.5f, 2, 2);

            // Compute the MVP matrix from the light's point of view
            Matrix4x4 depthProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(-10, 10, -10, 10, -10, 20);
            Matrix4x4 depthViewMatrix = Matrix4x4.CreateLookAt(lightInvDir, new Vector3(0, 0, 0),Vector3.UnitY);
            Matrix4x4 depthModelMatrix = Matrix4x4.Identity;;
            Matrix4x4 depthMVP = depthProjectionMatrix * depthViewMatrix * depthModelMatrix;

            var projectionCast = camera.GetComponent<CameraComponent>().Projection().Cast();
            var projectionViewLocation = GL.GetUniformLocation(ProgramId, "uViewProjection");
            GL.ProgramUniformMatrix4(ProgramId, projectionViewLocation, false, ref projectionCast);

            var depthProjectionCast = depthMVP.Cast();
            var depthProjectionViewLocation = GL.GetUniformLocation(ProgramId, "uDepthMVP");
            GL.ProgramUniformMatrix4(ProgramId, depthProjectionViewLocation, false, ref depthProjectionCast);

            Matrix4x4 biasMatrix = new Matrix4x4(
                0.5f, 0.0f, 0.0f, 0.0f,
                0.0f, 0.5f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.5f, 0.0f,
                0.5f, 0.5f, 0.5f, 1.0f
            );
            Matrix4x4 depthBiasMVP = biasMatrix * depthMVP;

            var depthBiasCast = depthBiasMVP.Cast();
            var depthBiasLocation = GL.GetUniformLocation(ProgramId, "uDepthBias");
            GL.ProgramUniformMatrix4(ProgramId, depthBiasLocation, false, ref depthBiasCast);

            var colorLocation = GL.GetUniformLocation(ProgramId, "uColor");
            var colorCast = Color.CastRendering();
            GL.ProgramUniform3(ProgramId,colorLocation,ref colorCast);

            GL.DrawElementsInstancedBaseInstance(PrimitiveType.Triangles, mesh.Attributes["INDEX"].BufferData.Length / 2, DrawElementsType.UnsignedShort, mesh.Attributes["INDEX"].BufferData, matrices.Length, 0);
        }
    }
}
