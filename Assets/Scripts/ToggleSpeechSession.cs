using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpeechSession : MonoBehaviour
{
    bool conductingSpeech = false;

    void OnMouseUpAsButton()
    {
        if (!conductingSpeech)
        {
            conductingSpeech = true;
            Debug.Log("Please start your speech.");
        } else
        {
            conductingSpeech = false;
            Debug.Log("Thank you for your speech!");
        }
    }
}
