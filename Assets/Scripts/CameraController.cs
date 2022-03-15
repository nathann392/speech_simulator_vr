using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// Used code from coffeebreakcodes.com to control the camera using keyboard and mouse
/// as well as click on game objects
/// <see cref="http://coffeebreakcodes.com/mouse-click-on-game-object-unity3d-c/"/>
/// <see cref="http://coffeebreakcodes.com/move-zoom-and-rotate-camera-unity3d/"/>
///
public class CameraController : MonoBehaviour
{
    private float minX = -360.0f;
	private float maxX = 360.0f;
	
	private float minY = -45.0f;
	private float maxY = 45.0f;
 
	public float sensX = 100.0f;
	public float sensY = 100.0f;
	
	private float rotationY = 0.0f;
	private float rotationX = 0.0f;
    private float speed = 2.0f;

    // Update is called once per frame
    void Update()
    {
        // Include Unity editor navigation scripts if compiling for
		// the Unity editor
        #if UNITY_EDITOR
		
			// Send "OnPointerClick" message on mouse click (primary mouse button)
			if (Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)){
					//Debug.Log("Clicked on an object");
					hit.transform.gameObject?.SendMessage("OnPointerClick", null, SendMessageOptions.DontRequireReceiver);
				}
			}

			// Allow for camera position movement
			if (Input.GetKey(KeyCode.W)){
				transform.position += Vector3.right * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S)){
				transform.position += Vector3.left * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.A)){
				transform.position += Vector3.forward * speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D)){
				transform.position += Vector3.back * speed * Time.deltaTime;
			}

			// Allow for camera rotation
			if (Input.GetKey(KeyCode.LeftAlt)) {
				// Uses default sensitivity of 2
				float mouse = Input.GetAxis("Mouse Y");
				transform.Rotate(new Vector3(0,0,-mouse * 2));
			}

			// Allow for camera rotation movement using secondary mouse button
			if (Input.GetMouseButton (1)) {
				rotationX += Input.GetAxis ("Mouse X") * sensX;
				rotationY += Input.GetAxis ("Mouse Y") * sensY;
				rotationY = Mathf.Clamp (rotationY, minY, maxY);
				transform.localEulerAngles = new Vector3 (-rotationY , rotationX, 0);
			}
			
		#endif
    }
}
