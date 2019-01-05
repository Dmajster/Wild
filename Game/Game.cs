using GameEngine;
using GameEngine.Components;
using GameEngine.Gltf;
using System;
using System.Drawing;
using System.Numerics;
using Game.Components;
using GameEngine.Materials;
using GameEngine.Physics;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;

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
            var playerModel = GltfImporter.Load("./Resources/Models/postpose.gltf");
            var pineTreeModel = GltfImporter.Load("./Resources/Models/pine_tree_1.gltf");

            foreach (var mesh in playerModel.Meshes)
            {
                mesh.Material = new BasicShadedColoredMaterial();
            }

            foreach (var mesh in pineTreeModel.Meshes)
            {
                mesh.Material = new BasicShadedColoredMaterial();
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
            ground.AddComponent(new PhysicsComponent()
            {
                RigidBody = new RigidBody(groundShape.Shape)
                {
                    IsStatic = true,
                    AllowDeactivation = false
                }
            });

            var player = new GameObject();
            var playerShape = new ModelShape(playerModel);

            var pineTree = new GameObject();
            pineTree.AddComponent(new ModelComponent()
            {
                Model = pineTreeModel
            });
            pineTree.Transform.Position = new Vector3(80, 73, 100);
            pineTree.Transform.Scale = new Vector3(4,4,4);

            player.AddComponent(new ModelComponent
            {
                Model = playerModel
            });
            player.AddComponent(new PhysicsComponent
            {
                RigidBody = new RigidBody(new BoxShape(1f,0.5f,1))
                {
                    AffectedByGravity = true,
                    AllowDeactivation = false
                }
            });
            player.AddComponent(new MonkeyComponent());
           
            player.Transform.Position = new Vector3(145.0654f, 100.74099f, 173.0334f);
            player.Transform.Rotation = new Vector3(215.9143f, 0f, 0f);

            var camera = WindowManager.Scene.ActiveCamera;
            camera.AddComponent(new FollowCameraComponent()
            {
                FollowGameObject = player
            });

            /*
            camera.AddComponent(new FreelookCameraComponent()
            {
                FollowGameObject = player
            });

            camera.Transform.Position = new Vector3(154.0654f, 80.74099f, 185.0334f);
            camera.Transform.Rotation = new Vector3(224.9143f, -2.557277f, 0f);
            */
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