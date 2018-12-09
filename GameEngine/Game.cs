using GameEngine.Components;
using GameEngine.Mesh.Primitives;
using GameEngine.Rendering;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using glTFLoader;
using GameEngine.Models.Gltf;
using Jitter.LinearMath;
using Vector3 = OpenTK.Vector3;

namespace GameEngine
{
    public class Game
    {
        public readonly WindowManager WindowManager;
        public readonly TickManager TickManager;

        public Game()
        {
            WindowManager = new WindowManager();
            WindowManager.Load += OnLoad;
            WindowManager.Resize += OnResize;
            WindowManager.UpdateFrame += OnFrameUpdate;


            TickManager = new TickManager(WindowManager);
            TickManager.PhysicsUpdated += OnPhysicsUpdate;

            WindowManager.Run();
        }

        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, WindowManager.Width, WindowManager.Height);
        }

        // A simple vertex shader possible. Just passes through the position vector.
        const string VertexShaderSource = @"
            #version 330
            layout(location = 0) in vec4 position;
            layout(location = 1) in mat4 model;

            uniform mat4 uViewProjection;

            void main(void)
            {  
                gl_Position = uViewProjection * model * position;
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

        public GlMaterial Material;
        public GlBuffer VertexBuffer;
        public GlBuffer MatrixBuffer;
        public GlVertexArray VertexArray;

        public Entity[] Entities = new Entity[3];
        public World World;
        public CollisionSystem CollisionSystem;

        public Models.Gltf.Mesh TestMesh;

        public virtual void OnLoad(object sender, EventArgs e)
        {
            var modelFile = Model.Load("./Game/Resources/Models/box.gltf");
            TestMesh = modelFile.CreateMesh();


            var vertexShader = new GlShader(ShaderType.VertexShader);
            vertexShader.Load(VertexShaderSource);

            var fragmentShader = new GlShader(ShaderType.FragmentShader);
            fragmentShader.Load(FragmentShaderSource);

            Material = new GlMaterial();
            Material.Bind();
            Material.LoadShader(vertexShader);
            Material.LoadShader(fragmentShader);
            Material.Link();

            VertexBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            VertexBuffer.Bind();
            VertexBuffer.SetData(TestMesh.Attributes["POSITION"]);

            MatrixBuffer = new GlBuffer(BufferTarget.ArrayBuffer);
            MatrixBuffer.Bind();

            var vertexBufferLayout = new GlBufferLayout();
            vertexBufferLayout.Add(VertexAttribPointerType.Float, 3); //Vector4 4 floats

            var matrixBufferLayout = new GlBufferLayout();
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1); //Matrix 16 floats
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);
            matrixBufferLayout.Add(VertexAttribPointerType.Float, 4, divisor: 1);

            VertexArray = new GlVertexArray();
            VertexArray.AddBuffer(VertexBuffer, vertexBufferLayout);
            VertexArray.AddBuffer(MatrixBuffer, matrixBufferLayout);

            //GL.CullFace(CullFaceMode.Front);

            var projection = Matrix4.CreatePerspectiveFieldOfView(60f * (float)System.Math.PI / 180f, (float)WindowManager.Width / WindowManager.Height, 0.1f, 100f);
            var view = Matrix4.LookAt(
                new Vector3(10, 10, 10),
                new Vector3(0, 0, 0),
                Vector3.UnitY
            );
            var viewProjection = view * projection;

            var projectionViewLocation = GL.GetUniformLocation(Material.ProgramId, "uViewProjection");
            GL.ProgramUniformMatrix4(Material.ProgramId, projectionViewLocation, false, ref viewProjection);

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

            /*
            Entities[0] = new Entity()
            {
                Mesh = new MeshComponent()
                {
                    Value = new Cube()
                },
                RigidBody = new RigidBodyComponent()
                {
                    Value = new RigidBody(new BoxShape(1, 1, 1))
                },
                Transform = new TransformComponent()
                {
                    Value = Matrix4.CreateTranslation(0, 0, 0)
                }
            };

            Entities[1] = new Entity()
            {
                Mesh = new MeshComponent()
                {
                    Value = new Cube()
                },
                RigidBody = new RigidBodyComponent()
                {
                    Value = new RigidBody(new BoxShape(1, 1, 1))
                },
                Transform = new TransformComponent()
                {
                    Value = Matrix4.CreateTranslation(0, 1, 0)
                }
            };

            Entities[2] = new Entity()
            {
                Mesh = new MeshComponent()
                {
                    Value = new Cube()
                },
                RigidBody = new RigidBodyComponent()
                {
                    Value = new RigidBody(new BoxShape(1, 1, 1))
                },
                Transform = new TransformComponent()
                {
                    Value = Matrix4.CreateTranslation(0, 0, 0)
                }
            };

            CollisionSystem = new CollisionSystemSAP();
            World = new World(CollisionSystem);

            Shape floor = new BoxShape(100.0f, 1.0f, 100.0f);
            RigidBody floorRigidBody = new RigidBody(floor) {Position = new JVector(0,-1,0) ,AffectedByGravity = false};
            World.AddBody(floorRigidBody);
            foreach (var entity in Entities)
            {
                var position = entity.Transform.Value.ExtractTranslation();

                entity.RigidBody.Value.Position = new JVector(position.X, position.Y, position.Z);
                World.AddBody(entity.RigidBody.Value);
            }

            */
        }
        

        public virtual void OnFrameUpdate(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //var matrices = Entities.Select(entity => entity.Transform.Value).ToArray();
            var matrices = new Matrix4[]
            {
                Matrix4.Identity,
                Matrix4.CreateTranslation(3,0,0),
                Matrix4.CreateTranslation(0,0,5),
            };

            MatrixBuffer.Bind();
            MatrixBuffer.SetData(matrices);
            
            VertexArray.Bind();

            Material.Bind();
            GL.DrawElementsInstancedBaseInstance( PrimitiveType.Triangles, TestMesh.Attributes["INDEX"].Length, DrawElementsType.UnsignedInt, TestMesh.Attributes["INDEX"], matrices.Length, 0);
            
            WindowManager.Context.SwapBuffers();
        }

        public virtual void OnPhysicsUpdate(object sender, PhysicsUpdateEventArgs e)
        {
            //World.Step(e.TimeStep, true);


            /*
            foreach (var entity in Entities)
            {
                JVector position = entity.RigidBody.Value.Position;
                entity.Transform.Value = Matrix4.CreateTranslation(new OpenTK.Vector3(position.X, position.Y, position.Z));
            }*/
        }
    }
}