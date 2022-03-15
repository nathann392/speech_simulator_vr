using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(MeshRenderer))]
public class ReplayService : MonoBehaviour
{
  public string audioFileName = "entreprenevr-audio-test.wav";
  AudioSource audioSource;
  MeshRenderer mesh;
  bool requestPlay = false;
  bool isAudioLoading = false;
  bool DEBUG = false;

  // Start is called before the first frame update
  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    mesh = GetComponent<MeshRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    // If there is a request to play the audio
    if (requestPlay)
    {
      // If the audio is not finished loading
      if (audioSource.clip == null && !isAudioLoading)
      {
        // Load Audio clip from storage
        StartCoroutine(LoadAudioClipFromStorage(audioFileName));
      }
      // If the audio has finished loading
      else if (audioSource.clip != null && !audioSource.isPlaying)
      {
        audioSource.Play();

        // Change color to green
        mesh.material.color = Color.green;
      }
    }
    // Stop only if the audio is already playing
    else if (audioSource.isPlaying)
    {
      audioSource.Stop();

      // Reset the clip to signal that the audio needs to be reloaded from disk next play request
      audioSource.clip = null;

      // Change color to red
      mesh.material.color = Color.red;
    }
  }

  /// <summary>
  /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
  /// is touched.
  /// </summary>
  public void OnPointerClick()
  {
    Debug.Log("ReplayService was clicked");

    // Toggle play/stop
    requestPlay = !requestPlay;
  }

  void OnGUI()
  {
    if (DEBUG)
    {
      if (GUI.Button(new Rect(10, 120, 150, 100), "Play audio from disk"))
      {
        OnPointerClick();
      }
      if (GUI.Button(new Rect(170, 120, 150, 100), "Stop and reload\naudio from disk"))
      {
        OnPointerClick();
      }
    }
  }

  ///
  /// Load a WAV file from device disk.
  /// Created this method based on a Unity form and from the Unity documentation.
  /// <see cref="https://forum.unity.com/threads/importing-audio-files-at-runtime.140088/">
  ///
  IEnumerator LoadAudioClipFromStorage(string filename)
  {

    // Signal that the laoding has begun
    isAudioLoading = true;

    // Create the file uri on the system
    string filepath = "file://" + Path.Combine(Application.persistentDataPath, filename);
    Debug.Log("filepath: " + filepath);

    // Fetch the audio clip from the system
    using (UnityWebRequest audio = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.WAV))
    {
      yield return audio.SendWebRequest();


      // Check for network errors
      if (audio.isNetworkError)
      {
        Debug.Log(audio.error);
      }

      else
      {
        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(audio);
        audioClip.name = filename;
        audioSource.clip = audioClip;
        Debug.Log("Loaded " + filename);
      }

      // Signal that the loading is now finished
      isAudioLoading = false;

    }

  }

}
