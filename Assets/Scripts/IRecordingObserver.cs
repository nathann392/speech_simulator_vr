using UnityEngine;

///
/// <description>Interface for objects needing to be notified
/// of new audio segments.</description>
///
public interface IMicrophoneObserver {
  void ReceiveAudioSegment(AudioClip segment);
}