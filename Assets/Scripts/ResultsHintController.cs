using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsHintController : MonoBehaviour
{
    public GameObject hint;

    // Start is called before the first frame update
    void Start()
    {
        hint.gameObject.transform.localScale = new Vector3(0,0,0);
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
        hint.gameObject.transform.localScale = new Vector3(1,1,1);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        hint.gameObject.transform.localScale = new Vector3(0,0,0);
    }
}
