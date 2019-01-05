using GameEngine;
using GameEngine.Components;
using GameEngine.Gltf;
using GameEngine.Rendering.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Game.Components;
using GameEngine.Materials;
using GameEngine.Physics;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace Game
{
    public sealed class Game
    {
        public readonly WindowManager WindowManager;
        public readonly GameLoop GameLoop;

        public Game()
        {
            WindowManager = new WindowManager();
            GameLoop = new GameLoop(WindowManager);
            Input.Window = WindowManager;
            Time.GameLoop = GameLoop;

            WindowManager.Scene = new Scene();
            GameObject.Scene = WindowManager.Scene;

            WindowManager.Scene.ActiveCamera = new Camera();

            WindowManager.Load += OnLoad;
            //WindowManager.Resize += OnResize;
            WindowManager.WindowFrameUpdated += OnFrameUpdate;


            GameLoop.PhysicsUpdated += OnPhysicsUpdate;
            WindowManager.Run();
        }


        public void OnLoad(object sender, EventArgs e)
        {
            var groundModel = GltfImporter.Load("./Resources/Models/test_1.gltf");
            var monkeyModel = GltfImporter.Load("./Resources/Models/monkey.gltf");
            foreach (var mesh in monkeyModel.Meshes)
            {
                mesh.Material = new BasicInstancedMaterial();
            }

            var groundMaterial = new BasicShadedMaterial()
            {
                Color = new Vector3(1, 1, 1)
            };

            foreach (var mesh in groundModel.Meshes)
            {
                mesh.Material = groundMaterial;
            }

            var ground = new GameObject();
            ground.AddComponent(new ModelComponent()
            {
                Model = groundModel
            });

            var groundShape = new ModelShape(groundModel);
            
            var groundPhysics = ground.AddComponent(new PhysicsComponent()
            {
                RigidBody = new RigidBody(groundShape.Shape)
                {
                    IsStatic = true,
                    Tag = Color.LightGray
                }
            });

            var camera = WindowManager.Scene.ActiveCamera;
            camera.AddComponent(new FreelookCameraComponent());
            camera.Transform.Position = new Vector3(154.0654f, 78.74099f, 185.0334f);
            camera.Transform.Rotation = new Vector3(224.9143f, -2.557277f, 0f);


            var monkey = new GameObject();
            monkey.AddComponent(new ModelComponent
            {
                Model = monkeyModel
            });
            monkey.AddComponent(new PhysicsComponent
            {
                RigidBody = new RigidBody(new BoxShape(10f, 10f, 10f))
                {
                }
            });
            monkey.AddComponent(new MonkeyComponent());
           
            monkey.Transform.Position = new Vector3(142.0654f, 110.74099f, 173.0334f);
        }

        public void OnFrameUpdate(float deltaTime)
        {
            WindowManager.Scene.OnUpdate();
            WindowManager.Scene.ActiveCamera.Render();
            WindowManager.Context.SwapBuffers();
        }

        public void OnPhysicsUpdate(object sender, PhysicsUpdateEventArgs e)
        {
            GameObject.Scene.OnFixedUpdate();
            WindowManager.Scene.PhysicsStep(e.TimeStep);
        }
    }
}