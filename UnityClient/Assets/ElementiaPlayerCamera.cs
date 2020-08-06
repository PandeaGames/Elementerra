using System.Collections;
using System.Collections.Generic;
using PandeaGames.Data;
using Terra.SerializedData.Entities;
using UnityEngine;

public class ElementiaPlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        vThirdPersonCamera camera = FindObjectOfType<vThirdPersonCamera>();
        FirstPersonLook FirstPersonLook = FindObjectOfType<FirstPersonLook>();

        camera.target = transform;
        if (FirstPersonLook != null)
        {
            FirstPersonLook.character = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
