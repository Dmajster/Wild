using GameEngine;
using OpenTK.Input;
using System.Numerics;

namespace Game.Components
{
    public class FreelookCameraComponent : Component
    {
        public override void OnUpdate()
        {
            if (Input.GetMouseButton(MouseButton.Left))
            {
                GameObject.Transform.Position += new Vector3(0.5f, 0, 0) * Time.DeltaTime;
            }

            //GameObject.Transform.Rotation += new Vector3(Input.GetMouseDelta.X*Time.DeltaTime,0,0);
            GameObject.Transform.Rotation += new Vector3(Input.GetMouseDelta.X * Time.DeltaTime, 0, 0);
            GameObject.Transform.Rotation += new Vector3(0, 0, Input.GetMouseDelta.Y * Time.DeltaTime);

            if (Input.GetKey(Key.W))
            {
                GameObject.Transform.Position += new Vector3(0.5f, 0, 0) * Time.DeltaTime;
            }

            if (Input.GetKey(Key.A))
            {
                GameObject.Transform.Position += new Vector3(0, 0, 0.5f) * Time.DeltaTime;
            }
        }
    }
}
