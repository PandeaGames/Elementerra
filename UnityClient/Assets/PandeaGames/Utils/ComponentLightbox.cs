using System;
using UnityEngine;

public class ComponentLightbox : MonoBehaviour
{    
    private LightboxCamera _lightboxCameraCache;
    private LightboxCamera _lightboxCamera
    {
        get
        {
            if (_lightboxCameraCache == null)
            {
                GameObject cameraGameObject = new GameObject();
                cameraGameObject.transform.parent = transform;
                cameraGameObject.transform.position = Vector3.back * 3;
                _lightboxCameraCache = cameraGameObject.AddComponent<LightboxCamera>();
            }

            return _lightboxCameraCache;
        }
    }
    private GameObject _componentObject;
    
    public void GetComponentTexture(GameObject gameObject, Action<Texture2D> onComplete, Action onError)
    {
        Texture2D texture = null;
        gameObject.transform.parent = transform;
        gameObject.transform.position = Vector3.zero;
        _lightboxCamera.GetTexture(tex =>
        {
            Destroy(gameObject);
            onComplete(tex);
        }, onError);
    }
}
