using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadController : MonoBehaviour
{
	Camera mainCamera;

	[Tooltip("Warning message text")]
	public Text displayMessage;
	public GameObject audienceHitBox;
 	private static int badHeadMovementCount = 0;
	private static float fidgetLevel = 0.0f;
	private string msg = "";
	private float flagTimer = 0.0f;
	private float timeElapsed = 0.0f;
	private float lastKnownX = 0.0f;
	private float lastKnownY = 0.0f;
	private float totalMoved = 0.0f;
	
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
	

    // Update is called once per frame
    void Update()
    {
		RaycastHit hit;
		var cameraCenter = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, GetComponent<Camera>().nearClipPlane));
		if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 1000)) {
			var obj = hit.transform.gameObject;
			if (obj == audienceHitBox) {
				// if user hasn't looked at audience for 3 or more seconds, increment bad head movement appropriately
				if (flagTimer >= 3.0f) {
					badHeadMovementCount += (int) (flagTimer / 3);
				}
				flagTimer = 0.0f;
				msg = "";
			} else {
				// time amount of time not looking at audience
				flagTimer += Time.deltaTime;
				//Debug.Log(flagTimer);
				msg = "Look at the audience!";
			}
		} else {
			// time amount of time not looking at audience
			flagTimer += Time.deltaTime;
			//Debug.Log(flagTimer);
			msg = "Look at the audience!";
		}
		
		setDisplayMessage(msg);
		
		// Calculate where the player is currently looking
		float x = mainCamera.transform.localEulerAngles.x;
		float y = mainCamera.transform.localEulerAngles.y;
		
		// localEulerAngles returns values from 0-360, but we need values from 0-180, so we do this to fix it
		if (x > 180) {
			x = 360 - x;
		}
		if (y > 180) {
			y = 360 - y;
		}
		
		// Calculate the distances moved by the player
		float xMoved = x - lastKnownX;
		float yMoved = y - lastKnownY;
		float moved = Mathf.Sqrt((xMoved * xMoved) + (yMoved * yMoved));
		
		// Adds up the distance moved between frames
		totalMoved += Mathf.Abs(moved);
		
		// Update saved values
		lastKnownX = x;
		lastKnownY = y;
		
		// Keep track of passed time
		timeElapsed += Time.deltaTime;
		
		// Currently the total distance moved divided by elapsed time
		fidgetLevel = totalMoved / timeElapsed;
		//Debug.Log(fidgetLevel);
		
		// Track instances of peak fidget levels
		// slope of fidgetLevel indicates fidgeting?
		
		// ideas for improvement: detect high amounts of movement in a short timeframe
    }

	void setDisplayMessage(string message) {
		if (this.displayMessage) {
			this.displayMessage.text = message;
		}
	}

	public int GetBadHeadMovementCount() {
		return badHeadMovementCount;
	}
	
	public float GetFidgetLevel() {
		return fidgetLevel;
	}

	// Reset reused values
	public void Reset() {
		flagTimer = 0.0f;
		badHeadMovementCount = 0;
		fidgetLevel = 0.0f;
		timeElapsed = 0.0f;
		lastKnownX = 0.0f;
		lastKnownY = 0.0f;
		totalMoved = 0.0f;
	}
}
