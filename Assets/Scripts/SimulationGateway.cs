using System.Collections;
using UnityEngine;


public class SimulationGateway : MonoBehaviour
{
  [Tooltip("Text field to display prompt.")]
  public TMPro.TextMeshPro ResultsField;

  [Tooltip("Controller GameObject")]
  public GameObject controller = null;

  private bool _sessionStarted = false;

  void Start() {
    Debug.Log("Found game object with name of " + controller?.name);
  }

   /// <summary>
  /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
  /// is touched.
  /// </summary>
  public void OnPointerClick()
  {
    // Toggle _sessionStarted 
    _sessionStarted = !_sessionStarted;
    Debug.Log("SimulationGatway was clicked!");

    if (_sessionStarted) {
      controller?.SendMessage("BeginSession");
    } else {
      StartCoroutine(WaitForEndSession());
    }
  }

  public void UpdatePrompt(string text) {
    Debug.Log("Updating prompt with: " + text);
    ResultsField.text = text;
  }

  IEnumerator WaitForEndSession() {
    yield return new WaitForSeconds(3);
    controller?.SendMessage("EndSession");
  }
}