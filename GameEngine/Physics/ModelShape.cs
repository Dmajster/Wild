using System;
using System.Collections.Generic;
using System.Numerics;
using GameEngine.Rendering.Models;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;

namespace GameEngine.Physics
{
    public class ModelShape
    {
        public readonly TriangleMeshShape Shape;

        public ModelShape(Model model)
        {
            var positionsByteArray = model.Meshes[0].Attributes["POSITION"].BufferData;
            var positionsFloatArray = new float[positionsByteArray.Length / 4];
            Buffer.BlockCopy(positionsByteArray, 0, positionsFloatArray, 0, positionsByteArray.Length);

            var positions = new List<JVector>();
            for (var i = 0; i < positionsFloatArray.Length / 3; i++)
            {
                positions.Add(new JVector(positionsFloatArray[i * 3], positionsFloatArray[i * 3 + 1], positionsFloatArray[i * 3 + 2]));
            }

            var indicesByteArray = model.Meshes[0].Attributes["INDEX"].BufferData;
            var indicesShortArray = new short[indicesByteArray.Length / 2];
            Buffer.BlockCopy(indicesByteArray, 0, indicesShortArray, 0, indicesByteArray.Length);

            var tris = new List<TriangleVertexIndices>();
            for (var i = 0; i < indicesShortArray.Length / 3; i++)
            {
                tris.Add(new TriangleVertexIndices(indicesShortArray[i * 3], indicesShortArray[i * 3 + 1], indicesShortArray[i * 3 + 2]));
            }

            var octree = new Octree(positions, tris);
            octree.BuildOctree();
            Shape = new TriangleMeshShape(octree);
            Shape.FlipNormals = false;
        }
    }
}
