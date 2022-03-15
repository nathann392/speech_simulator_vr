using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;

namespace TranscriptNS {
  public class TranscriptGenerator : MonoBehaviour
  {

    private Transcript transcript;
    private bool finished = false;

  
    public TranscriptGenerator() {
      this.transcript = new Transcript();
    }

    public void processText(string text) {
      if (!this.finished) {
        this.transcript.append(text);
      }
    }

    public Transcript endTranscript() {
      this.finished = true;
      Debug.Log("Full transcript: " + this.transcript.getFullText());
      return this.transcript;
    }

    public void resetTranscript() {
      this.finished = false;
      this.transcript = new Transcript();
    }

    public bool isTranscriptGeneratorFinished() {
      return this.finished;
    }

    public string getCurrentTranscriptFullText() {
      return this.transcript.getFullText();
    }
  }
}