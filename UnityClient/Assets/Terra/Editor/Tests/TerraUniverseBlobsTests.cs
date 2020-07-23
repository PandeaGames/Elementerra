using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

namespace Tests
{
    public class TerraUniverseBlobsTests
    {
        private bool[,] _data;

        [SetUp]
        public void Setup()
        {
            bool[,] data = new bool[,]
            {
                {false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false},
                {false, true, true, true, false, false, false},
                {false, true, true, true, false, false, false},
                {false, true, true, true, false, false, false},
                {false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false},
                {false, false, false, false, false, true, true},
                {false, false, false, true, true, true, true},
                {false, false, false, true, true, true, true},
                {false, false, false, false, true, true, true},
                {false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false},
                {false, false, false, false, false, false, false},
            };

            _data = new bool[data.GetLength(1), data.GetLength(0)];

            for (int x = 0; x < data.GetLength(0); x++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    _data[y, x] = data[x, y];
                }
            }
        }
        
        [TestCase(8, 1, 0), TestCase(5, 1, 2), TestCase(0, 2, 3)]
        public void EdgeDetection(int expected, int x, int y)
        {
            Assert.AreEqual(expected, GetNumberOfExposedEdges(new TerraVector(x, y), _data));
            //Assert.AreEqual(5, GetNumberOfExposedEdges(new TerraVector(1, 4), _data));
            
            //List<TerraBlob> blobs = GetBlobs(_data).ToList();
            //Assert.AreEqual(2, blobs.Count);
        }
        
        [Test]
        public void BlobDetection()
        {
            List<TerraBlob> blobs = GetBlobs(_data).ToList();
            Assert.AreEqual(2, blobs.Count);
            Assert.AreEqual(8, blobs[0].VertexCount);
            Assert.AreEqual(11, blobs[1].VertexCount);
        }

        public struct TerraBlob
        {
            public IEnumerable<TerraVector> Vertices;
            public int VertexCount;
            public int Area;
        }

        public IEnumerable<TerraBlob> GetBlobs(bool[,] input)
        {
            List<TerraBlob> blobs = new List<TerraBlob>();
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            bool[,] occupied = new bool[width,height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isOccupied = occupied[x, y];

                    if (isOccupied)
                    {
                        continue;
                    }

                    int numberOfExposedVertices = GetNumberOfExposedEdges(new TerraVector(x, y), input);
                    if (input[x, y] && numberOfExposedVertices > 0)
                    {
                        TerraBlob blob = GenerateBlob(x, y, input, occupied);
                        if (blob.VertexCount > 0)
                        {
                            blobs.Add(blob);
                        }
                    }
                }
            }

            return blobs;
        }

        private TerraBlob GenerateBlob(
            int x, 
            int y,
            bool[,] input, 
            bool[,] occupied)
        {
            TerraBlob result = default(TerraBlob);
            List<TerraVector> vertices = new List<TerraVector>();
            occupied[x, y] = true;
            result.VertexCount++;
            vertices.Add(new TerraVector(x, y));
            RecursiveCompileVertices(new TerraVector(x, y),
                input,
                occupied, newVertice =>
                {
                    occupied[newVertice.x, newVertice.y] = true;
                    result.VertexCount++;
                    vertices.Add(newVertice);
                });

            result.Vertices = vertices;
            return result;
        }

        private void RecursiveCompileVertices(
            TerraVector vector,
            bool[,] input, 
            bool[,] occupied, 
            Action<TerraVector> OnVectorFound)
        {
            TerraVector newVertice = default(TerraVector);
            bool newVerticeFound = false;
            int minNumberOfExposedEdges = Int32.MinValue;
            for (int x = Math.Max(0, vector.x - 1); x < Math.Min(input.GetLength(0), vector.x + 2); x++)
            {
                for (int y = Math.Max(0, vector.y - 1); y < Math.Min(input.GetLength(1), vector.y + 2); y++)
                {
                    if (x == vector.x && y == vector.y || occupied[x, y] || !input[x, y])
                    {
                        continue;
                    }

                    TerraVector vertice = new TerraVector(x, y);
                    int numberOfExposedEdges = GetNumberOfExposedEdges(vertice, input);

                    if (numberOfExposedEdges > 0 && numberOfExposedEdges > minNumberOfExposedEdges)
                    {
                        newVertice = vertice;
                        minNumberOfExposedEdges = numberOfExposedEdges;
                        newVerticeFound = true;
                    }
                }
            }

            if (newVerticeFound)
            {
                OnVectorFound(newVertice);
                RecursiveCompileVertices(newVertice, input, occupied, OnVectorFound);
            }
        }

        private int GetNumberOfExposedEdges(
            TerraVector vector,
            bool[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int numberOfExposedEdges = 0;
            for (int x = vector.x - 1; x < vector.x + 2; x++)
            {
                for (int y = vector.y - 1; y < vector.y + 2; y++)
                {
                    if (x == vector.x && y == vector.y)
                    {
                        continue;
                    }

                    if (x < 0 || y < 0 || x >= width || y >= height)
                    {
                        numberOfExposedEdges++;
                        continue;
                    }
                    if (!input[x, y])
                    {
                        numberOfExposedEdges++;
                    }
                }
            }

            return numberOfExposedEdges;
        }
    }
}