using System;
using System.Collections.Generic;
using System.Linq;
using PandeaGames;
using Terra.Utils;
using Terra.ViewModels;
using Terra.Views;
using UnityEngine;

namespace Terra.Rendering
{
    public class TerraTerrainBlobRenderer
    {
        public class VirtualVertice
        {
            public Vector3 Vector { get; set; }
            public TerraVector TerraVector { get; set; }
            public List<int> Connections { get; } = new List<int>();
            public bool IsReverseSide { get; set; }
        }

        private struct Triangle
        {
            private int[] sortedArray;

            public int A;
            public int B;
            public int C;

            public override int GetHashCode()
            {
                int[] sortedArray = {A, B, C};
                Array.Sort(sortedArray);
                return (sortedArray[0] * sortedArray[0]) * 
                       (sortedArray[1] * sortedArray[1] * sortedArray[1]) *
                       (sortedArray[2] * sortedArray[2] * sortedArray[2] * sortedArray[2]);
                
                
            }
        }

        private TerraBlob _blob;
        private Transform _parent;
        private TerraViewModel _terraViewModel;
        public Mesh Mesh { private set; get; }

        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        
        public TerraTerrainBlobRenderer(TerraBlob blob, Transform parent)
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _blob = blob;
            _parent = parent;
        }

        private GameObject _renderingPlane;
        private MeshCollider MeshCollider;
        private MeshFilter meshFilter;

        public void Render()
        {
            if(_renderingPlane == null)
            {
                _renderingPlane = GameObject.Instantiate(new GameObject("Terra Blob Platform"), _parent);
                _renderingPlane.AddComponent<MeshRenderer>();
                _renderingPlane.AddComponent<MeshFilter>();
                MeshCollider = _renderingPlane.AddComponent<MeshCollider>();
                meshFilter = _renderingPlane.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                Rigidbody rb = _renderingPlane.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            
            List<VirtualVertice> virtualVertices = GetVirtualVertices(_blob);
            HashSet<Triangle> triangles = new HashSet<Triangle>();

            for (int i = 0; i < virtualVertices.Count; i++)
            {
                VirtualVertice virtualVertice = virtualVertices[i];
                Vertices.Add(virtualVertice.Vector);
                foreach (int connection in virtualVertice.Connections)
                {
                    foreach (int sharedConnectionCandidate in virtualVertices[connection].Connections)
                    {
                        //both vertices are connected to the same vertices. possible triangle
                        if (virtualVertice.Connections.Contains(sharedConnectionCandidate))
                        {
                            Triangle triangle = new Triangle()
                            {
                                A = i, B = connection, C = sharedConnectionCandidate
                            };

                            bool doesTriangleAlreadyExist = triangles.Contains(triangle);

                            if (!doesTriangleAlreadyExist)
                            {
                                triangles.Add(triangle);
                            }
                        }
                    }
                }
            }
            
            List<int> meshTriangles = new List<int>();

            foreach (Triangle triangle in triangles)
            {
                meshTriangles.Add(triangle.A);
                meshTriangles.Add(triangle.B);
                meshTriangles.Add(triangle.C);
            }
            
            Mesh = meshFilter.sharedMesh;
            MeshCollider.sharedMesh = meshFilter.sharedMesh;

            Mesh.vertices = Vertices.ToArray();
            Mesh.triangles = meshTriangles.ToArray();
            Mesh.RecalculateNormals();
            Mesh.RecalculateBounds();
            Mesh.RecalculateTangents();
        }

        public List<VirtualVertice> GetVirtualVertices(TerraBlob blob)
        {
            HashSet<TerraVector> edges = new HashSet<TerraVector>();
            List<TerraVector> vectors = new List<TerraVector>(blob.MeshVertices);
            int numberOfVectors = vectors.Count;

            foreach (TerraVector edgeVector in blob.Vertices)
            {
                edges.Add(edgeVector);
            }
            
            VirtualVertice[] virtualVertices = new VirtualVertice[numberOfVectors * 2];
            
            for (int i = 0; i < numberOfVectors; i++)
            {
                TerraVector localVector = vectors[i];
                Vector3 position = _terraViewModel.Geometry[localVector];
                virtualVertices[i] = new VirtualVertice()
                {
                    TerraVector = vectors[i],
                    IsReverseSide = false,
                    Vector = position
                };
                
                virtualVertices[i + numberOfVectors] = new VirtualVertice()
                {
                    TerraVector = vectors[i],
                    IsReverseSide = true,
                    Vector = new Vector3(position.x, position.y - 3, position.z)
                };
            }

            foreach (VirtualVertice virtualVertice in virtualVertices)
            {
                //find connections
                for (int i = 0; i < virtualVertices.Length; i++)
                {
                    VirtualVertice connectedVirtualVerticeCandidate = virtualVertices[i];
                    
                    if (virtualVertice == connectedVirtualVerticeCandidate)
                    {
                        continue;
                    }

                    TerraVector delta = (virtualVertice.TerraVector - connectedVirtualVerticeCandidate.TerraVector).Abs();

                    bool areBothEdges = edges.Contains(virtualVertice.TerraVector) &&
                                        edges.Contains(connectedVirtualVerticeCandidate.TerraVector);
                    bool isConnected = delta.x <= 1 && delta.y <= 1;
                    isConnected &= virtualVertice.IsReverseSide == connectedVirtualVerticeCandidate.IsReverseSide || areBothEdges;

                    if (isConnected)
                    {
                        virtualVertice.Connections.Add(i);
                    }
                }
            }

            return new List<VirtualVertice>(virtualVertices);
        }
    }
}