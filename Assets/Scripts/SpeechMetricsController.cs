using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class SpeechMetricsController : MonoBehaviour
{    
    private const int SPEECH_END_DELAY = 3;

    private float timer = 0.0f;
    private int elapsed_time = 0;
    private bool time_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Count time for pacing
         if (time_flag) {
            timer += Time.deltaTime;
            elapsed_time = (int)(timer % 60);
        }		
    }

    public void StartSpeech() {
        ResetSpeechValues();

        // Start time at speech start
        time_flag = true;
        timer = 0;
    }

    public void EndSpeech() {

        // Stop time at speech end
        time_flag = false;
        elapsed_time -= SPEECH_END_DELAY;
    }

    public void ResetSpeechValues() {
		FindObjectOfType<entreprenevr.StreamingSpeechToText>().ResetResultsValues();
		FindObjectOfType<HeadController>().Reset();
        FindObjectOfType<SpeechGradeCalculator>().Reset();

        timer = 0;
        elapsed_time = 0;
    }

    public float CalculatePacing() {
        int word_count = FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTotalWords();
        return (float)word_count/(float)elapsed_time;
    }

    public string CalculateRepetitiveWords() {
        string transcript = FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTranscript();
		transcript = Regex.Replace(transcript, @"\((.*?)\)", "");
        Debug.Log("transcript: " + transcript);

		string[] words = transcript.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

		// Turn string into a dictionary
		Dictionary<string, int> dict = words
			.GroupBy(word => word)
			.ToDictionary(
				kvp => kvp.Key, // the word itself is the key
				kvp => kvp.Count()); // number of occurences is the value

		// Retrieving all duplicate words
		string[] duplicates = dict
			.Where(kvp => kvp.Value > 1)
			.Select(kvp => kvp.Key)
			.ToArray();

		// Counting all duplicates and formatting it into a list in the desired output format
		string output = String.Join(
			", ", 
			dict
				.Where(kvp => kvp.Value > 3)
				.Select(kvp => 
					String.Format(
						"\"{0}\" x {1}", 
						kvp.Key, 
						kvp.Value))
		);

        return output;
    }

    public int GetSpeechLengthSeconds() {
        return elapsed_time;
    }

    public int GetFiller() {
        return FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTotalHesitations();
    }

    public float GetPacingWps() {
        return CalculatePacing();
    }

    public int GetAudienceEngagement() {
        return FindObjectOfType<HeadController>().GetBadHeadMovementCount();
    }

    public int GetPauses() {
        return FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetPauses();
    }

    public int GetTotalWords() {
        return FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTotalWords();
    }

    public string GetRepetitiveWords() {
		return CalculateRepetitiveWords();
    }

    public double GetOverallPerformance() {
        return FindObjectOfType<SpeechGradeCalculator>().GetQuality();
    }
}
