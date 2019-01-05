using System;
using GameEngine;
using OpenTK.Input;
using System.Numerics;
using GameEngine.Components;

namespace Game.Components
{
    public class FollowCameraComponent : Component
    {
        public CameraComponent CameraComponent;
        public GameObject FollowGameObject;

        public override void OnLoad()
        {
            CameraComponent = GetComponent<CameraComponent>();
        }

        public override void OnUpdate()
        {
            GameObject.Transform.Position = FollowGameObject.Transform.Position +
                                            FollowGameObject.Transform.Forward * -7 + Vector3.UnitY * 2;
                                            
            CameraComponent.CameraFront = FollowGameObject.Transform.Forward;

           

            if (Input.GetKey(Key.A))
            {
                FollowGameObject.Transform.Rotation -= new Vector3(100 * Time.DeltaTime, 0, 0);


                Console.WriteLine($"FTR: {FollowGameObject.Transform.Position} FF{FollowGameObject.Transform.Forward}");

            }

            if (Input.GetKey(Key.D))
            {
                FollowGameObject.Transform.Rotation += new Vector3(100 * Time.DeltaTime, 0, 0);
            }

            if (Input.GetKey(Key.W))
            {
                FollowGameObject.Transform.Position += FollowGameObject.Transform.Forward * 10 * Time.DeltaTime;
            }

            
        }
    }
}
