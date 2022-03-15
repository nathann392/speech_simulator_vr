using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonController : MonoBehaviour
{
    [Tooltip("Save button GameObject")]
    public GameObject SaveButton;

    [Tooltip("Audio checkbox GameObject")]
    public GameObject AudioCheckmark;

    [Tooltip("Microphone GameObject")]
    public GameObject Microphone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SaveButton.GetComponent<Image>().color = new Color32(57,195,0,80);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SaveButton.GetComponent<Image>().color = new Color32(57,195,0,137);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        bool audio_checked = FindObjectOfType<AudioCheckboxController>().GetCheckbox();
        
        if (audio_checked) {
            Microphone?.SendMessage("SaveAudio");
        }
        GetComponent<ResultsNS.ExportSpeechResults>().SendMessage("exportResultData");
    }
}
