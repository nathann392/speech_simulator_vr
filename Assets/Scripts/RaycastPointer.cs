/**
 * These functions are used to actuate mouse click behavior on game objects
 * using the ray cast feature.
 * 
 * @author David Bieganski 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPointer : MonoBehaviour
{

    public float maxDistance;

    private GameObject gObject = null;

    private float rotationSpeed = 15.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; // Capture game object with ray cast
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // Game object captured by ray cast
            if (gObject != hit.transform.gameObject)
            {
                gObject?.SendMessage("OnPointerExit");
                gObject = hit.transform.gameObject;
                gObject?.SendMessage("OnPointerEnter");
            }
        }
        else
        {
            // No game object captured
            gObject?.SendMessage("OnPointerExit");
            gObject = null;
        }

        // Selects captured game object if screen is tapped
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            gObject?.SendMessage("OnPointerClick");
            gObject = null;
        }

        // Move camera with mouse when using Unity player
        if (Input.GetMouseButton(1)) // Move camera when right button is pressed
        {
            MoveCamera();
        }

        // Selects captured game object if left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            gObject?.SendMessage("OnPointerClick");
            gObject = null;
        }
    }

    // Moves camera with mouse
    private void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * rotationSpeed;
        rotationX += -mouseY * rotationSpeed;

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
    }
}
