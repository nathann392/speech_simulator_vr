using System;
using System.Collections.Generic;
using UnityEngine;

namespace TranscriptNS {
  public class Store : MonoBehaviour
  {
    private List<Transcript> transcriptStore;

    public Transcript createTranscript()
    {
      Transcript transcript = new Transcript();
      transcriptStore.Add(transcript);
      return transcript;
    }

    public Transcript getLatest()
    {
      return transcriptStore.Count > 0 ? transcriptStore[transcriptStore.Count - 1] : null;
    }
  }
}