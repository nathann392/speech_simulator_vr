using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject simMenu;
    public GameObject simLevelsMenu;

    // Start is called before the first frame update
    public void Start()
    {
        mainMenu = transform.parent.parent.Find("MainMenu").gameObject;
        simMenu = transform.parent.parent.Find("SimMenu").gameObject;
        simLevelsMenu = transform.parent.parent.Find("SimLevelsMenu").gameObject;
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {

    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {

    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if (gameObject.name == "PlayButton") {
            mainMenu.SetActive(false);
            simMenu.SetActive(true);
        }
        else if (gameObject.name == "QuitButton") {
            Application.Quit();
        }
        else if (gameObject.name == "SpeakingSimButton") {
            simMenu.SetActive(false);
            simLevelsMenu.SetActive(true);
        }
        else if (gameObject.name == "SimBackButton") {
            mainMenu.SetActive(true);
            simMenu.SetActive(false);
        }
        else if (gameObject.name == "ClassroomLevelButton") {
            SceneManager.LoadScene("MainClassroom");
        }
        else if (gameObject.name == "LevelsBackButton") {
            simMenu.SetActive(true);
            simLevelsMenu.SetActive(false);
        }

        mainMenu = transform.parent.parent.Find("MainMenu").gameObject;
        simMenu = transform.parent.parent.Find("SimMenu").gameObject;
        simLevelsMenu = transform.parent.parent.Find("SimLevelsMenu").gameObject;
    }

}
