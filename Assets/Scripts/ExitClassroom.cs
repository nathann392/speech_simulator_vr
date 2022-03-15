using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitClassroom : MonoBehaviour
{
    void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(1);
    }
}
