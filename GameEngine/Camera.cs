using GameEngine.Components;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace GameEngine
{
    public class Camera : GameObject
    {
        public readonly CameraComponent CameraComponent;

        public Camera()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.ClearColor(0.64f, 0.83f, 1, 1.0f);

            CameraComponent = AddComponent(new CameraComponent()
            {
                FieldOfView = 60,
                AspectRatio = 3 / 4f,
                Near = 0.1f,
                Far = 100f
            });
        }


        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            var projection = Matrix4x4.CreatePerspectiveFieldOfView(
                CameraComponent.FieldOfView * (float)System.Math.PI / 180f,
                CameraComponent.AspectRatio,
                CameraComponent.Near,
                CameraComponent.Far
            );

            CameraComponent.Projection = Transform.ToMatrix4X4() * projection;


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
        }
    }
}
