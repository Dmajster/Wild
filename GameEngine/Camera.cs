using System;
using GameEngine.Components;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using Jitter;
using Jitter.LinearMath;

namespace GameEngine
{
    public class Camera : GameObject
    {
        
        public CameraComponent CameraComponent;
        
        public Camera()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.ClearColor(0.64f, 0.83f, 1, 1.0f);

            CameraComponent = AddComponent(new CameraComponent());
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            var gameObjects = Scene.FindGameObjectsWithComponent<ModelComponent>();

            foreach (var gameObject in gameObjects)
            {
                var modelComponent = gameObject.GetComponent<ModelComponent>();
                var model = modelComponent.Model;

                foreach (var mesh in model.Meshes)
                {
                    mesh.Material.Render(mesh, Scene.ActiveCamera, new[]
                    {
                        gameObject.GetComponent<TransformComponent>().ToMatrix4X4()
                    });
                }
            }

            gameObjects = Scene.FindGameObjectsWithComponent<PhysicsComponent>();

            foreach (var gameObject in gameObjects)
            {
                var physics = gameObject.GetComponent<PhysicsComponent>();
                //physics.RigidBody.EnableDebugDraw = true;
                //physics.RigidBody.DebugDraw(new DebugDrawer());
            }
        }
    }
}
