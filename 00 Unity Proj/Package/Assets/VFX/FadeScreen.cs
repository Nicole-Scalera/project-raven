using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FadeScreen : MonoBehaviour
{
	public static FadeScreen instance; // Singleton instance
	public static bool FadeCompleted; // Is fade completed?
	public float speed = 1; // Fade speed

	public Material Screen;
	public AudioMixer Audio;
	
	// Set the FadeTime float to the value from the shader editor
	private float FadeTime
	{
		set
		{
			Screen.SetFloat("_FadeTime", value);
			Audio.SetFloat("_MasterVolume", Mathf.Lerp(0, -80, value));
		}
	}
	
	private void Awake()
	{
		instance = this;
		FadeIn();
	}

	// Call to fade into current scene
	public void FadeIn()
	{
		StartCoroutine(interpolate(1, 0));
	}

	// Call to fade out of current scene
	public void FadeOut()
	{
		StartCoroutine(interpolate(0, 1));
	}
	
	// Transition coroutine
	private IEnumerator interpolate(float from, float to)
	{		
		FadeCompleted = false;
		float currentValue = from;
		
		// This loop will run once every frame until the fade is complete
		for (float time = 0; currentValue != to; time += Time.deltaTime * speed) 
		{
			// Using smoothstep for a smoother transition
			currentValue = Mathf.Clamp01(Mathf.SmoothStep(from, to, time));
			FadeTime = currentValue;
			yield return null;
		}
		FadeCompleted = true; // Set the fade as completed
	}
}
