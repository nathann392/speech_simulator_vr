using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	public string scene;
	
	// Highlights object when ray cast pointer hits selectable object
    public void OnPointerEnter()
    {
        //Debug.Log("enter " + transform.ToString());
    }

    // Returns object to normal when ray cast pointer stops hitting object
    public void OnPointerExit()
    {
        //Debug.Log("exit " + transform.ToString());
    }

    // Selects object by tapping on screen while ray cast is hitting object
    public void OnPointerClick() {
        SceneManager.LoadScene(sceneName:scene);
    }
}
