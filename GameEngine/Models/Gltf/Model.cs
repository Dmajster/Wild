using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using glTFLoader;
using glTFLoader.Schema;

namespace GameEngine.Models.Gltf
{
    public class Model
    {
        public string Path;
        public glTFLoader.Schema.Mesh[] Meshes;
        public glTFLoader.Schema.Accessor[] Accessors;
        public glTFLoader.Schema.BufferView[] BufferViews;
        public glTFLoader.Schema.Buffer[] Buffers;

        public static Model Load(string path)
        {
            var gltf = Interface.LoadModel(path);

            return new Model()
            {
                Path = path,
                Meshes = gltf.Meshes,
                Accessors = gltf.Accessors,
                BufferViews = gltf.BufferViews,
                Buffers = gltf.Buffers
            };
        }

        public Mesh CreateMesh() => CreateMesh(Meshes[0]);
        public Mesh CreateMesh(glTFLoader.Schema.Mesh mesh)
        {
            var meshData = new Dictionary<string, byte[]>();

            foreach (var meshPrimitive in mesh.Primitives)
            {
                //TODO add support for drawing without requiring Indexing buffer
                if (!meshPrimitive.Indices.HasValue)
                    continue;

                meshData.Add("INDEX", GetDataFromAccessor(Accessors[meshPrimitive.Indices.Value]));

                foreach (var meshPrimitiveAttribute in meshPrimitive.Attributes)
                {
                    meshData.Add(meshPrimitiveAttribute.Key, GetDataFromAccessor(Accessors[meshPrimitiveAttribute.Value])); 
                }
            }

            return new Mesh()
            {
                Attributes = meshData
            };
        }

        public byte[] GetDataFromAccessor(Accessor accessor)
        {
            if (!accessor.BufferView.HasValue)
                return null;
            
            //Get buffer view object
            var bufferView = BufferViews[accessor.BufferView.Value];
    
            //Get buffer object
            var buffer = Buffers[bufferView.Buffer];

            //Load buffer data from disk
            //TODO streaming support
            var bufferDirectory = new DirectoryInfo(System.IO.Path.GetDirectoryName(Path)) + @"/" + buffer.Uri;
            var bufferData = File.ReadAllBytes(bufferDirectory);

            //Slice pre byte offset data away
            var bufferViewData = bufferData.Skip(bufferView.ByteOffset).Take(bufferView.ByteLength).ToArray();

            //Check if it's ok to return or if there's still the stride we need to take care of
            if (!bufferView.ByteStride.HasValue)
                return bufferViewData;

            //Slice away data by jumping with byte stride and taking next N values where N is the accessor component type (byte, float...) multiplied by accessor type itself (VEC2, MAT4...)
            return bufferViewData.Where((n, index) => index % bufferView.ByteStride < GetComponentSize(accessor.ComponentType) * GetTypeSize(accessor.Type)).ToArray();
        }


        private int GetComponentSize(Accessor.ComponentTypeEnum componentType)
        {
            switch (componentType)
            {
                case Accessor.ComponentTypeEnum.BYTE:
                    return 1;
                case Accessor.ComponentTypeEnum.UNSIGNED_BYTE:
                    return 1;
                case Accessor.ComponentTypeEnum.SHORT:
                    return 2;
                case Accessor.ComponentTypeEnum.UNSIGNED_SHORT:
                    return 2;
                case Accessor.ComponentTypeEnum.UNSIGNED_INT:
                    return 4;
                case Accessor.ComponentTypeEnum.FLOAT:
                    return 4;
            }

            throw new InvalidEnumArgumentException("Unknown Component type!");
        }

        private int GetTypeSize(Accessor.TypeEnum type)
        {
            switch (type)
            {
                case Accessor.TypeEnum.SCALAR:
                    return 1;
                case Accessor.TypeEnum.VEC2:
                    return 2;
                case Accessor.TypeEnum.VEC3:
                    return 3;
                case Accessor.TypeEnum.VEC4:
                    return 4;
                case Accessor.TypeEnum.MAT2:
                    return 4;
                case Accessor.TypeEnum.MAT3:
                    return 9;
                case Accessor.TypeEnum.MAT4:
                    return 16;
            }

            throw new InvalidEnumArgumentException("Unknown type!");
        }
    }

    public class Mesh
    {
        public Dictionary<string, byte[]> Attributes;
    }
}
