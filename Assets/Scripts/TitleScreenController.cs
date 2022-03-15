/**
 * This controller is used for managing the behavior of selectable objects
 * in the title screen scene.
 * 
 * @author David Bieganski
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using static GlobalSettings;

public class TitleScreenController : MonoBehaviour
{
    public Material normalMaterial;
    public Material highlightedMaterial;
    public GameObject startButton;
    public GameObject optionsButton;
	public GameObject pastButton;
    public GameObject quitButton;
    public GameObject optionsLabel;
    public GameObject backButton;
    public GameObject pointerLabel;
    public GameObject reticleButton;
    public GameObject fingerButton;
    public GameObject noneButton;
    public GameObject reticle;
    public GameObject crosshairs;
    public GameObject finger;
    public GameObject titleLabel;

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
            case "StartButton (UnityEngine.MeshRenderer)":
                LoadScene("MainClassroom");
                break;
			case "PastButton (UnityEngine.MeshRenderer)":
                LoadScene("PhillipPastResults");
                break;
            case "OptionsButton (UnityEngine.MeshRenderer)":
                titleLabel.SetActive(false);
                startButton.SetActive(false);
                optionsButton.SetActive(false);
				pastButton.SetActive(false);
                quitButton.SetActive(false);
                optionsLabel.SetActive(true);
                pointerLabel.SetActive(true);
                reticleButton.SetActive(true);
                fingerButton.SetActive(true);
                noneButton.SetActive(true);
                backButton.SetActive(true);
                break;
            case "QuitButton (UnityEngine.MeshRenderer)":
                Debug.Log("Quitting...");
                Application.Quit();
                break;
            case "ReticleButton (UnityEngine.MeshRenderer)":
                GlobalSettings.pointer = "reticle";
                reticle.SetActive(true);
                crosshairs.SetActive(false);
                finger.SetActive(false);
                break;
            case "FingerButton (UnityEngine.MeshRenderer)":
                GlobalSettings.pointer = "finger";
                reticle.SetActive(false);
                crosshairs.SetActive(false);
                finger.SetActive(true);
                break;
            case "NoneButton (UnityEngine.MeshRenderer)":
                GlobalSettings.pointer = "none";
                reticle.SetActive(false);
                crosshairs.SetActive(false);
                finger.SetActive(false);
                break;
            case "BackButton (UnityEngine.MeshRenderer)":
                titleLabel.SetActive(true);
                startButton.SetActive(true);
                optionsButton.SetActive(true);
				pastButton.SetActive(true);
                quitButton.SetActive(true);
                optionsLabel.SetActive(false);
                pointerLabel.SetActive(false);
                reticleButton.SetActive(false);
                fingerButton.SetActive(false);
                noneButton.SetActive(false);
                backButton.SetActive(false);
                break;
        }
    }

    // Enters the classroom scene
    public void LoadScene(string value)
    {
        SceneManager.LoadScene(sceneName:value);
    }

    // Quits the app
    public void QuitSimulator()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
