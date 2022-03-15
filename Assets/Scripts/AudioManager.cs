using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public Sound[] sounds;
	
    // Same as Start(), but called right before, so we can play sounds in the start
    void Awake()
    {
        foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			
			// move these to update to update them realtime during game play
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.spatialBlend = s.spatialBlend;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Play(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		// only play if not already playing
		s.source.Play();
	}
	
	public AudioSource GetSource(string name) {
		Sound s = Array.Find(sounds, sound => sound.name == name);
		//Debug.Log("Getting s.source: " + s.source);
		
		return s.source;
	}
}
