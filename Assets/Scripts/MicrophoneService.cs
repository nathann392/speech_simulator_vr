using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///
/// Microphone service provides a robust method for accessing the audio from
/// the main microphone. This class is a singleton and should be accessed using
/// the inherited `Instance` public static member. For example:
/// `MicrophoneService.Instance.GetAudioClip()`
/// This class is also implements the observer design pattern so that IMicrophoneObserver
/// objects are able to listen for new AudioClip segments by adding themselves to
/// the listeners list by doing the following:
/// `Microphone.Instance.ListenForAudioSegments(this)`
/// @author Spencer Brown
/// <see cref="https://github.com/patrickhimes/microphone-demo"/> 
/// <see cref="https://github.com/hardcyder/Audio-Spectrum-Data-Sample/blob/master/Assets/Scripts/MicrophoneInput.cs"/>
///
[RequireComponent(typeof(MeshRenderer))]
public class MicrophoneService : Singleton<MicrophoneService>
{
  [Tooltip("Length of audio buffer in seconds")]
  public int AUDIO_BUFFER_SECONDS = 10;

  [Tooltip("Rate of reading from buffer in seconds")]
  public int READ_RATE_SECONDS = 1;

  [Tooltip("Recording frequency")]
  public int frequency = 44100;

  // Audio clip buffer used for reading audio from microphone
  private AudioClip audioClipBuffer;

  // Mesh to update appearance
  MeshRenderer mesh;

  // List of available microphones on device
  private List<string> microphones = new List<string>();

  // The selected microphone to record audio from
  private string selectedMicrophone;

  // Whether class should be in debug mode
  public const bool DEBUG = false;

  // Recording coroutine reference
  private IEnumerator recordingCoroutine;

  // List of all recorded audio segments
  private List<AudioClip> audioSegments;

  // Keep track of clips (good for debugging)
  private int clipNum = 0;

  // Keep track of latest saved audio file path
  private string latestSavedFileName = "";

  // List of IMicrophoneObserver objects to notify of new AudioClip segments
  private List<IMicrophoneObserver> listeners = new List<IMicrophoneObserver>();

  ///
  /// <summary>This class is a Singleton.</summary>
  ///
  protected MicrophoneService() {}

  ///
  /// <summary>Get the audio clip from the recording.</summary>
  ///
  public AudioClip GetAudioClip()
  {
    return audioClipBuffer;
  }

  ///
  /// <summary>Get the position of the microphone.</summary>
  ///
  public int GetPosition()
  {
    return Microphone.GetPosition(selectedMicrophone);
  }

  ///
  /// <summary>Determine whether the microphone is being used
  /// to record.</summary>
  ///
  public bool IsRecording()
  {
    return Microphone.IsRecording(selectedMicrophone);
  }

  ///
  /// <summary>Get the path and file name of the latest saved audio file.
  /// </summary>
  ///
  public string GetLatestSavedFileName()
  {
    return latestSavedFileName;
  }

  // Start is called before the first frame update
  void Start()
  {
    foreach (string device in Microphone.devices)
    {
      Debug.Log("Name: " + device);
      microphones.Add(device);
    }

    if (microphones.Count > 0)
    {
      selectedMicrophone = microphones[0]; // TODO: Have more elegant way of selecting microphone device
    }

    mesh = GetComponent<MeshRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if(!Microphone.IsRecording(selectedMicrophone)) {
      mesh.material.color = Color.red;
    }
  }

  // Displays GUI controls when debugging.
  void OnGUI()
  {
    if (DEBUG)
    {
      if (GUI.Button(new Rect(10, 10, 150, 100), "Stop recording"))
      {
        StopAudioRecording();
      }
      if (GUI.Button(new Rect(170, 10, 150, 100), "Start recording"))
      {
        StartAudioRecording();
      }
    }
  }

  ///
  /// <summary>Stores audio segment and notifies listeners.</summary>
  ///
  private void AddAudioSegment(AudioClip segment)
  {
    audioSegments.Add(segment);
    NotifyListeners(segment);
  }

  ///
  /// <summary>Allows for an IMicrophoneObserver to register for new microphone
  /// audio segments.</summary>
  ///
  public void ListenForAudioSegments(IMicrophoneObserver listener)
  {
    listeners.Add(listener);
  }

  ///
  /// <description>Stop listening for audio segments.</description>
  ///
  public void StopListeningForAudioSegments(IMicrophoneObserver listener)
  {
    listeners.Remove(listener);
  }

  ///
  /// <summary>Sends audioclip segment to listeners.</summary>
  ///
  private void NotifyListeners(AudioClip segment)
  {
    foreach (IMicrophoneObserver listener in listeners)
    {
      // Give each listener a copy of the segment so that no one listener can mutate/delete 
      // the segment for all other listeners and MicrophoneService
      AudioClip cloned = CopySegment(segment, 0, segment.samples);
      listener?.ReceiveAudioSegment(cloned);
    }
  }

  ///
  /// <summary>Starts recording from the microphone.</summary>
  ///
  public void StartAudioRecording()
  {
    audioSegments = new List<AudioClip>();
    recordingCoroutine = RecordingHandler();

    // Change color to green
    mesh.material.color = Color.green;

    // Start recording to audio clip for AUDIO_BUFFER_SECONDS
    audioClipBuffer = Microphone.Start(selectedMicrophone, true, AUDIO_BUFFER_SECONDS, frequency);

    // Track if microphone is recording
    if (Microphone.IsRecording(selectedMicrophone))
    {
      // Microphone.IsRecording may give a false positive.
      // This busy while loop helps verify that the microphone is recording
      while (!(Microphone.GetPosition(selectedMicrophone) > 0))
      {
        // Busy loop
      }
      Debug.Log("started microphone with the name of " + selectedMicrophone);
      
      StartCoroutine(recordingCoroutine);
    }
    else
    {
      Debug.Log(selectedMicrophone + " is not working");
    }
  }

  ///
  /// <summary>Stops recording from the microphone.</summary>
  ///
  public void StopAudioRecording() {
    Microphone.End(selectedMicrophone);
    StopCoroutine(recordingCoroutine);
    Debug.Log("Stopped audio recording");
  }

  ///
  /// <description>This IEnumerator will grab chunks of audio from the MicrophoneService
  /// to send to IBM Watson for speech-to-text.</description>
  ///
  private IEnumerator RecordingHandler()
  {
      AudioClip _recording = GetAudioClip();
      
      // Recording must be working to continue
      if (_recording == null)
      {
          yield break;
      }

      // Store the last position in the AudioClip from the MicrophoneService
      int lastPosition = 0;

      // Loop while there is a recording and while there is a routine
      while (IsRecording() && _recording != null)
      {
          int currentPosition = GetPosition();

          // Get the length of unsent audio
          int sampleLength = currentPosition - lastPosition;

          // Read from circular array depending on current and last positions
          if (sampleLength < 0) {
            // Get audio segment that is at the end of the buffer
            AudioClip endingSegment = CopySegment(_recording, lastPosition, _recording.samples - lastPosition);
            AddAudioSegment(endingSegment);
            // Get audio segment that is at the beginning of the buffer
            AudioClip beginningSegment = CopySegment(_recording, 0, currentPosition);
            AddAudioSegment(beginningSegment);
          } else {
            // Audio segment is fully intact
            AudioClip clip = CopySegment(_recording, lastPosition, sampleLength);
            AddAudioSegment(clip);
          }

          // Update last position with the current position
          lastPosition = currentPosition;

          // Wait for one second before sending more audio to IBM Watson
          yield return new WaitForSeconds(READ_RATE_SECONDS);
      }
      
      yield break;
  }

  ///
  /// <description>Combines the list of audio segments and outputs
  /// a single AudioClip of all the segments.</description>
  ///
  private AudioClip combineAudioSegments()
  {
    AudioClip _recording = GetAudioClip();
    int combinedSampleLength = 0;

    foreach (AudioClip segment in audioSegments)
    {
      combinedSampleLength += segment.samples;
    }

    AudioClip mergedClip = AudioClip.Create("Recording", combinedSampleLength, _recording.channels, frequency, false);
    int currentPosition = 0;

    foreach (AudioClip segment in audioSegments)
    {
      float[] samples = new float[segment.samples];
      segment.GetData(samples, 0);
      mergedClip.SetData(samples, currentPosition);

      currentPosition += segment.samples;
    }

    return mergedClip;
  }

  ///
  /// <summary>Saves audio with default name.</summary>
  ///
  public void SaveAudio()
  {
    string audioFileName = MicrophoneService.generateFileName();
    SaveAudio(audioFileName);
  }

  ///
  /// <summary>Saves audio with a given name.</summary>
  /// <param name="audioFileName">The name (including relative file path)
  /// of the file to be saved.</param>
  ///
  public void SaveAudio(string audioFileName)
  {
    Debug.Log("Attempting to save to wav file");

    latestSavedFileName = audioFileName;

    SaveWav saver = new SaveWav();
    AudioClip fullClip = combineAudioSegments();
    saver.Save(audioFileName, fullClip);
  }

  ///
  /// <summary>Generates a default file name to be used for saving
  /// the audio file.</summary>
  ///
  public static string generateFileName()
  {
    string DATE_TIME_FORMAT = "yyyy-MM-dd-HHmmss";
    string timestamp = DateTime.Now.ToString(DATE_TIME_FORMAT);

    return string.Format("saved-speeches/speech-{0}.wav", timestamp);
  }

  ///
  /// <description>Copies a segment from an original AudioClip.</description>
  ///
  private AudioClip CopySegment(AudioClip original, int startSample, int sampleLength)
  {
    float[] samples = new float[sampleLength];
    original.GetData(samples, startSample);

    AudioClip clip = AudioClip.Create("Recording" + clipNum++, sampleLength, original.channels, original.frequency, false);
    clip.SetData(samples, 0);

    return clip;
  }
}
