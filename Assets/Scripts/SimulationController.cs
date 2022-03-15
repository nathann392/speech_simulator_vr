using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

public class SimulationController : MonoBehaviour
{
  [Tooltip("Gateway GameObject")]
  public GameObject gateway = null;

  [Tooltip("Microphone GameObject")]
  public GameObject microphone = null;
  public TMPro.TextMeshPro ScoreField;

  public GameObject results_panel = null;
  public GameObject speech_quality_controller = null;
  public GameObject head_controller = null;
  public GameObject speech_metrics_controller = null;
  
  private bool _simulationHasStarted = false;

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Found gateway GameObject with name of " + gateway?.name);
    Debug.Log("Found microphone GameObject with name of " + microphone?.name);
    Debug.Log("Found results panel GameObject with name of " + results_panel?.name);
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void BeginSession() {
    speech_metrics_controller?.SendMessage("StartSpeech");
    results_panel?.SendMessage("CloseResultsPanel");
    
    Debug.Log("Simulation has begun");
    _simulationHasStarted = true;

    // Update gateway's prompt
    gateway?.SendMessage("UpdatePrompt", "Click me when you are finished!");

    // Start recording
    MicrophoneService.Instance.StartAudioRecording();
    // Start speech to text
    microphone?.SendMessage("DoSpeechToText");

  }

  public void EndSession() {
    Debug.Log("Ending session");
    _simulationHasStarted = false;

    gateway?.SendMessage("UpdatePrompt", "Great speech! Thank you.");

    MicrophoneService.Instance.StopAudioRecording();

    microphone?.SendMessage("StopSpeechToText");
    speech_metrics_controller?.SendMessage("EndSpeech");
    results_panel?.SendMessage("DisplayResultsPanel");
  }

  public bool IsSessionActive()
  {
    return _simulationHasStarted;
  }

}
