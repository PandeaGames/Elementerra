using System;
using System.Collections;
using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.ViewModels;
#if  UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class TerraDebugMonoView : MonoBehaviour
{
    public enum DebugRenderModes
    {
        None, 
        GrassPotential,
    }

    [SerializeField]
    private DebugRenderModes _debugRenderMode;
    
    private GameObject _debugWindow;
    
    // Start is called before the first frame update
    void Start()
    {
        _debugWindow = Instantiate(TerraGameResources.Instance.TerraDebugWindow, transform, worldPositionStays:false);
        _debugWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && !_debugWindow.activeSelf)
        {
            ShowDebugWindow();
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) && _debugWindow.activeSelf)
        {
            HideDebugWindow();
        }
    }

    private void ShowDebugWindow()
    {
        _debugWindow.SetActive(true);
    }
    
    private void HideDebugWindow()
    {
        _debugWindow.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
        switch (_debugRenderMode)
        {
            case DebugRenderModes.GrassPotential:
            {
#if  UNITY_EDITOR
                foreach (TerraGrassPotentialNodeGridPoint potential in vm.GrassPotential.AllData())
                {
                    Handles.Label(vm.Geometry[potential.Vector], potential.Data.ToString());
                }
                #endif
                break;
            }
        }
    }
}
