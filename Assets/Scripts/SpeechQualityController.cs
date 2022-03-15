using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class SpeechQualityController : MonoBehaviour
{
	private int totalHesitations = 0;
	private int tenSecHesitations = 0;
    private int speechQuality = 0;
	private float timeCounter = 0.0f;
	private float timeStamp = 0.0f;
	
	void Update() {
		timeCounter += Time.deltaTime;
		
		// Every ten seconds calculate the hesitations in the last ten seconds
		if (timeCounter - timeStamp >= 10.0f) {
			CalculateHesitation();
		}
	}
	
	private void CalculateSpeechQuality() {
		// 100-80 = Excellent, 80-60 = Satisfactory, 60-0 Needs Improvement
		speechQuality = 0;

	}

	public void CalculateHesitation() {
		// get new hesitations in temp var for calculations
		int tempHesitations = 0;
		tempHesitations = FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTotalHesitations();

		// calculate new hesitations from past 10 seconds
		tenSecHesitations = tempHesitations - totalHesitations;

		// update total hesitations
		totalHesitations = tempHesitations;
		timeStamp = timeCounter;
	}

	public string GetRepetitiveWords() {
        string transcript = FindObjectOfType<entreprenevr.StreamingSpeechToText>().GetTranscript();
		transcript = Regex.Replace(transcript, @"\((.*?)\)", "");

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

	public int GetSpeechQuality() {
		CalculateSpeechQuality();
		return speechQuality;
	}


	public void Reset() {
		totalHesitations = 0;
	 	tenSecHesitations = 0;
    	speechQuality = 0;
		timeCounter = 0.0f;
	 	timeStamp = 0.0f;

		FindObjectOfType<entreprenevr.StreamingSpeechToText>().ResetResultsValues();
		FindObjectOfType<HeadController>().Reset();
	}
}
