using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace GameEngine
{
    public class Mouse
    {
        public Vector2 OldPosition = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Delta;
        public float Scroll = 0f;
        public float ScrollDelta = 0f;

        public Dictionary<MouseButton, bool> ButtonStatus = new Dictionary<MouseButton, bool>();
    }

    public static class Input
    {
        private static readonly Dictionary<Key, bool> KeyboardStatus = new Dictionary<Key, bool>();
        private static readonly Mouse Mouse = new Mouse();

        private static GameWindow _window;
        public static GameWindow Window
        {
            get => _window;
            set
            {
                _window = value;
                _window.KeyDown += KeyDown;
                _window.KeyUp += KeyUp;
                _window.MouseMove += MouseMove;
                _window.MouseDown += MouseDown;
                _window.MouseUp += MouseUp;
                _window.MouseWheel += MouseWheel;
                _window.UpdateFrame += MouseUpdateDelta;
            }
        }

        private static void MouseUpdateDelta(object sender, FrameEventArgs e)
        {
            Mouse.Delta = new Vector2(Mouse.Position.X-Mouse.OldPosition.X,Mouse.Position.Y-Mouse.OldPosition.Y);
            Mouse.OldPosition = Mouse.Position;
        }

        private static void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Mouse.Scroll = e.ValuePrecise;
            Mouse.ScrollDelta = e.DeltaPrecise;
        }

        private static void KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardStatus[e.Key] = true;
        }

        private static void KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            KeyboardStatus[e.Key] = false;
        }

        private static void MouseMove(object sender, MouseMoveEventArgs e)
        {
            Mouse.Position = new Vector2(e.X, e.Y);
            Mouse.Delta = new Vector2(e.XDelta,e.YDelta);
        }

        private static void MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.ButtonStatus[e.Button] = e.IsPressed;
        }

        private static void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.ButtonStatus[e.Button] = e.IsPressed;
        }

        public static float GetMouseScroll => Mouse.Scroll;
        public static float GetMouseScrollDelta => Mouse.ScrollDelta;
        public static Vector2 GetMousePosition => Mouse.Position;
        public static Vector2 GetMouseDelta => Mouse.Delta;

        public static bool GetMouseButton(MouseButton button)
        {
            return Mouse.ButtonStatus.ContainsKey(button) && Mouse.ButtonStatus[button];
        }

        public static bool GetKey(Key key)
        {
            return KeyboardStatus.ContainsKey(key) && KeyboardStatus[key];
        }
    }
}
