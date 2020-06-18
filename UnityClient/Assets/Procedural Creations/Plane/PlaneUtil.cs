using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This wizard class creates a polygonal plane based on various user parameters.
/// (C) Copyright 2013 by Paul C. Isaac 
/// </summary>
public class PlaneUtil
{
	class MeshData
	{
		public List<Vector3> VertexList = new List<Vector3>();
		public List<int> IndexList = new List<int>();
		public List<Vector2> UvList = new List<Vector2>();
		
		public MeshData()
		{
			VertexList = new List<Vector3>();
			IndexList = new List<int>();
		}
		
		public void AddVertex(Vector3 pos)
		{
			VertexList.Add(pos);
		}
		
		public void AddTriangle(int a, int b, int c)
		{
			IndexList.Add(a);
			IndexList.Add(b);
			IndexList.Add(c);
		}
		
		public void AddTexCoord(Vector3 uv)
		{
			UvList.Add(uv);
		}
	}
	
	static void CreateRect(PlaneDescription desc, MeshData mesh)
	{
		float hFull, vFull;
		float hStep, vStep;
		
		int h = (desc.GridHorizontal > 0) ? desc.GridHorizontal : 1;
		int v = (desc.GridVertical > 0) ? desc.GridVertical : 1;
		
		hFull = desc.Size.x;
		vFull = desc.Size.y;
		hStep = desc.Size.x / h;
		vStep = desc.Size.y / v;
		
		Vector3 origin = Vector3.zero;
		if (desc.Center)
		{
			origin = (-0.5f*hFull) * desc.AxisHorizontal + (-0.5f*vFull) * desc.AxisVertical;
		}
		
		for (int y=0; y<=v; ++y)
		{
			for (int x=0; x<=h; ++x)
			{
				Vector3 pos = origin + (x*hStep)*desc.AxisHorizontal + (y*vStep)*desc.AxisVertical;
				mesh.AddVertex(pos);
				float tx = (x*hStep)/hFull;
				float ty = (y*vStep)/vFull;
				if (desc.FlipX) tx = 1-tx;
				if (!desc.FlipY) ty = 1-ty;
				float tu = Mathf.Lerp(desc.TextureCoords.Min.x,desc.TextureCoords.Max.x,tx);
				float tv = Mathf.Lerp(desc.TextureCoords.Min.y,desc.TextureCoords.Max.y,ty);
				mesh.AddTexCoord(new Vector2(tu,tv));
			}
		}

		int twoBase = 0;
		
		if (desc.TwoSided)
		{
			// A second set of vertices is required so that a unique normal can be generated.
			twoBase = mesh.VertexList.Count;
			for (int i=0; i<twoBase; ++i)
			{
				mesh.AddVertex(mesh.VertexList[i]);
				mesh.AddTexCoord(mesh.UvList[i]);
			}
		}
		
		for (int y=0; y<v; ++y)
		{
			for (int x=0; x<h; ++x)
			{
				int a = x + y*(h+1);
				int b = a+1;
				int c = b + (h+1);
				int d = c-1;
				if (true)
				{
					mesh.AddTriangle(a,c,b);
					mesh.AddTriangle(a,d,c);
				}
				if (desc.TwoSided)
				{
					mesh.AddTriangle(twoBase+a,twoBase+b,twoBase+c);
					mesh.AddTriangle(twoBase+a,twoBase+c,twoBase+d);
				}
			}
		}
	}

	static void CreatePolygon(PlaneDescription desc, MeshData mesh)
	{
		// Limit vertices to triangle or reasonable circle
		int vcount = Mathf.Clamp(desc.PolygonVertices,3,1024);

		Vector3 origin = desc.Center ? (-0.5f*desc.Size.x) * desc.AxisHorizontal + (-0.5f*desc.Size.y) * desc.AxisVertical : Vector3.zero;
		
		for (int i=0; i<vcount; ++i)
		{
			float angle = i*2*Mathf.PI/vcount;
			float tx = 0.5f+0.5f*Mathf.Sin(angle); // 0..1
			float ty = 0.5f+0.5f*Mathf.Cos(angle); // 0..1
			Vector3 pos = origin + (tx*desc.Size.x)*desc.AxisHorizontal + (ty*desc.Size.y)*desc.AxisVertical;
			mesh.AddVertex(pos);
			if (desc.FlipX) tx = 1-tx;
			if (!desc.FlipY) ty = 1-ty;
			float tu = Mathf.Lerp(desc.TextureCoords.Min.x,desc.TextureCoords.Max.x,tx);
			float tv = Mathf.Lerp(desc.TextureCoords.Min.y,desc.TextureCoords.Max.y,ty);
			mesh.AddTexCoord(new Vector2(tu,tv));
		}
		
		int twoBase = 0;
		
		if (desc.TwoSided)
		{
			// A second set of vertices is required so that a unique normal can be generated.
			twoBase = mesh.VertexList.Count;
			for (int i=0; i<twoBase; ++i)
			{
				mesh.AddVertex(mesh.VertexList[i]);
				mesh.AddTexCoord(mesh.UvList[i]);
			}
		}
		
		int tris = (vcount-2);
		for (int i=0; i<tris; ++i)
		{
			int a = 0;
			int b = i+1;
			int c = b+1;
			if (true)
			{
				mesh.AddTriangle(a,b,c);
			}
			if (desc.TwoSided)
			{
				mesh.AddTriangle(twoBase+a,twoBase+c,twoBase+b);
			}
		}
	}
	
	static public void Generate(PlaneDescription desc, GameObject obj)
	{
		MeshData data = new MeshData();
		
		if (desc.UsePolygon)
		{
			CreatePolygon(desc,data);
		}
		else
		{
			CreateRect(desc,data);
		}
		
        Mesh mesh = new Mesh();

        mesh.vertices = data.VertexList.ToArray();
        mesh.triangles = data.IndexList.ToArray();
		mesh.uv = data.UvList.ToArray();
		
        mesh.RecalculateBounds();
        mesh.Optimize();
		mesh.RecalculateNormals();
		
        MeshFilter mFilter = obj.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (mFilter == null)
		{
			mFilter = obj.AddComponent( typeof(MeshFilter) ) as MeshFilter;
		}
        mFilter.mesh = mesh;
		if (obj.GetComponent<Renderer>() == null)
		{
			obj.AddComponent(typeof(MeshRenderer));
		}
		if (obj.GetComponent<Renderer>().sharedMaterial == null)
		{
			obj.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Diffuse"));
		}
		
		UnityEngine.Debug.Log("# vertices = "+mesh.vertexCount.ToString());
		UnityEngine.Debug.Log("# triangles = "+mesh.triangles.Length/3);
	}
}