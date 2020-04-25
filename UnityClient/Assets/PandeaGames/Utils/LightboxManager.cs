using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandeaGames.Utils
{
    public static class LightboxManager
    {
        private static Dictionary<int, GameObject> dictionary = new Dictionary<int, GameObject>();
        private static int _requestCount;
        public static void GetTexture(GameObject gameObject, Action<Texture2D> onComplete, Action onError)
        {
            ComponentLightbox lightbox = new GameObject("lightbox", new Type[]{typeof(ComponentLightbox)}).GetComponent<ComponentLightbox>();
            lightbox.gameObject.transform.position = new Vector3(1000, 1000, 1000 + _requestCount++ * 10);
            lightbox.GetComponentTexture(gameObject, onComplete, onError);
        }
    }
}