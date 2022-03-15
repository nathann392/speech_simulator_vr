using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    // Highlights object when ray cast pointer hits selectable object
    public void OnPointerEnter()
    {
        //Debug.Log("enter " + transform.ToString());
		Image image = GetComponent<Image>();
		Color newColor = image.color;
		newColor.a = 1;
		image.color = newColor;
    }

    // Returns object to normal when ray cast pointer stops hitting object
    public void OnPointerExit()
    {
        //Debug.Log("exit " + transform.ToString());
		Image image = GetComponent<Image>();
		Color newColor = image.color;
		newColor.a = 0.5F;
		image.color = newColor;
    }
}
