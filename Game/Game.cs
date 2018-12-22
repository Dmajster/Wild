using System;
using GameEngine;
using OpenTK;

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
