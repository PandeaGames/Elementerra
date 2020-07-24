using System.Collections.Generic;
using PandeaGames;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraBlobRenderer
    {
        private Mesh mesh;
        private GameObject _renderingPlane;
        public void Render(TerraBlob blob, Transform parent, Material material)
        {
            TerraViewModel terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            MeshRenderer meshRenderer = null;
            MeshFilter meshFilter = null;
            if(_renderingPlane == null)
            {
                _renderingPlane = GameObject.Instantiate(new GameObject("TerraBlob"), parent);
                meshRenderer = _renderingPlane.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
                meshRenderer.castShadows = false;
                meshFilter = _renderingPlane.AddComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                mesh = meshFilter.mesh;
            }

            List<TerraVector> blobVertices = new List<TerraVector>(blob.Vertices);
            
            int verticiesLength = blobVertices.Count * 2;

            Vector3[] vertices = new Vector3[verticiesLength];
            Color[] colors = new Color[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];

            //for every point, there is 2 triangles, equaling 6 total vertices
            int[] triangles = new int[(blobVertices.Count - 1) * 6];

            for (int i = 0; i < blobVertices.Count; i++)
            {
                int bottomVerticeIndex = i * 2;
                int topVerticeIndex = bottomVerticeIndex + 1;
                Vector3 vertice = terraViewModel.Geometry[blobVertices[i]];
                vertices[bottomVerticeIndex] = vertice;
                vertices[topVerticeIndex] = new Vector3(vertice.x, vertice.y + 100, vertice.z);

                colors[bottomVerticeIndex] = new Color(1, 1, 1, 0.25f);
                colors[topVerticeIndex] = new Color(1, 1, 1, 0);
                
                int triIndex = i * 6;

                if (i < blobVertices.Count - 1)
                {
                    triangles[triIndex] = bottomVerticeIndex;
                    triangles[triIndex + 1] = (i + 1) * 2;
                    triangles[triIndex + 2] = topVerticeIndex;
                
                    triangles[triIndex + 3] = (i + 1) * 2;
                    triangles[triIndex + 4] = (i + 1) * 2 + 1;
                    triangles[triIndex + 5] = topVerticeIndex;
                }
            }
            
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.normals = normals;

            _renderingPlane.SetActive(true);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            _renderingPlane.SetActive(false);
            _renderingPlane.SetActive(true);
        }
    }
}