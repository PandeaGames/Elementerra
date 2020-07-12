
using System;
using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra;
using Terra.MonoViews;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

public class TerraTerrainSectionRenderer
{
    private TerraTerrainGeometryDataModel chunk;
    public TerraArea localArea;
    public TerraArea localAreaWithBevel;
    public TerraArea localRenderArea;
    private Transform parent;
    private TerraViewModel _terraViewModel;
    
    public TerraTerrainSectionRenderer(TerraTerrainGeometryDataModel chunk, TerraArea localArea, Transform parent)
    {
        _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
        this.chunk = chunk;
        this.localArea = localArea;
        this.localRenderArea = new TerraArea(localArea.x, localArea.y, localArea.width + 1, localArea.height + 1);
        this.localAreaWithBevel = new TerraArea(localRenderArea.x-1, localRenderArea.y-1, localRenderArea.width + 1, localRenderArea.height + 1);
        this.parent = parent;
    }
    private TerraTerrainGeometryDataModel _renderingChunk;
        private TerraChunksViewModel _vm;

        private void GeometryUpdate(TerraTerrainGeometryDataModel geom)
        {
            if (_renderingChunk != null)
            {
                //_renderingChunk.OnDataHasChanged -= OnDataHasChanged;
            }
            _renderingChunk = geom;
            //_renderingChunk.OnDataHasChanged += OnDataHasChanged;
            
            RenderGround();
        }

        private GameObject _renderingPlane;
        private Texture2D _soilQualityValueTexture;

        [SerializeField] private Vector3 _planeOffset;
        [SerializeField] private string _generatedGameObjectName;
        [SerializeField] private bool _debugView;
        
        [SerializeField]
        private float _scale = 1;
        public float Scale
        {
            get { return _scale; }
        }

        private Mesh mesh;
        private MeshFilter meshFilter = null;

        public void OnDataHasChanged(IEnumerable<TerraSoilQualityGridPoint> data)
        {
            bool hasChanges = false;
            foreach (TerraSoilQualityGridPoint dataPoint in data)
            {
                if (localAreaWithBevel.Contains(dataPoint.Vector))
                {
                    int x = dataPoint.Vector.x - localRenderArea.x;
                    int y = dataPoint.Vector.y - localRenderArea.y;
                    hasChanges = true;
                    _soilQualityValueTexture.SetPixel(x, y, dataPoint.Data);
                }  
            }

            if (hasChanges)
            {
                _soilQualityValueTexture.Apply();
            }
        }

        public void OnDataHasChanged(IEnumerable<TerraTerrainGeometryDataPoint> data)
        {
            Color[] colors = meshFilter.sharedMesh.colors;
            Vector3[] vertices = meshFilter.sharedMesh.vertices;

            bool hasChanges = false;
            foreach (TerraTerrainGeometryDataPoint dataPoint in data)
            {
                if (localAreaWithBevel.Contains(dataPoint.Vector))
                {
                    hasChanges = true;
                    UpdateData(dataPoint, colors, vertices);
                }  
            }

            if (hasChanges)
            {
                meshFilter.sharedMesh.colors = colors;
                meshFilter.sharedMesh.vertices = vertices;
                meshFilter.sharedMesh.RecalculateNormals();
                meshFilter.sharedMesh.RecalculateBounds();
                meshFilter.sharedMesh.RecalculateTangents();
                MeshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }
        
        private void UpdateData(TerraTerrainGeometryDataPoint vector, Color[] colors, Vector3[] vertices)
        {
            int vertPosition = ((vector.Vector.x - localRenderArea.x) * localRenderArea.width) + (vector.Vector.y - localRenderArea.y);
            vertices[vertPosition] = vector.Data;
        }

        private MeshCollider MeshCollider;
        public void RenderGround()
        {
            if(_renderingPlane == null)
            {
                _renderingPlane = GameObject.Instantiate(new GameObject(string.IsNullOrEmpty(_generatedGameObjectName) ? "Terra Terrain":_generatedGameObjectName), this.parent);
                _renderingPlane.transform.position = _planeOffset;
                _renderingPlane.layer =LayerMask.NameToLayer(TerraGameResources.Instance.LayerForTerrain);
                _renderingPlane.AddComponent<MeshRenderer>();
                _renderingPlane.AddComponent<MeshFilter>();
                _renderingPlane.AddComponent<TerrainSectionMonoView>().SetData(chunk,localRenderArea, localArea);
                MeshCollider = _renderingPlane.AddComponent<MeshCollider>();
                meshFilter = _renderingPlane.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                Rigidbody rb = _renderingPlane.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            
            

            GameObject plane = _renderingPlane;
            Renderer renderer = plane.GetComponent<Renderer>();
            plane.SetActive(false);
            renderer.material = TerraGameResources.Instance.TerrainMaterial;

            _soilQualityValueTexture = new Texture2D(localRenderArea.width, localRenderArea.height);
            _soilQualityValueTexture.name = "soilQuality";
            
          
            meshFilter = plane.GetComponent<MeshFilter>();
            mesh = meshFilter.sharedMesh;
            MeshCollider.sharedMesh = meshFilter.sharedMesh;
            int verticiesLength = (localArea.width + 1) * (localArea.height + 1);

            Vector3[] vertices = new Vector3[verticiesLength];
            Color[] colors = new Color[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];
            float[] grass = new float[vertices.Length];
            // Vector2[] triangles = new Vector2[(int)(dimensions.Area * 2)];

            //for every point, there is 2 triangles, equaling 6 total vertices
            int[] triangles = new int[(localArea.width * localArea.height) * 6];

            //Create Vertices
            for (int x = 0; x < localRenderArea.width; x++)
            {
                for (int y = 0; y < localRenderArea.height; y++)
                {
                    int localX = x + localRenderArea.x;
                    int localY = y + localRenderArea.y;
                    Color color = GetColor(x, y);
                    float soilQualityValue = 0;
                    int position = (x * localRenderArea.width) + y;
                    Color soilQUalityColor = default(Color);
                    if (localX >= chunk.Width || localY >= chunk.Height)
                    {
                        vertices[position] = chunk[chunk.Width - 1, chunk.Height - 1];
                        soilQUalityColor = new Color(0, 0, 0);
                    }
                    else
                    {
                        vertices[position] = chunk[localX, localY];
                        //soilQualityValue = _terraViewModel.GrassPotential[Math.Min(localX+1, chunk.Width-1), localY];
                        soilQUalityColor = _terraViewModel.TerraSoilQualityViewModel[localX, localY];
                    }
                    
                    /*_soilQualityValueTexture.SetPixel(x, y, new Color(
                        soilQualityValue, 
                        soilQualityValue,
                        soilQualityValue));*/
                    _soilQualityValueTexture.SetPixel(x, y, soilQUalityColor);
                    
                    colors[position] = color;
                    uvs[position] = new Vector2(x / (float)localRenderArea.width, y / (float)localRenderArea.height);
                    normals[position] = Vector3.up;
                    grass[position] = 0.5f;
                }
            }
            
            _soilQualityValueTexture.Apply();
            renderer.material.SetTexture("Texture2D_5426D726", _soilQualityValueTexture);

            List<Vector3> vectorTriangles = new List<Vector3>();

            //Create Triangles
            for (int x = 0; x < localArea.width; x++)
            {
                for (int y = 0; y < localArea.height; y++)
                {
                    SetTriangles(chunk, localArea, x, y, triangles, vectorTriangles);
                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.normals = normals;

            plane.SetActive(true);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            plane.SetActive(false);
            plane.SetActive(true);
        }

        private Color GetColor(int x, int y)
        {
            return Color.cyan;
        }

        private void SetTriangles(TerraTerrainGeometryDataModel chunk, TerraArea area, int x, int y, int[] triangles, List<Vector3> vectorTriangles)
        {
            //we are making 2 triangles per loop. so offset goes up by 6 each time
            int triangleOffset = (x * area.height + y) * 6;
            int verticeX = area.width + 1;
            int verticeY = area.height + 1;
                    
            //triangle 1
            triangles[triangleOffset] = x * verticeY + y;
            triangles[1 + triangleOffset] = x * verticeY + y + 1;
            triangles[2 + triangleOffset] = x * verticeY + y + verticeY;

            vectorTriangles.Add(new Vector3(triangles[triangleOffset], triangles[1 + triangleOffset], triangles[2 + triangleOffset]));

            //triangle 2
            triangles[3 + triangleOffset] = x * verticeY + y + verticeY;
            triangles[4 + triangleOffset] = x * verticeY + y + 1;
            triangles[5 + triangleOffset] = x * verticeY + y + verticeY + 1;

            vectorTriangles.Add(new Vector3(triangles[3 + triangleOffset], triangles[4 + triangleOffset], triangles[5 + triangleOffset]));
        }

        public void OnDrawGizmos()
        {
            if (_debugView)
            {
                MeshFilter MeshFilter = _renderingPlane.GetComponent<MeshFilter>();
                for (int i = 0; i < MeshFilter.sharedMesh.vertexCount; i++)
                {
                    Gizmos.DrawLine(MeshFilter.sharedMesh.vertices[i], MeshFilter.sharedMesh.vertices[i] + MeshFilter.sharedMesh.normals[i] * 1);
                }
            }
            
        }
#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            for (int x = 0; x < localRenderArea.width; x++)
            {
                for (int y = 0; y < localRenderArea.height; y++)
                {
                    
                    string text = $"({x}:{y})";
                    DebugUtils.DrawString(text,new Vector3(localArea.x+x, 0, localArea.y+y), Color.white, 8);
                }
            }
        }
#endif

        /* private void UpdateSurroundingHights(TerraVector vector, Vector3[] vertices, TerraWorldChunk chunk)
        {
            UpdateHights(vector, vertices, chunk);
            
            UpdateHights(new TerraVector(){x = vector.x - 1, y = vector.y - 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x, y = vector.y - 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x + 1, y = vector.y - 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x + 1, y = vector.y}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x + 1, y = vector.y + 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x, y = vector.y + 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x - 1, y = vector.y + 1}, vertices, chunk);
            UpdateHights(new TerraVector(){x = vector.x - 1, y = vector.y}, vertices, chunk);
        }*/
        
       /* private void UpdateHights(TerraVector vector, Vector3[] vertices,  TerraWorldChunk chunk)
        {
            int vertPosition = (vector.x * (chunk.Width + 1)) + vector.y;
            vertices[vertPosition] = GetVertice(_currentToken, vector.x, vector.y);
        }*/
        
       /* private Vector3 GetVertice(TerraWorldChunk chunk, int x, int y)
        {
            return new Vector3(
                (chunk..left + x), 
                ((chunk[x, y].Height) +
                 chunk[x - 1, y -1].Height +
                chunk[x, y -1].Height +
                   chunk[x + 1, y -1].Height +
                chunk[x + 1, y].Height +
                chunk[x + 1, y + 1].Height +
                chunk[x, y + 1].Height +
                chunk[x - 1, y + 1].Height +
                      chunk[x - 1, y].Height) / 9f) * 0.5f,
                (token.Request.top + y));
        }*/
       
}
