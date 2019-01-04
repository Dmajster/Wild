using GameEngine;
using GameEngine.Components;
using GameEngine.Gltf;
using GameEngine.Rendering.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using Game.Components;
using GameEngine.Materials;
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
            var groundModel = GltfImporter.Load("./Resources/Models/box.gltf");
            var monkeyModel = GltfImporter.Load("./Resources/Models/monkey.gltf");
            foreach (var mesh in monkeyModel.Meshes)
            {
                mesh.Material = new BasicInstancedMaterial();
            }

            var groundMaterial = new BasicColoredMaterial
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
            ground.Transform.Scale = new Vector3(100,1,100);

            ground.AddComponent(new PhysicsComponent()
            {
                RigidBody = new RigidBody(new BoxShape(100, 1, 100))
                {
                    AffectedByGravity = false
                }
            });


            var camera = WindowManager.Scene.ActiveCamera;
            camera.AddComponent(new FreelookCameraComponent());
            camera.Transform.Position = new Vector3(5,-3,0);

            var monkey = new GameObject();
            monkey.AddComponent(new ModelComponent
            {
                Model = monkeyModel
            });
            monkey.AddComponent(new PhysicsComponent
            {
                RigidBody = new RigidBody(new BoxShape(3,3,3))
            });
            monkey.AddComponent(new MonkeyComponent());
            monkey.Transform.Position = new Vector3(0, 3, 0);

            var bla2 = new TriangleVertexIndices();

            var positions = new List<JVector>();
            var tris = new List<TriangleVertexIndices>();
            var octree = new Octree(positions, tris);
            var bla = new TriangleMeshShape(octree);
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
