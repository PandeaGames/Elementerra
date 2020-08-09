using System.Collections.Generic;
using Terra.Utils;
using UnityEngine;

namespace Terra.Rendering
{
    public class TerraTerrainBlobRenderer
    {
        
        private TerraBlob _blob;
        private Transform _parent;
        public Mesh Mesh { private set; get; }

        public List<TerraVector> Vertices { get; private set; } = new List<TerraVector>();
        
        public TerraTerrainBlobRenderer(TerraBlob blob, Transform parent)
        {
            _blob = blob;
            _parent = parent;
        }

        public void Render()
        {
            Vertices.AddRange(_blob.MeshVertices);

            Mesh = new Mesh();
            
            
        }
    }
}