using OpenTK;

namespace GameEngine
{
    public class WindowManager : GameWindow
    {
        public static Scene Scene;

        public WindowManager()
        {
            RenderFrame += ENGINE_OnRenderFrame;
        }

        private void ENGINE_OnRenderFrame(object sender, FrameEventArgs e)
        {
            WindowFrameUpdated?.Invoke((float)e.Time);
        }

        public delegate void WindowFrameUpdatedEventHandler(float deltaTime);
        public event WindowFrameUpdatedEventHandler WindowFrameUpdated;
    }
}
