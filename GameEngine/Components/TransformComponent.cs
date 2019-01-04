using System.Numerics;
using GameEngine.Extensions;

namespace GameEngine.Components
{
    public class TransformComponent : Component
    {
        private bool _changed = false;
        public bool Changed
        {
            get
            {
                if (!_changed)
                {
                    return false;
                }

                _changed = false;
                return true;
            }
            set => _changed = value;
        }

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
                
                Changed = true;
            }
        }

        private Vector3 _rotation = new Vector3(0, 0, 0);
        public Vector3 Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                Changed = true;
            }
        }

        private Vector3 _scale = new Vector3(1, 1, 1);
        public Vector3 Scale 
        {
            get => _scale;
            set
            {
                _scale = value;
                Changed = true;
            }
        }

        public Matrix4x4 ToMatrix4X4()
        {
            return
                Matrix4x4.CreateTranslation(Position) *
                Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) *
                Matrix4x4.CreateScale(Scale);
        }
    }
}
