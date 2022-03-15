using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioCheckboxController : MonoBehaviour
{
    public GameObject AudioCheckbox;
    public GameObject Checkmark;
    private bool checked_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        Checkmark.gameObject.transform.localScale = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        AudioCheckbox.GetComponent<Image>().color = new Color32(200,200,200,255);

    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        AudioCheckbox.GetComponent<Image>().color = new Color32(255,255,255,255);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if (checked_flag) {
            Checkmark.gameObject.transform.localScale = new Vector3(0,0,0);
            checked_flag = false;
        }
        else {
            Checkmark.gameObject.transform.localScale = new Vector3(1,1,1);
            checked_flag = true;
        }

    }

    public bool GetCheckbox() 
    {
        return checked_flag;
    }
}
