#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ResultsNS;

[CustomEditor(typeof(ExportSpeechResults))]
public class ExportSpeechResultCustomEditor : Editor
{
    ExportSpeechResults ExportScript;

    // Start is called before the first frame update
    void OnEnable()
    {
        ExportScript = (ExportSpeechResults)target;
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (GUILayout.Button("Save"))
        {
            AddExportFormat.WriteSpeechResultClass(ExportScript.fieldsToExport);
        }
    }
}
#endif