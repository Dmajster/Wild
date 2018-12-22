using System.Collections.Generic;
using System.Numerics;
using GameEngine.Rendering.Models;

namespace GameEngine.Physics
{
    public class ModelShape
    {
        public ModelShape(Model model)
        {
            var positions = new List<Vector3>();
            var indices = new List<int>();

            foreach (var mesh in model.Meshes)
            {
                if (mesh.Attributes.ContainsKey("POSITION"))
                {
                    //positions.AddRange(mesh.Attributes["POSITION"].BufferData);
                }

                if (mesh.Attributes.ContainsKey("INDEX"))
                {
                    //indices.AddRange(mesh.Attributes["INDEX"].BufferData);
                }
            }
        }
    }
}
