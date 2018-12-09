namespace GameEngine.Mesh.Primitives
{
    public class Cube
    {
        public static readonly float[] VERTICES = {
            // X, Y, Z, W
            0f, 0f, 0f,
            0f, 0f, 1f,
            0f, 1f, 0f,
            0f, 1f, 1f,
            1f, 0f, 0f,
            1f, 0f, 1f,
            1f, 1f, 0f,
            1f, 1f, 1f,
        };

        public static readonly int[] INDICES =
        {
            2, 6, 0, 6, 4, 0,   //Front
            2, 0, 1, 2, 3, 1,   //Left
            4, 6, 5, 6, 7, 5,   //Right
            3, 7, 1, 7, 5, 1,   //Back
            0, 4, 1, 4, 5, 1,   //Bottom
            3, 6, 2, 3, 7, 6    //Top
        };

        public float[] Vertices { get; set; } = VERTICES;
        public int[] Indices { get; set; } = INDICES;
    }
}
