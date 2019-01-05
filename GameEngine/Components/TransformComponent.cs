using System;
using System.Numerics;
using GameEngine.Extensions;

namespace GameEngine.Components
{
    public class TransformComponent : Component
    { 
        private Vector3 _position = new Vector3(0, 0, 0);
        public Vector3 Position
        {
            get
            {
                var physics = GetComponent<PhysicsComponent>();
                if (physics != null)
                {
                    _position = GetComponent<PhysicsComponent>().RigidBody.Position.Cast();
                }
                return _position;
            }
            set
            {
                _position = value;

                var physics = GetComponent<PhysicsComponent>();
                if (physics != null)
                {
                    physics.RigidBody.Position = _position.CastPhysics();
                }
                
            }
        }

        public Vector3 Rotation { get; set; } = new Vector3(0, 0, 0);
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public static float DegToRad(float degrees)
        {
            return degrees * 3.14f / 180;
        }

        public Vector3 Forward => Vector3.Normalize(new Vector3()
        {
            X = (float) Math.Cos(DegToRad(Rotation.X)) * (float) Math.Cos(DegToRad(Rotation.Y)),
            Y = (float) Math.Sin(DegToRad(Rotation.Y)),
            Z = (float) Math.Sin(DegToRad(Rotation.X)) * (float) Math.Cos(DegToRad(Rotation.Y))
        });

        public Matrix4x4 ToMatrix4X4()
        {
            return
                Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(DegToRad(Rotation.X), DegToRad(Rotation.Y), DegToRad(Rotation.Z))) *
                Matrix4x4.CreateScale(Scale) *
                Matrix4x4.CreateTranslation(Position);
        }
    }
}
