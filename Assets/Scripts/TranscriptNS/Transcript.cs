using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TranscriptNS {
  public class Transcript
  {

    private string fullText;
    private DateTime startDateTime;
    private DateTime endDateTime;

    public const string HESITATION_WORD = "%HESITATION";
    public const string DATE_TIME_FORMAT = "yyyy-MM-dd-HHmmss";

    public Transcript() {
      this.fullText = "";
      this.startDateTime = DateTime.Now;
    }

    public string getFullText() {
      return this.fullText;
    }

    public DateTime getStartDateTime() {
      return this.startDateTime;
    }

    public string getStartDateTimeString() {
      return this.startDateTime.ToString(DATE_TIME_FORMAT);
    }

    public DateTime getEndDateTime() {
      return this.endDateTime;
    }

    public string getEndDateTimeString() {
      return this.endDateTime.ToString(DATE_TIME_FORMAT);
    }

  ///
  /// Get the count of every word in the full text.
  /// See the reference for an explanation of the regex pattern to watch words.
  /// <see cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#NonWhitespaceCharacter" />
    public int getWordCount() {
      string pattern = @"\b(\S+)\s?";
      MatchCollection matches = Regex.Matches(this.fullText, pattern);
      return matches.Count;
    }

    public void append(string text) {
      this.fullText += text;
    }

    public void stopTranscript() {
      this.endDateTime = DateTime.Now;
    }

    public int getHesitationCount() {
      Regex pattern = new Regex(HESITATION_WORD);
      MatchCollection matches = pattern.Matches(this.fullText);
      return matches.Count;
    }

    public override string ToString() {
      return this.fullText;
    } 
  }
}