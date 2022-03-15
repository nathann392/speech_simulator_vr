#pragma warning disable 0649

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using System.Text.RegularExpressions;
using TranscriptNS;

namespace entreprenevr
{
    public class StreamingSpeechToText : TranscriptGenerator, IMicrophoneObserver
    {
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        
        [Space(10)]
        [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
        [SerializeField]
        private string _serviceUrl;
        
        [Tooltip("Text field to display the results of streaming.")]
        public TMPro.TextMeshPro ResultsField;
        public bool view_text_flag;

        [Header("IAM Authentication")]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string _iamApikey;

        [Header("Parameters")]
        // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
        [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
        [SerializeField]
        private string _recognizeModel;
        private int hesitationCount = 0;
        public bool DEBUG = false;

        #endregion

        private SpeechToTextService _service;
        private MeshRenderer mesh;

		private int totalHesitations = 0;
        private int totalWords = 0;
        private string transcript = "";
        private float timer = 0.0f;
        private double elapsed_time = 0;
        private bool time_flag = false;
        private int pauses = 0;
        private string hesitationWord = "%HESITATION";

        void Start()
        {   
            LogSystem.InstallDefaultReactors();
            mesh = GetComponent<MeshRenderer>();
            Debug.Log("Starting Streaming Speech to Text");
        }

        void Update()
        {
            if (time_flag) {
                timer += Time.deltaTime;
                elapsed_time = (double)(timer % 60);
            }
        }

        public void DoSpeechToText()
        {
            mesh.material.color = Color.green;
            Runnable.Run(CreateService());
        }

        public void StopSpeechToText() 
        {
            StopRecording();
            mesh.material.color = Color.red;
            Active = false;
            Transcript transcript = base.endTranscript();
        }

        void OnGUI()
        {
            if (DEBUG) 
            {
                if (GUI.Button(new Rect(10, 230, 150, 100), "Start Speech-To-Text"))
                {
                    DoSpeechToText();
                }
                if (GUI.Button(new Rect(170, 230, 150, 100), "Stop Speech-To-Text"))
                {
                    StopSpeechToText();
                }
            }
        }

        private IEnumerator CreateService()
        {
            if (string.IsNullOrEmpty(_iamApikey))
            {
                throw new IBMException("Please provide IAM ApiKey for the service.");
            }

            IamAuthenticator authenticator = new IamAuthenticator(apikey: _iamApikey);

            //  Wait for tokendata
            while (!authenticator.CanAuthenticate())
                yield return null;

            _service = new SpeechToTextService(authenticator);
            if (!string.IsNullOrEmpty(_serviceUrl))
            {
                _service.SetServiceUrl(_serviceUrl);
            }
            _service.StreamMultipart = true;

            Active = true;
            StartRecording();
        }

        public bool Active
        {
            get { return _service.IsListening; }
            set
            {
                if (value && !_service.IsListening)
                {
                    _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                    _service.DetectSilence = true;
                    _service.EnableWordConfidence = true;
                    _service.EnableTimestamps = true;
                    _service.SilenceThreshold = 0.01f;
                    _service.MaxAlternatives = 1;
                    _service.EnableInterimResults = true;
                    _service.OnError = OnError;
                    _service.InactivityTimeout = -1;
                    _service.ProfanityFilter = false; // TODO: May want to enable this
                    _service.SmartFormatting = true;
                    _service.SpeakerLabels = false;
                    _service.WordAlternativesThreshold = null;
                    _service.EndOfPhraseSilenceTime = null;
                    _service.StartListening(OnRecognize, OnRecognizeSpeaker);
                }
                else if (!value && _service.IsListening)
                {
                    _service.StopListening();
                }
            }
        }

        private void StartRecording()
        {
            UnityObjectUtil.StartDestroyQueue();
            
            // Listen for new audio segments from MicrophoneService
            MicrophoneService.Instance.ListenForAudioSegments(this);
        }

        private void StopRecording()
        {
            // Stop listening for new audio segments from MicrophoneService
            MicrophoneService.Instance.StopListeningForAudioSegments(this);
        }

        private void OnError(string error)
        {
            Active = false;

            Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
        }

        ///
        /// <description>This IEnumerator will grab chunks of audio from the MicrophoneService
        /// to send to IBM Watson for speech-to-text.</description>
        ///
        public void ReceiveAudioSegment(AudioClip segment)
        {
            // Get latest audio samples
            float[] samples = new float[segment.samples];
            segment.GetData(samples, 0);


            // Prepare latest samples to be sent to IBM Watson
            AudioData record = new AudioData();
            record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
            record.Clip = segment;

            // Send to IBM Watson
            _service.OnListen(record);

        }

        private void OnRecognize(SpeechRecognitionEvent result)
        {

            if (result != null && result.results.Length > 0)
            {
                foreach (var res in result.results)
                {

                    if (res.final)
                    {
                        //processesAFinalResult(res.final);
                        double max = 0;
                        SpeechRecognitionAlternative strongestAlternative = null;
                        foreach (var alt in res.alternatives) {
                            if (alt.confidence > max) {
                                max = alt.confidence;
                                strongestAlternative = alt;
                            }
                        }

                        base.processText(strongestAlternative.transcript);
                    }

                    foreach (var alt in res.alternatives)
                    {
                        string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                        Log.Debug("ExampleStreaming.OnRecognize()", text);
                        SetResultsFieldText(text);

                        ProcessSentenceStart(res.final);

                        // Look for final hesitations
                        if (alt.transcript.Contains(hesitationWord) && !res.final) {
                            Debug.Log("Found one!");
                            hesitationCount = Regex.Matches(alt.transcript, hesitationWord).Count;
                        }

                        ProcessSentenceEnd(res.final, text);

                    }

                    if (res.keywords_result != null && res.keywords_result.keyword != null)
                    {
                        foreach (var keyword in res.keywords_result.keyword)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                        }
                    }

                    if (res.word_alternatives != null)
                    {
                        foreach (var wordAlternative in res.word_alternatives)
                        {
                            Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                            foreach (var alternative in wordAlternative.alternatives)
                                Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                        }
                    }
                }
            }
        }

        private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
        {
            if (result != null)
            {
                foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
                {
                    Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
                }
            }
        }
		
        public void SetResultsFieldText(string text) {
            if (view_text_flag) {
                ResultsField.text = text;
            }
        }

		public int GetTotalHesitations()
		{
			return totalHesitations;
		}

        public int GetTotalWords() 
        {
            return totalWords;
        }

        public int GetPauses() 
        {
            return pauses;
        }

        public string GetTranscript()
        {
            return transcript;
        }

        public void CalculateWordCount(string text) {
            for(int i = 0; i < text.Length-1; i++) {  
                if(text[i] == ' ' && Char.IsLetter(text[i+1]) && (i > 0)) {  
                        totalWords++;  
                }  
            }  
            totalWords++; 
        }

        public void ProcessSentenceStart(bool sentence_end) {
            if (!sentence_end) {
                time_flag = false;
            }
        }

        public void ProcessSentenceEnd(bool sentence_end, string text) {
            if (sentence_end) {
                // Hesitations
                totalHesitations += hesitationCount; 
                hesitationCount = 0;

                CalculateWordCount(text);

                // Transcript
                transcript += text;
                
                // Start time at sentence end for pause time
                time_flag = true;
                timer = 0;

                if (elapsed_time > 5) {
                    pauses++;
                }
            }
        }

        public void ResetResultsValues() 
        {
            pauses = 0;
            elapsed_time = 0;
            time_flag = false;
            totalHesitations = 0;
            totalWords = 0;
            transcript = "";
        }
    }
}
