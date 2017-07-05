using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class fade : MonoBehaviour
{
	public float fadeSpeed
	= 1.5f;
	public string nextscene;
	private bool sceneStarting
	= true;
	private bool sceneEnding
	= false;

	void Awake()
	{
		//		guiTexture.pixelInset
		//		= new Rect(0f,
		//			0f, Screen.width, Screen.height);
		GetComponent <GUITexture> () .pixelInset= new Rect(0f,
			0f, Screen.width, Screen.height);
	}

	void Update()
	{
		if (sceneStarting)
		{
			StartScene();
		}

		if (sceneEnding)
		{
			EndScene();
		}
	}

	void FadeToClear()
	{
		GetComponent <GUITexture> () .color
		= Color.Lerp(GetComponent <GUITexture> () .color, Color.clear, fadeSpeed * Time.deltaTime);
	}

	void FadeToBlack()
	{
		GetComponent <GUITexture> () .color
		= Color.Lerp (GetComponent <GUITexture> () .color, Color.black, fadeSpeed * Time.deltaTime);
	}

	public void StartScene()
	{
		FadeToClear();

		if(GetComponent <GUITexture> () .color.a
			< 0.05f)
		{
			GetComponent <GUITexture> () .color
			= Color.clear;
			GetComponent <GUITexture> () .enabled
			= false;
			sceneStarting 
			= false;
		}
	}

	public void EndScene()
	{
		GetComponent <GUITexture> () .enabled
		= true;
		FadeToBlack();

		if(GetComponent <GUITexture> () .color.a
			>= 0.95f)
		{
			sceneEnding
			= false;
			SceneManager.LoadScene (nextscene);
		}
	}

	void OnGUI()
	{
		if (!sceneStarting
			&& GUI.Button (new Rect(0,0,100,100),"NewLevel"))
		{
			sceneEnding
			= true;
		}
	}

}