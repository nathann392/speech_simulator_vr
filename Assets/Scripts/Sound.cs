using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
	public string name;
	
    public AudioClip clip;
	
	[Range(0f, 1f)]
	public float volume;
	[Range(0.1f, 3f)]
	public float pitch;
	[Range(0f, 1f)]
	public float spatialBlend;
	
	public bool loop;
	
	// Hide this in the inspector since audio managers populates it
	[HideInInspector]
	public AudioSource source;
}
