#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudienceController))]
public class AudienceControllerCustomEditor : Editor
{
    AudienceController AudienceControllerScript;
    [SerializeField] private string filePath = "Assets/Enums/";
    [SerializeField] private string fileName = "StudentPersonalityTypes";

    private void OnEnable()
    {
        AudienceControllerScript = (AudienceController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = false;
        filePath = EditorGUILayout.TextField("Path", filePath);
        fileName = EditorGUILayout.TextField("Name", fileName);
        GUI.enabled = true;
        if(GUILayout.Button("Save"))
        {
            AddEnumerator.WriteToEnum(filePath, fileName, AudienceControllerScript.PersonalityTypes);
        }
    }
}
#endif