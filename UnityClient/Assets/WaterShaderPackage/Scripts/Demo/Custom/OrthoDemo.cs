using UnityEngine;
using UnityEngine.SceneManagement;

public class OrthoDemo : MonoBehaviour
{
	public Material mat;
	public string sceneName;

	public void OnEdgeChanged(float newValue)
	{
		mat.SetFloat("_EdgeFade", newValue);
	}

	public void OnNormalStrengthChanged(float newValue)
	{
		mat.SetFloat("_NormalStrength", newValue);
	}

	public void OnLightOffsetChanged(float newValue)
	{
		mat.SetFloat("_LightOffset", newValue);
	}

	public void OnReflectivityChanged(float newValue)
	{
		mat.SetFloat("_Reflectivity", newValue);
	}

	public void NextScene()
	{
		SceneManager.LoadScene(sceneName);
	}
}
