using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// This wizard class creates a polygonal plane based on various user parameters.
/// (C) Copyright 2013 by Paul C. Isaac 
/// </summary>
public class CreatePlane : ScriptableWizard
{
	public PlaneDescription Description;
	
    [MenuItem ("GameObject/Create Other/Procedural Creations/Plane")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create Plane", typeof(CreatePlane));
    }
 
    void OnWizardCreate()
    {
        GameObject root = new GameObject( "Plane" );
		PlaneGenerator gen = root.AddComponent<PlaneGenerator>();
		gen.Description = Description;
		PlaneUtil.Generate(Description,root);
        Selection.activeObject = root;
    }
}