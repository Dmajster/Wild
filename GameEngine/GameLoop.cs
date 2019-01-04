using System;
using System.Diagnostics;
using OpenTK;

namespace GameEngine
{
    public class GameLoop
    {
        public float SimulationTime { get; private set; }
        public float SimulationTimeStep { get; set; } = 0.03125f;
        public float DeltaTime { get; private set; }

        public delegate void UpdatedEventHandler();
        public event UpdatedEventHandler Updated;

        public event EventHandler<PhysicsUpdateEventArgs> PhysicsUpdated;

        private float _simulationTimeAccumulator;
        private float _newRenderTick;
        private float _lastRenderTick;

        private readonly Stopwatch _stopwatch;

        public GameLoop(WindowManager windowManager)
        {
            windowManager.UpdateFrame += OnUpdate;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _lastRenderTick = _stopwatch.ElapsedTicks;
        }

        private void OnUpdate(object sender, FrameEventArgs e)
        {
            _newRenderTick = _stopwatch.ElapsedTicks;
            DeltaTime = (_newRenderTick - _lastRenderTick) / Stopwatch.Frequency;
            _simulationTimeAccumulator += DeltaTime;
            _lastRenderTick = _newRenderTick;

            while (_simulationTimeAccumulator > SimulationTimeStep)
            {
                PhysicsUpdated?.Invoke(this, new PhysicsUpdateEventArgs()
                {
                    TimeStep = SimulationTimeStep
                });

                _simulationTimeAccumulator -= SimulationTimeStep;
                SimulationTime += SimulationTimeStep;
            }

            Updated?.Invoke();
        }
    }

    public class PhysicsUpdateEventArgs : EventArgs
    {
        public float TimeStep { get; set; }
    }
}
