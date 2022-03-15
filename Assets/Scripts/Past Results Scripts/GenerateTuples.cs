// default imports
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// import used for Image object
using UnityEngine.UI;

public class GenerateTuples : MonoBehaviour {
	public GameObject PastResultsCanvas;
	public GameObject BreakdownCanvas;
	public GameObject Background;
	public GameObject Title;
	public GameObject Score;
	
	private const float TUPLE_WIDTH = 10.24F;
	private const float TUPLE_HEIGHT = 1.28F;
	
    // Start is called before the first frame update
    void Start() {
		int length = 8;
		// inserts tuples starting with the first element
		for (int i = 0; i < length; i++) {
			// random number between 0-100
			int randomScore = Random.Range(0, 100);
			string titleText = "Speech Results " + i;
			string scoreText = "Overall Score: " + randomScore;
			
			var scoreColor = Color.blue;
			if (randomScore > 69)
				scoreColor = Color.green;
			else if (randomScore > 49)
				scoreColor = Color.yellow;
			else
				scoreColor = Color.red;
			
			// define a new GameObject for new tuple
			GameObject tuple = new GameObject("Tuple (" + i + ")");
			// set defaults for location to be inside of whatever the script is attached to (Content)
			tuple.transform.SetParent (transform);
            tuple.transform.localScale = Vector3.one;
            tuple.transform.localRotation = Quaternion.Euler (Vector3.zero);
			// add RectTransform for shape
			tuple.AddComponent<RectTransform>();
			// position of tuple in its parent
            tuple.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -TUPLE_HEIGHT * 2 + TUPLE_HEIGHT * i, 0);
            tuple.GetComponent<RectTransform>().sizeDelta = new Vector2(TUPLE_WIDTH, TUPLE_HEIGHT);
			// add Image for color
			tuple.AddComponent<Image>();
			tuple.GetComponent<Image>().color = scoreColor;
			
			// add text to tuples
			// define a new GameObject for title
			GameObject title = new GameObject("TitleText");
			// set defaults for location to be inside of the tuple
			title.transform.SetParent (tuple.transform);
            title.transform.localScale = Vector3.one;
            title.transform.localRotation = Quaternion.Euler (Vector3.zero);
			// add RectTransform for shape
			title.AddComponent<RectTransform>();
			// position of title in its parent
            title.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-TUPLE_WIDTH / 2, TUPLE_HEIGHT / 2, 0);
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(TUPLE_WIDTH, TUPLE_HEIGHT);
            title.GetComponent<RectTransform>().localScale = new Vector3(0.003F, 0.003F, 0.003F);
			// add text
			title.AddComponent<Text>();
            title.GetComponent<Text>().text = titleText;
			title.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			title.GetComponent<Text>().fontSize = 150;
			title.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			title.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
			
			// define a new GameObject for score
			GameObject score = new GameObject("ScoreText");
			// set defaults for location to be inside of the tuple
			score.transform.SetParent (tuple.transform);
            score.transform.localScale = Vector3.one;
            score.transform.localRotation = Quaternion.Euler (Vector3.zero);
			// add RectTransform for shape
			score.AddComponent<RectTransform>();
			// position of score in its parent
            score.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-TUPLE_WIDTH / 2, 0, 0);
            score.GetComponent<RectTransform>().sizeDelta = new Vector2(TUPLE_WIDTH, TUPLE_HEIGHT);
            score.GetComponent<RectTransform>().localScale = new Vector3(0.0025F, 0.0025F, 0.0025F);
			// add text
			score.AddComponent<Text>();
            score.GetComponent<Text>().text = scoreText;
			score.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			score.GetComponent<Text>().fontSize = 150;
			score.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			score.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
			
			//TODO, create rect transform w/ image for color and box collider. add load breakdown script, passing in data, and button highlight script
			// define a new GameObject for breakdown button
			GameObject breakdownButton = new GameObject("BreakdownButton");
			// set defaults for location to be inside of the tuple
			breakdownButton.transform.SetParent (tuple.transform);
            breakdownButton.transform.localScale = Vector3.one;
            breakdownButton.transform.localRotation = Quaternion.Euler (Vector3.zero);
			// add RectTransform for shape
			breakdownButton.AddComponent<RectTransform>();
			// position of RectTransform in its parent
            breakdownButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(TUPLE_WIDTH / 3, 0, 0);
            breakdownButton.GetComponent<RectTransform>().sizeDelta = new Vector2(TUPLE_WIDTH / 3.5F, TUPLE_HEIGHT / 1.5F);
            breakdownButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			// add BoxCollider for raycasting
			breakdownButton.AddComponent<BoxCollider>();
			// position of BoxCollider in its parent
            breakdownButton.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
            breakdownButton.GetComponent<BoxCollider>().size = new Vector3(TUPLE_WIDTH / 3.5F, TUPLE_HEIGHT / 1.5F, 0.01F);
			// add Image for color
			breakdownButton.AddComponent<Image>();
			// set alpha of blue to half
			var blue = Color.blue;
			blue.a = 0.5F;
			breakdownButton.GetComponent<Image>().color = blue;
			// add button highlight script
			breakdownButton.AddComponent<ButtonHighlight>();
			// add load breakdown script
			breakdownButton.AddComponent<LoadBreakdown>();
			LoadBreakdown loadBreakdownScript = breakdownButton.GetComponent<LoadBreakdown>();
			// pass game objects to breakdown script
			loadBreakdownScript.PastResultsCanvas = PastResultsCanvas;
			loadBreakdownScript.BreakdownCanvas = BreakdownCanvas;
			loadBreakdownScript.Background = Background;
			loadBreakdownScript.BackgroundColor = scoreColor;
			loadBreakdownScript.TitleText = titleText;
			loadBreakdownScript.ScoreText = scoreText;
			loadBreakdownScript.Title = Title;
			loadBreakdownScript.Score = Score;
			
			// define a new GameObject for breakdown button text
			GameObject breakdownText = new GameObject("ScoreText");
			// set defaults for location to be inside of the breakdown button
			breakdownText.transform.SetParent (breakdownButton.transform);
            breakdownText.transform.localScale = Vector3.one;
            breakdownText.transform.localRotation = Quaternion.Euler (Vector3.zero);
			// add RectTransform for shape
			breakdownText.AddComponent<RectTransform>();
			// position of score in its parent
            breakdownText.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-(TUPLE_WIDTH / 3) / 2.5F, (TUPLE_HEIGHT / 1.5F) / 4, 0);
            breakdownText.GetComponent<RectTransform>().sizeDelta = new Vector2(TUPLE_WIDTH, TUPLE_HEIGHT);
            breakdownText.GetComponent<RectTransform>().localScale = new Vector3(0.0025F, 0.0025F, 0.0025F);
			// add text
			breakdownText.AddComponent<Text>();
            breakdownText.GetComponent<Text>().text = "Goto Breakdown";
			breakdownText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			breakdownText.GetComponent<Text>().fontSize = 150;
			breakdownText.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
			breakdownText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
		}
		var pos = transform.position;
		pos.y = (3 - length) * TUPLE_HEIGHT + TUPLE_HEIGHT / 2;
		transform.position = pos;
    }
}
