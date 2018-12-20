using Jitter;
using Jitter.Collision;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using GameEngine.Camera;
using GameEngine.Extensions;
using GameEngine.Materials;
using GameEngine.Models.Gltf;
using GameEngine.Models;

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

        public BasicInstancedMaterial Material;
        
        public World World;
        public CollisionSystem CollisionSystem;

        public PerspectiveCamera Camera;
        public Model Model;

        public virtual void OnLoad(object sender, EventArgs e)
        {
            Model = GltfImporter.Load("./Game/Resources/Models/monkey.gltf");

            Material = new BasicInstancedMaterial();
            Material.Link();

            GL.CullFace(CullFaceMode.FrontAndBack);

            Camera = new PerspectiveCamera()
            {
                AspectRatio = (float) WindowManager.Width / WindowManager.Height
            };

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

            /*
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
            var bla = Matrix4.CreateScale(3, 3, 3).Cast();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //var matrices = Entities.Select(entity => entity.Transform.Value).ToArray();
            var matrices = new Matrix4[]
            {
                Matrix4.CreateScale(3,3,3),
                Matrix4.CreateTranslation(5,0,0)
            };

            Material.Render(Model, Camera, matrices);
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