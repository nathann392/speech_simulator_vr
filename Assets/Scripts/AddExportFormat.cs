#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AddExportFormat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public static void WriteSpeechResultClass(List<GameObject> fieldsToExport)
    {
        string fieldName = "";
        string line = "";

        Transform ResultsPanel = GameObject.Find("ResultsPanel").transform.Find("Canvas");

        //Debug.Log("Started Writing Class");

        using (StreamWriter file = File.CreateText("Assets/Scripts/SpeechResult.cs"))
        {
            file.WriteLine("using System.IO;");
            file.WriteLine("using UnityEngine;");

            file.WriteLine("public class SpeechResult \n{");
            file.WriteLine("string speechDate;");
            file.WriteLine("string audioFilePath;");

            foreach (GameObject currentField in fieldsToExport)
            {
 
                fieldName = ResultsPanel.Find(currentField.name).GetChild(0).name;
                
                line = "string " + fieldName + ";";
                
                file.WriteLine(line);

                fieldName = "";
                line = "";
            }

            file.WriteLine("}");
        }

        AssetDatabase.ImportAsset("Assets/Scripts/SpeechResult.cs");

        //Debug.Log("Finished Writing Class");
    }
}
#endif