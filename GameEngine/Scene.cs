using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameEngine.Components;
using GameEngine.Extensions;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace GameEngine
{
    public sealed class Scene
    {
        public Scene()
        {
            _collisionSystem = new CollisionSystemBrute();
            _collisionSystem.CollisionDetected += OnCollisionDetected;
            _collisionSystem.Detect(true);
            _world = new World(_collisionSystem);
        }

        private void OnCollisionDetected(
            RigidBody body1, RigidBody body2,
            JVector point1, JVector point2,
            JVector normal,
            float penetration)
        {

            var gameObjects = FindGameObjectsWithComponent<PhysicsComponent>();
            GameObject retractor; 

            if (body1.IsStatic)
            {
                retractor = gameObjects.First(gameObject => gameObject.GetComponent<PhysicsComponent>().RigidBody == body2);
                body2.LinearVelocity = JVector.Zero;
            }
            else
            {
                retractor = gameObjects.First(gameObject => gameObject.GetComponent<PhysicsComponent>().RigidBody == body1);
                body1.LinearVelocity = JVector.Zero;
            }


            //retractor.GetComponent<PhysicsComponent>().RigidBody.AddForce(normal*-penetration);
            //first.GetComponent<PhysicsComponent>().RigidBody.LinearVelocity = JVector.Zero;
            //retractor.GetComponent<PhysicsComponent>().RigidBody.Force = JVector.Zero;
            retractor.Transform.Position += Vector3.Normalize(normal.Cast()) * penetration;
            //Console.WriteLine($"Collision detected! {body2.Position} {body2.Force}");
        }

        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly World _world;
        public Camera ActiveCamera;
        private readonly CollisionSystem _collisionSystem;

        public GameObject[] FindGameObjectsWithComponent<T>() where T : Component
        {
            return _gameObjects.FindAll(gameObject => gameObject.GetComponent<T>() != null).ToArray();
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (_gameObjects.Find(checkedGameObject => checkedGameObject == gameObject) != null )
            {
                throw new Exception("GameObject already added to scene!");
            }
            _gameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            if (_gameObjects.Find(checkedGameObject => checkedGameObject == gameObject) == null)
            {
                throw new Exception("GameObject already removed from scene!");
            }

            _gameObjects.Remove(gameObject);
        }

        public void AddRigidBody(ref RigidBody rigidBody)
        {
            _world.AddBody(rigidBody);
        }

        public void OnUpdate()
        {
            _gameObjects.ForEach(gameObject => gameObject.ENGINE_OnUpdate());
        }

        public void OnFixedUpdate()
        {
            _gameObjects.ForEach(gameObject => gameObject.ENGINE_OnFixedUpdate());
        }

        public void PhysicsStep(float deltaTime)
        {
            _world.ContactSettings.BiasFactor = 1f;
            _world.Step(deltaTime,false);
        }
    }
}
