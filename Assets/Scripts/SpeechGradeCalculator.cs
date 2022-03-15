using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class SpeechGradeCalculator : MonoBehaviour
{
    private float filler_grade;
    private float pacing_grade;
    private float audience_eng_grade;
    private float pauses_grade;
    private float quality_grade;

    public int max_fillers_per_min = 5;
    public int max_aud_eng_per_min = 3;
    public int slowest_words_per_min = 80;
    public int perfect_words_per_min = 130;
    public int fastest_words_per_min = 180;
    public int max_pauses_per_min = 5;

    private int elapsed_time = 0;
    
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        elapsed_time = FindObjectOfType<SpeechMetricsController>().GetSpeechLengthSeconds();
    }
    
    public void CalculateFillerGrade() {
        int filler_count = FindObjectOfType<SpeechMetricsController>().GetFiller();
        float filler_per_sec = (float)filler_count/(float)elapsed_time;
        float max_fillers_per_sec = (float)max_fillers_per_min/(float)60;

        // 0% = 0.0833+ fillers per sec = 5+ fillers per min
        // 100% = 0 fillers per sec
        if (filler_per_sec <= max_fillers_per_sec) {
            filler_grade = 1 - (filler_per_sec/max_fillers_per_sec);
        }
        else {
            filler_grade = 0;
        }

    }

    public void CalculateAudEngGrade() {
        int aud_eng_count = FindObjectOfType<SpeechMetricsController>().GetAudienceEngagement();     
        float aud_eng_per_sec = (float)aud_eng_count/(float)elapsed_time;
        float max_aud_eng_per_sec = (float)max_aud_eng_per_min/(float)60;

        // 0% = 0.05+ aud eng per sec = 3+ times looked away per min
        // 100% = 0 aud eng per sec
        if (aud_eng_per_sec <= max_aud_eng_per_sec) {
            audience_eng_grade = 1 - (aud_eng_per_sec/max_aud_eng_per_sec);
        }
        else {
            audience_eng_grade = 0;
        }

    }    

    public void CalculatePacingGrade() {
        float pacing_wps = FindObjectOfType<SpeechMetricsController>().GetPacingWps();
        float slowest_wpm_per_sec = (float)slowest_words_per_min/(float)60;
        float perfect_wpm_per_sec = (float)perfect_words_per_min/(float)60;
        float fastest_wpm_per_sec = (float)fastest_words_per_min/(float)60;

        // 0% = < 80 wpm OR > 180 wpm
        // 100% = 130 wpm
        if (pacing_wps < slowest_wpm_per_sec || pacing_wps > fastest_wpm_per_sec) {
            pacing_grade = 0;
        }
        else if (pacing_wps >= slowest_wpm_per_sec && pacing_wps <= perfect_wpm_per_sec) {
            pacing_grade = (pacing_wps - slowest_wpm_per_sec)/(perfect_wpm_per_sec - slowest_wpm_per_sec);
        }
        else if (pacing_wps > perfect_wpm_per_sec && pacing_wps <= fastest_wpm_per_sec) {
            pacing_grade =  1 - ((pacing_wps - perfect_wpm_per_sec)/(fastest_wpm_per_sec - perfect_wpm_per_sec));
        }
    }    
    
    public void CalculatePauseGrade() {
        int pause_count = FindObjectOfType<SpeechMetricsController>().GetPauses();
        float pauses_per_sec = (float)pause_count/(float)elapsed_time;
        float max_pauses_per_sec = (float)max_pauses_per_min/(float)60;

        // 0% = 5 of 5 seconds in 60 seconds
        // 100% = 0 of 5 second pauses in 60 seconds
        if (pauses_per_sec <= max_pauses_per_sec)
        {
            pauses_grade = 1 - (pauses_per_sec/max_pauses_per_sec);
        }
        else {
            pauses_grade = 0;
        }

    }    
    
    // Average of all grades
    public void CalculateQualityGrade() {
        CalculateFillerGrade();
        CalculateAudEngGrade();
        CalculatePacingGrade();
        CalculatePauseGrade();

        quality_grade = (filler_grade + audience_eng_grade + pacing_grade + pauses_grade)/4;
    }

    public float GetQuality() {
        CalculateQualityGrade();

        /*Debug.Log("filler_grade: " + filler_grade);
        Debug.Log("audience_eng_grade: " + audience_eng_grade);
        Debug.Log("pacing_grade: " + pacing_grade);
        Debug.Log("pauses_grade: " + pauses_grade);
        Debug.Log("quality_grade: " + quality_grade);
        */
        return quality_grade;
    }

    public void Reset() {
        filler_grade = 0;
        pacing_grade = 0;
        audience_eng_grade = 0;
        pauses_grade = 0;
        quality_grade = 0;
        elapsed_time = 0;
    }

}
