using ECS;
using ECS.Interfaces;
using Game.Components;
using Game.Resources.Materials;
using GameEngine;
using GameEngine.Camera;
using GameEngine.Rendering.Models;
using GameEngine.Rendering.Models.Gltf;
using OpenTK;
using System;
using System.Numerics;
using OpenTK.Graphics.OpenGL4;

namespace Game
{
    public sealed class Game
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

        public void OnResize(object sender, EventArgs e)
        {
        }

        public Model Model;
        public BasicInstancedMaterial Material;
        public PerspectiveCamera Camera;

        public void OnLoad(object sender, EventArgs e)
        {
            ComponentManager componentManager = new ComponentManager();
            SystemManager systemManager = new SystemManager(componentManager);

            Model = GltfImporter.Load("./Resources/Models/monkey.gltf");
            Material = new BasicInstancedMaterial();

            Camera = new PerspectiveCamera()
            {
                AspectRatio = (float)WindowManager.Width / WindowManager.Height,
            };

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.CullFace(CullFaceMode.FrontAndBack);

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public void OnFrameUpdate(object sender, FrameEventArgs e)
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            Material.Render(Model, Camera, new[] { Matrix4x4.Identity });

            WindowManager.Context.SwapBuffers();
        }

        public void OnPhysicsUpdate(object sender, PhysicsUpdateEventArgs e)
        {
        }
    }
}
