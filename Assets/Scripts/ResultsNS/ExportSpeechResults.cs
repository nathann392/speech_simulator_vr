using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace ResultsNS
{
    public class ExportSpeechResults : MonoBehaviour
    {
        [SerializeField] public List<GameObject> fieldsToExport = new List<GameObject>();
        private Transform ResultsPanel;

        //json string variables
        string prefix = "{";
        string content = "";
        string postfix = "}";

        string fieldName;
        string fieldValue;

        //Export Variables
        [SerializeField] public static string fileName = "results-history";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //Formats Result Data to Json for export then exports data
        public void exportResultData()
        {
            Debug.Log("Started Results Export");
            ResultsPanel = GameObject.Find("ResultsPanel").transform.Find("Canvas");
            //Create new Results Object
            //Cycle through Results Object FieldName[]  and assign ResultsPanel Values to Results Object FieldValue where FieldName[] equals ResultsPanel Component GUI text


            try
            {
                var culture = new CultureInfo("en-US");
                content = "\"speechDate\":\"" + System.DateTime.Now.ToString(culture) + "\",";
                content += "\"audioFilePath\":\"" + MicrophoneService.Instance.GetLatestSavedFileName() + "\",";

                foreach (GameObject currentField in fieldsToExport)
                {
                    //Debug.Log(currentField);
                    fieldName = ResultsPanel.Find(currentField.name).GetChild(0).name;
                    fieldValue = ResultsPanel.Find(currentField.name).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;

                    content = content + "\"" + fieldName + "\":\"" + fieldValue + "\",";


                    //Debug.Log(content);
                }

                content = content.Trim(',');

                Debug.Log(prefix + content + postfix);

                jsonToFile(prefix + content + postfix);
            }
            catch (System.Exception ex)
            {
                Debug.Log("Error in Json Export: " + ex.ToString());
            }

            Debug.Log("Finished Results Export");
        }

        //Exports Json Data to file.
        private void jsonToFile(string jSONExport)
        {
            Debug.Log("Starting File Write");

            string currentFileContent = "";
            string filepath = Path.Combine(Application.persistentDataPath, fileName + ".json");

            try
            {
                if (File.Exists(filepath))
                {
                    using (StreamReader fileRead = new StreamReader(filepath))
                    {
                        currentFileContent = fileRead.ReadToEnd();
                    }
                }
                else
                {
                    using (File.Create(filepath)) ;
                }

                using (StreamWriter fileWrite = new StreamWriter(filepath))
                {
                    if (!(currentFileContent.Trim(' ').Equals("")))
                    {
                        fileWrite.WriteLine(currentFileContent.Insert(currentFileContent.IndexOf("]"), "," + jSONExport));
                    }
                    else
                    {
                        fileWrite.WriteLine("{\"Items\": [" + jSONExport + "]}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("Error in File Writing: " + ex.ToString());
            }

            Debug.Log("Finished File Write");
        }
    }

    public class SpeechResultSet
    {
        System.DateTime dateOfSpeech;
        string fieldName = "";
        string fieldValue = "";
    }
}