using UnityEngine;
using System.Collections;

namespace ResultsNS
{
    public class ImportTest : MonoBehaviour, IUI
    {
        // Use this for initialization
        void Start()
        {
            ResultHistory history = ResultHistory.getFromSavedFile();
            Debug.Log(JsonUtility.ToJson(history));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick()
        {
            ResultHistory history = ResultHistory.getFromSavedFile();
            Debug.Log(JsonUtility.ToJson(history));
        }

    }
}
