/**
 * This controller is used for managaging the behavior of selectable objects
 * in the classroom scene.
 * 
 * @author David Bieganski
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassroomController : MonoBehaviour
{
    public Material normalMaterial;
    public Material highlightedMaterial;
    public GameObject door;

    private Renderer component;

    // Start is called before the first frame update
    void Start()
    {
        component = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Highlights object when ray cast pointer hits selectable object
    public void OnPointerEnter()
    {
        component.material = highlightedMaterial;
    }

    // Returns object to normal when ray cast pointer stops hitting object
    public void OnPointerExit()
    {
        component.material = normalMaterial;
    }

    // Selects object by tapping on screen while ray cast is hitting object
    public void OnPointerClick()
    {
        string componentName = component.ToString();
        switch (componentName)
        {
            case "BackButton (UnityEngine.MeshRenderer)":
                ExitClassroom();
                break;
            case "ExitDoor (UnityEngine.MeshRenderer)":
                ExitClassroom();
                break;
        }
    }

    // Exits the classroom scene
    public void ExitClassroom()
    {
        SceneManager.LoadScene(0);
    }
}
