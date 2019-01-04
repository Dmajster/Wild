namespace GameEngine
{
    public static class Time
    {
        public static GameLoop GameLoop;

        public static float DeltaTime => GameLoop.DeltaTime;
    }
}
