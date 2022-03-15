using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanelController : MonoBehaviour
{
    public GameObject controller = null;
    public GameObject results_panel;
    public GameObject overall_text;
    public GameObject filler_text;
    public GameObject pacing_text;
    public GameObject bad_head_text;
    public GameObject length_text;
    public GameObject total_words_text;
    public GameObject abnormal_text;
    public GameObject repeated_words_text;

    // Start is called before the first frame update
    void Start()
    {
        CloseResultsPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Displays at end of session
    public void DisplayResultsPanel() {
        // Open panel
        results_panel.gameObject.transform.localScale = new Vector3((float)1.4,(float)1.4,(float)1.4);

        // Set values for each metric on panel
        SetPacing();
        SetAudEng();
        SetFiller();
        SetQuality();
        SetLength();
        SetTotalWords();
        SetAbnormalPauses();
        SetRepetitiveWords();
    }

    // Closes at start of session
    public void CloseResultsPanel() {
        results_panel.gameObject.transform.localScale = new Vector3(0,0,0);
    }


    /***************************************/
    /* SET VALUES FOR EACH METRIC ON PANEL */
    /***************************************/

    public void SetPacing() {
        // Get values
        TMPro.TextMeshProUGUI pacing_text_tmp = pacing_text.GetComponent<TMPro.TextMeshProUGUI>();
        float wps = FindObjectOfType<SpeechMetricsController>().GetPacingWps();

        if (!float.IsNaN(wps)) {
            if (wps < 1.9) {
                pacing_text_tmp.text = "Slow"; 
            }
            else if (wps > 1.9 && wps < 2.55) {
                pacing_text_tmp.text = "Medium";
            }
            else if (wps > 2.55) {
                pacing_text_tmp.text = "Fast";
            }
        }
        else {
            pacing_text_tmp.text = "None";
        }

    }

    public void SetAudEng() {
        // Get values
        TMPro.TextMeshProUGUI bad_head_text_tmp = bad_head_text.GetComponent<TMPro.TextMeshProUGUI>();
        int aud_eng_count = FindObjectOfType<SpeechMetricsController>().GetAudienceEngagement();     

        // Set loss of eye contact value
        bad_head_text_tmp.text = aud_eng_count.ToString();
    }

    public void SetFiller() {
        // Get values
        TMPro.TextMeshProUGUI filler_text_tmp = filler_text.GetComponent<TMPro.TextMeshProUGUI>();
        int filler_count = FindObjectOfType<SpeechMetricsController>().GetFiller();

        // Set filler value
        filler_text_tmp.text = filler_count.ToString();
    }

    public void SetLength() {
        // Get tmp
        TMPro.TextMeshProUGUI length_text_tmp = length_text.GetComponent<TMPro.TextMeshProUGUI>();
        int speech_length = FindObjectOfType<SpeechMetricsController>().GetSpeechLengthSeconds();

        // Format time
        int seconds = speech_length % 60;
        int minutes = speech_length / 60;
        string ans = string.Format("{0:D2}m:{1:D2}s", minutes, seconds);

        // Set length value
        length_text_tmp.text = ans;
    }

    public void SetTotalWords() {
        // Get tmp
        TMPro.TextMeshProUGUI total_words_text_tmp = total_words_text.GetComponent<TMPro.TextMeshProUGUI>();

        int word_count = FindObjectOfType<SpeechMetricsController>().GetTotalWords();

        total_words_text_tmp.text = word_count.ToString();
    }

    public void SetAbnormalPauses() {
        TMPro.TextMeshProUGUI abnormal_text_tmp = abnormal_text.GetComponent<TMPro.TextMeshProUGUI>();
        int pauses = FindObjectOfType<SpeechMetricsController>().GetPauses();

        abnormal_text_tmp.text = pauses.ToString();
    }
    
    public void SetRepetitiveWords() {
        TMPro.TextMeshProUGUI repeated_words_text_tmp = repeated_words_text.GetComponent<TMPro.TextMeshProUGUI>();
        string repeat_words = FindObjectOfType<SpeechMetricsController>().GetRepetitiveWords();
        
        repeated_words_text_tmp.text = repeat_words;
    }

    public void SetQuality() {
        TMPro.TextMeshProUGUI quality_text_tmp = overall_text.GetComponent<TMPro.TextMeshProUGUI>();
        float quality = FindObjectOfType<SpeechGradeCalculator>().GetQuality();

        if (quality < 0.333) {
            quality_text_tmp.text = "Great Start!";
            quality_text_tmp.color =  Color.yellow;
        } 
        else if (quality < 0.667) {
            quality_text_tmp.text = "Good Job!";
            quality_text_tmp.color =  new Color32(154, 205, 50, 255);
        } 
        else {
            quality_text_tmp.text = "Excellent!";  
            quality_text_tmp.color =  Color.green;
        }
    }
}
