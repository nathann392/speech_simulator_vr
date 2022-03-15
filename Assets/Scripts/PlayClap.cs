using UnityEngine.Audio;
using UnityEngine;

public class PlayClap : MonoBehaviour
{
    private int randomClap = 0;
	private float timeCounter = 0.0f;
	private bool isPlaying = false;
	private int speechQuality = 0;

    // Update is called once per frame
    void Update() {
		if (!isPlaying) {
			speechQuality = FindObjectOfType<SpeechQualityController>().GetSpeechQuality();
			//Debug.Log(speechQuality);
			if (speechQuality >= 99) {
				//Debug.Log("Playing clip.");
				isPlaying = true;
				GetClap().Play();
			}
		} else {
			timeCounter += Time.deltaTime;
			if (timeCounter > 2.0f) {
				//Debug.Log("Stopping clip.");
				timeCounter = 0.0f;
				isPlaying = false;
				// This prints an error message because it is being called more than once?
				GetComponent<AudioSource>().Stop();
			}
		}
    }
	
	private AudioSource GetClap() {
		randomClap = Random.Range(0, 3);
		//Debug.Log("Getting clip: " + (randomClap + 1));
		switch (randomClap) {
			case 0:
				return FindObjectOfType<AudioManager>().GetSource("Clapping1");
			case 1:
				return FindObjectOfType<AudioManager>().GetSource("Clapping2");
			case 2:
				return FindObjectOfType<AudioManager>().GetSource("Clapping3");
		}
		
		return null;
	}
}
