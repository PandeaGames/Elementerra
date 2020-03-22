using System.Collections;
using System.Collections.Generic;
using PandeaGames.Data;
using UnityEngine;

public class TerraDebugMonoView : MonoBehaviour
{
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
}
