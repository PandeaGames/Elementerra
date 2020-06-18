using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// (C) Copyright 2013 by Paul C. Isaac 
public class PlaneGenerator : MonoBehaviour
{
	public PlaneDescription Description = new PlaneDescription();
	
	public bool ShowGizmos = true;
	public Color GridColor = Color.red;
	
	int GridWidth
	{
		get { return Description.GridHorizontal; }
		set { Description.GridHorizontal = value; }
	}
	int GridHeight
	{
		get { return Description.GridVertical; }
		set { Description.GridVertical = value; }
	}
	
	public bool IsValid()
	{
		int verts = GridWidth * GridHeight;
		int tris = GridWidth * GridHeight * 2;
		if (Description.Size.x > 0 && Description.Size.y > 0)
		{
			return GridWidth > 0 && GridHeight > 0 && verts <= 32768 && tris < 32768 && Description.PolygonVertices > 2;
		}
		return false;
	}
	
#if UNITY_EDITOR
	void DrawGizmosGrid(Color c)
	{
		Gizmos.color = c;
		if (Description.UsePolygon)
		{
			DrawPolygonByDelegate(Gizmos.DrawLine);
		}
		else
		{
			DrawGridByDelegate(Gizmos.DrawLine);
		}
	}

	public void DrawHandlesGrid(Color c)
	{
		Handles.color = c;
		if (Description.UsePolygon)
		{
			DrawPolygonByDelegate(Handles.DrawLine);
		}
		else
		{
			DrawGridByDelegate(Handles.DrawLine);
		}
	}
#endif
	
	public delegate void LineDelegate(Vector3 a, Vector3 b);
	
	float GetHorizontalSize()
	{
		return Vector3.Dot(this.transform.localScale,Description.AxisHorizontal)*Description.Size.x;
	}
	float GetVerticalSize()
	{
		return Vector3.Dot(this.transform.localScale,Description.AxisVertical)*Description.Size.y;
	}
	
	struct Bounds
	{
		public Vector3 H0,H1; // mid-left/right
		public Vector3 V0,V1; // mid-top/bottom
		public Vector3 TL,TR; // top-left/right
		public Vector3 BL,BR; // bot-left/right
	};
	
	Bounds GetBounds()
	{
		Bounds result = new Bounds();
		Vector3 hh= GetHorizontalSize() * Description.AxisHorizontal;
		Vector3 vv= GetVerticalSize() * Description.AxisVertical;
		if (Description.Center)
		{
			result.H0 = this.transform.position - 0.5f*(hh);
			result.V0 = this.transform.position - 0.5f*(vv);
			result.TL = this.transform.position - 0.5f*(hh) + 0.5f*(vv);
			result.TR = this.transform.position + 0.5f*(hh) + 0.5f*(vv);
			result.BL = this.transform.position - 0.5f*(hh) - 0.5f*(vv);
			result.BR = this.transform.position + 0.5f*(hh) - 0.5f*(vv);
		}
		else
		{
			result.H0 = this.transform.position + 0.5f*(vv);
			result.V0 = this.transform.position + 0.5f*(hh);
			result.TL = this.transform.position + (vv);
			result.TR = this.transform.position + (hh) + (vv);
			result.BL = this.transform.position;
			result.BR = this.transform.position + (hh);
		}
		result.H1 = result.H0 + hh;
		result.V1 = result.V0 + vv;
		return result;
	}
	
	void DrawGridByDelegate(LineDelegate drawLine)
	{
		Vector3 a,b;
		Vector3 h = Description.AxisHorizontal;
		Vector3 v = Description.AxisVertical;
		float dh = GetHorizontalSize()/GridWidth;
		float dv = GetVerticalSize()/GridHeight;
		Bounds bounds = GetBounds();
		for (int i=0; i<=GridHeight; ++i)
		{
			Vector3 vi = ((i)*dv)*v;
			a = bounds.BL + vi;
			b = bounds.BR + vi;
	        drawLine(a,b);
		}
		for (int i=0; i<=GridWidth; ++i)
		{
			Vector3 hi = ((i)*dh)*h;
			a = bounds.TL + hi;
			b = bounds.BL + hi;
	        drawLine(a,b);
		}
	}

	void DrawPolygonByDelegate(LineDelegate drawLine)
	{
		float da = 2*Mathf.PI/Description.PolygonVertices;
		float hsize = this.GetHorizontalSize();
		float vsize = this.GetVerticalSize();
		Vector3 origin = Description.Center ? (-0.5f*hsize) * Description.AxisHorizontal + (-0.5f*vsize) * Description.AxisVertical : Vector3.zero;
		for (int i=0; i<Description.PolygonVertices; ++i)
		{
			float angle = i*da;
			float angle1 = (i+1)*da;
			float tx = 0.5f+0.5f*Mathf.Sin(angle); // 0..1
			float ty = 0.5f+0.5f*Mathf.Cos(angle); // 0..1
			Vector3 pos = transform.position + origin + (tx*hsize)*Description.AxisHorizontal + (ty*vsize)*Description.AxisVertical;
			tx = 0.5f+0.5f*Mathf.Sin(angle1); // 0..1
			ty = 0.5f+0.5f*Mathf.Cos(angle1); // 0..1
			Vector3 pos1 = transform.position + origin + (tx*hsize)*Description.AxisHorizontal + (ty*vsize)*Description.AxisVertical;
			drawLine(pos,pos1);
		}
	}
	
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (!Selection.Contains(this.gameObject))
		{
			Color darker = Color.Lerp(GridColor,Color.black,0.67f);
			DrawGizmosGrid(darker);
		}
		else 
		{
			// The inspector has to draw the grid to be under the GUI buttons!
			//DrawHandlesGrid(GridColor);
		}
	}
	void OnDrawGizmosSelected()
	{
		// The inspector has to draw the buttons for click events to work?
		//Handles.BeginGUI();
		//	DrawGUI();//Camera.current);
		//Handles.EndGUI();
	}
#endif
	
	// Mouse position is in Viewport coordinates
	// Viewport (0,0) = bottom left
	// Screen (0,0) = top left
	bool InViewportRange(Camera camera,Vector2 mouse, int pixels, Vector3 worldPos)
	{
		Vector3 pos = camera.WorldToScreenPoint(worldPos);
		Debug.Log("InScreen = "+mouse.ToString()+" , "+pos.ToString());
		float max = Mathf.Max(Mathf.Abs(pos.x-mouse.x),Mathf.Abs(pos.y-mouse.y));
		if (max < pixels)
		{
			return true;
		}
		return false;
	}
	
	public void DrawGUI()
	{
		if (ShowGizmos)
		{
			int bw = 32;
			int bh = 28;
			Vector2 xy;
			Bounds bounds = GetBounds();
			// HORIZONTAL
			xy = Camera.current.WorldToScreenPoint( bounds.H0 );
			GUI.skin.button.fontSize = 24;
			GUI.skin.button.fontStyle = FontStyle.Bold;
			GUI.skin.button.alignment = TextAnchor.LowerCenter;
			if (GUI.Button(new Rect(xy.x-bw/2,Camera.current.pixelHeight-xy.y-bh/2,bw,bh),"-"))
			{
				if (this.GridWidth > 1) this.GridWidth -= 1;
				Event.current.Use();
			}
			GUI.skin.button.alignment = TextAnchor.LowerLeft;
			xy = Camera.current.WorldToScreenPoint( bounds.H1 );
			if (GUI.Button(new Rect(xy.x-bw/2,Camera.current.pixelHeight-xy.y-bh/2,bw,bh),"+"))
			{
				if (this.GridWidth < 255) this.GridWidth += 1;
				Event.current.Use();
			}
			// VERTICAL
			GUI.skin.button.alignment = TextAnchor.LowerCenter;
			xy = Camera.current.WorldToScreenPoint( bounds.V0 );
			if (GUI.Button(new Rect(xy.x-bw/2,Camera.current.pixelHeight-xy.y-bh/2,bw,bh),"-"))
			{
				if (this.GridHeight > 1) this.GridHeight -= 1;
				Event.current.Use();
			}
			GUI.skin.button.alignment = TextAnchor.LowerLeft;
			xy = Camera.current.WorldToScreenPoint( bounds.V1 );
			if (GUI.Button(new Rect(xy.x-bw/2,Camera.current.pixelHeight-xy.y-bh/2,bw,bh),"+"))
			{
				if (this.GridHeight < 255) this.GridHeight += 1;
				Event.current.Use();
			}
		}
	}
}
