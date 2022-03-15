using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForAudioCommand : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        float db = 20 * Mathf.Log10(Mathf.Abs(MicInput.MicLoudness));
        Debug.Log("Volume is " + MicInput.MicLoudness.ToString("##.#####") + ", decibels is " + db.ToString());
    }
}
