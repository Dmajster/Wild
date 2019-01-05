using System.Numerics;
using GameEngine;
using GameEngine.Components;
using OpenTK.Input;

namespace Game.Components
{
    public class MonkeyComponent : Component
    {
        public override void OnUpdate()
        {
            GameObject.Transform.Position += new Vector3(1,0,0) * Time.DeltaTime;
        }
    }
}
