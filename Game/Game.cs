using ECS;
using ECS.Interfaces;
using Game.Components;
using GameEngine;
using OpenTK;
using System;

namespace Game
{
    public sealed class Game
    {
        public readonly WindowManager WindowManager;
        public readonly TickManager TickManager;

        public Game()
        {
            WindowManager = new WindowManager();
            WindowManager.Load += OnLoad;
            WindowManager.Resize += OnResize;
            WindowManager.UpdateFrame += OnFrameUpdate;

            TickManager = new TickManager(WindowManager);
            TickManager.PhysicsUpdated += OnPhysicsUpdate;
            
            ComponentManager componentManager = new ComponentManager();
            Entity entity = componentManager.AddEntity(new IComponent[]
            {
                new InputComponent(), 
            });

            var input = componentManager.GetComponent<InputComponent>(entity);

            input.Forward = true;

            componentManager.SetComponent(entity, input);

            componentManager.AddComponent(entity, new MeshComponent());


            WindowManager.Run();
        }

        public void OnResize(object sender, EventArgs e)
        {
        }

        public void OnLoad(object sender, EventArgs e)
        {
        }

        public void OnFrameUpdate(object sender, FrameEventArgs e)
        {
        }

        public void OnPhysicsUpdate(object sender, PhysicsUpdateEventArgs e)
        {
        }
    }
}
