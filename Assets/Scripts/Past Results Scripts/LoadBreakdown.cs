using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBreakdown : MonoBehaviour
{
	public GameObject PastResultsCanvas;
	public GameObject BreakdownCanvas;
	public GameObject Background;
	public GameObject Title;
	public GameObject Score;
	public Color BackgroundColor;
	public string TitleText;
	public string ScoreText;
	
    // Selects object by tapping on screen while ray cast is hitting object
    public void OnPointerClick() {
		if (PastResultsCanvas.activeSelf) {
			Background.GetComponent<Image>().color = BackgroundColor;
			Title.GetComponent<Text>().text = TitleText;
			Score.GetComponent<Text>().text = ScoreText;
			PastResultsCanvas.SetActive(false);
			BreakdownCanvas.SetActive(true);
		} else {
			PastResultsCanvas.SetActive(true);
			BreakdownCanvas.SetActive(false);
		}
    }
}
