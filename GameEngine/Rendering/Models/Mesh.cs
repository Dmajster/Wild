using System.Collections.Generic;

namespace GameEngine.Rendering.Models
{
    public class Mesh
    {
        public GlMaterial Material;
        public Dictionary<string, MaterialAttribute> Attributes;
    }
}
