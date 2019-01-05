using System;
using GameEngine;
using OpenTK.Input;
using System.Numerics;
using GameEngine.Components;

namespace Game.Components
{
    public class FreelookCameraComponent : Component
    {
        public CameraComponent CameraComponent;
        public GameObject FollowGameObject;

        public override void OnLoad()
        {
            CameraComponent = GetComponent<CameraComponent>();
        }

        public override void OnUpdate()
        {
            if (Input.GetMouseButton(MouseButton.Left))
            {
                GameObject.Transform.Position += GameObject.GetComponent<CameraComponent>().CameraFront * 100 * Time.DeltaTime;
            }

            if (Input.GetMouseButton(MouseButton.Right))
            {
                GameObject.Transform.Position -= GameObject.GetComponent<CameraComponent>().CameraFront * 100 * Time.DeltaTime;
            }

            if (Input.GetKey(Key.Q))
            {
                GameObject.Transform.Rotation -= new Vector3(100 * Time.DeltaTime, 0, 0);
            }

            if (Input.GetKey(Key.E))
            {
                GameObject.Transform.Rotation += new Vector3(100 * Time.DeltaTime, 0, 0);
            }
        

            if (Input.GetKey(Key.W))
            {
                GameObject.Transform.Rotation += new Vector3(0, 100 * Time.DeltaTime, 0);
            }

            if (Input.GetKey(Key.S))
            {
                GameObject.Transform.Rotation -= new Vector3(0, 100 * Time.DeltaTime, 0);
            }

            if (Input.GetKey(Key.J))
            {
                FollowGameObject.Transform.Rotation += new Vector3(100 * Time.DeltaTime, 0, 0);
            }

            if (Input.GetKey(Key.K))
            {
                FollowGameObject.Transform.Position += FollowGameObject.Transform.Forward * Time.DeltaTime * 4;
            }

            if (Input.GetKey(Key.D))
            {

                GameObject.Transform.Position += Vector3.Normalize(Vector3.Cross(CameraComponent.CameraFront, Vector3.UnitY)) * Time.DeltaTime;
            }

            if (Input.GetKey(Key.A))
            {
                GameObject.Transform.Position -= Vector3.Normalize(Vector3.Cross(CameraComponent.CameraFront, Vector3.UnitY)) * Time.DeltaTime;
            }

            CameraComponent.CameraFront = CameraComponent.CalculateFrontVector();
        }
    }
}
