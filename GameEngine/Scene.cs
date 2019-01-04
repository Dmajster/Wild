using System;
using System.Collections.Generic;
using Jitter;
using Jitter.Collision;
using Jitter.Dynamics;

namespace GameEngine
{
    public sealed class Scene
    {
        private readonly List<GameObject> _gameObjects = new List<GameObject>();
        private readonly World _world = new World(new CollisionSystemSAP());
        public Camera ActiveCamera;

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

        public void AddRigidBody(RigidBody rigidBody)
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
            _world.Step(deltaTime,true);
        }
    }
}
