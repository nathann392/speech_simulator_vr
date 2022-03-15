/**
 * This controller manages the appearance of the reticle.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GlobalSettings;

public class ReticleController : MonoBehaviour
{
    public GameObject reticle;
    public GameObject crosshairs;
    public GameObject finger;

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalSettings.pointer == "reticle") reticle.SetActive(true);
        else if (GlobalSettings.pointer == "crosshairs") crosshairs.SetActive(true);
        else if (GlobalSettings.pointer == "finger") finger.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
