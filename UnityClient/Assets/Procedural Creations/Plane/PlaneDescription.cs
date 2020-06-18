using UnityEngine;
using System.Collections;

/// <summary>
/// This description of the plane for the procedural generator.
/// (C) Copyright 2013 by Paul C. Isaac 
/// </summary>
[System.Serializable]
public class PlaneDescription
{
	[System.Serializable]
	public class UV_Info
	{
		public Vector2 Min= new Vector2(0,0);
		public Vector2 Max = new Vector2(1,1);
	};
	
	// Two-sided polygons have a second set of vertices/triangles with reverse winding so that it is visible from front/back.
	public bool TwoSided = false;
	
	public enum AxisEnum { X,Y,Z };
	
	// Normal determines Horizontal and Vertical axes
	public AxisEnum NormalAxis = AxisEnum.Z;
	
	// Default (4) is rectangle
	public int PolygonVertices = 4;

	// Number of triangle mesh grid boxes across/down 
	// Ignored if non-rectangular polygon
	public int GridHorizontal = 2;
	public int GridVertical = 2;

	// If the plane's corner or center should be at the objects origin.
	public bool Center = true;

	// Horizontal or vertical flip how the texture image appears on the plane.
	public bool FlipX = false;
	public bool FlipY = true;
	
	// Horizontal and Vertical size of plane
	public Vector2 Size = new Vector2(1,1);
	
	// Defines the rectangular subset of the texture that is mapped to the plane.
	public UV_Info TextureCoords = new UV_Info();

	// This is (true) if the plane is not a simple Rectangle grid.
	public bool UsePolygon
	{
		get { return PolygonVertices != 4; }
	}
	
	public Vector3 AxisHorizontal
	{
		get
		{
			switch (NormalAxis)
			{
			case AxisEnum.X: return Vector3.back; // -Z
			case AxisEnum.Y: return Vector3.right; // X
			case AxisEnum.Z: return Vector3.right; // X
			}
			return Vector3.zero;
		}
	}
	
	public Vector3 AxisVertical
	{
		get
		{
			switch (NormalAxis)
			{
			case AxisEnum.X: return Vector3.up;        // Y
			case AxisEnum.Y: return Vector3.forward; // Z
			case AxisEnum.Z: return Vector3.up;       // Y
			}
			return Vector3.zero;
		}
	}
};
