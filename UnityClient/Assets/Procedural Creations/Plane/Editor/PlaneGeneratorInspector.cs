using UnityEditor;
using UnityEngine;
using System.Collections;

/// (C) Copyright 2013 by Paul C. Isaac 
[CustomEditor(typeof(PlaneGenerator))]
public class PlaneGeneratorInspector : Editor
{
	Vector2 V2(string title, Vector2 v)
	{
		EditorGUILayout.LabelField(title);
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("X",GUILayout.Width(12));
			float x = EditorGUILayout.FloatField(v.x);
			EditorGUILayout.LabelField("Y",GUILayout.Width(12));
			float y = EditorGUILayout.FloatField(v.y);
		EditorGUILayout.EndHorizontal();
		return new Vector2(x,y);
	}

	Vector2 V2WithPrefix(string prefix, float pad, Vector2 v)
	{
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField(prefix,GUILayout.Width(pad));
			EditorGUILayout.LabelField("X",GUILayout.Width(12));
			float x = EditorGUILayout.FloatField(v.x);
			EditorGUILayout.LabelField("Y",GUILayout.Width(12));
			float y = EditorGUILayout.FloatField(v.y);
		EditorGUILayout.EndHorizontal();
		return new Vector2(x,y);
	}
	
	bool Boolean(string prefix, float pad, bool value)
	{
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField(prefix,GUILayout.Width(pad));
			bool result = EditorGUILayout.Toggle(value);
		EditorGUILayout.EndHorizontal();
		return result;
	}
	
	int IntWithPrefix(string prefix, float pad, int value)
	{
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField(prefix,GUILayout.Width(pad));
			int result = EditorGUILayout.IntField(value);
		EditorGUILayout.EndHorizontal();
		return result;
	}
	
	public override void OnInspectorGUI ()
	{
		PlaneGenerator c = target as PlaneGenerator;
		PlaneDescription desc = c.Description;
		
		EditorGUIUtility.LookLikeControls(15f);

		//DrawDefaultInspector();
		
		float tab = 128;
		
		desc.TwoSided = Boolean("Two Sided",tab,desc.TwoSided);
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("Normal Axis",GUILayout.Width(tab));
			System.Enum e = (System.Enum)desc.NormalAxis;
			desc.NormalAxis = (PlaneDescription.AxisEnum)EditorGUILayout.EnumPopup(e);
		EditorGUILayout.EndHorizontal();

		desc.PolygonVertices = IntWithPrefix("Polygon Vertices",tab,desc.PolygonVertices);

		desc.Center = Boolean("Center",tab,desc.Center);

		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("Flip Texture",GUILayout.Width(tab));
			EditorGUILayout.LabelField("X",GUILayout.Width(12));
			desc.FlipX = EditorGUILayout.Toggle(desc.FlipX,GUILayout.Width(32));
			EditorGUILayout.LabelField("Y",GUILayout.Width(12));
			desc.FlipY = EditorGUILayout.Toggle(desc.FlipY,GUILayout.Width(32));
		EditorGUILayout.EndHorizontal();
		
		c.ShowGizmos = Boolean("Show Gizmos",tab,c.ShowGizmos);
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("Grid Color",GUILayout.Width(tab));
			c.GridColor = EditorGUILayout.ColorField(c.GridColor);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
			//EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("Grid Units");
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("",GUILayout.Width(4));
			EditorGUILayout.LabelField("X",GUILayout.Width(12));
			desc.GridHorizontal = EditorGUILayout.IntField(desc.GridHorizontal);
			EditorGUILayout.LabelField("Y",GUILayout.Width(12));
			desc.GridVertical = EditorGUILayout.IntField(desc.GridVertical);
		EditorGUILayout.EndHorizontal();
		
		desc.Size = V2("Mesh Size",desc.Size);
		
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Texture Coords",GUILayout.Width(96));
			if (GUILayout.Button(new GUIContent("UV","Reset Texture Coords"),GUILayout.Width(32)))
			{
				desc.TextureCoords.Min = new Vector2(0,0);
				desc.TextureCoords.Max = new Vector2(1,1);
				EditorUtility.SetDirty(target);
			}
		EditorGUILayout.EndHorizontal();
		
		desc.TextureCoords.Min = V2WithPrefix ("Min", 32, desc.TextureCoords.Min);
		desc.TextureCoords.Max = V2WithPrefix ("Max", 32, desc.TextureCoords.Max);

		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			if (DrawButton("Generate", "Generate Plane", c.IsValid(), 240))
			{
				PlaneUtil.Generate(c.Description,c.gameObject);
				EditorUtility.SetDirty(target);
			}
			GUILayout.FlexibleSpace();
		}
		EditorGUILayout.EndHorizontal();
		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}
	
	/// <summary>
	/// Helper function to draw button in enabled or disabled state.
	/// </summary>
	static bool DrawButton (string title, string tooltip, bool enabled, float width)
	{
		if (enabled)
		{
			// Draw a regular button
			GUI.color = Color.green;
			return GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width), GUILayout.Height(24));
		}
		else
		{
			// Button should be disabled -- draw it darkened and ignore its return value
			Color old = GUI.color;
			GUI.color = new Color(1f, 0.33f, 0.33f, 0.35f);
			GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width), GUILayout.Height(24));
			GUI.color = old;
			return false;
		}
	}
	
	public void OnSceneGUI()
    {
		PlaneGenerator plane = target as PlaneGenerator;
		plane.DrawHandlesGrid(plane.GridColor);
		Handles.BeginGUI();
			plane.DrawGUI();
		Handles.EndGUI();
    }	
}
