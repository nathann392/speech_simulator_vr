#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * This script adds custom functionality regarding dynamically written Enumerations
 * The script is written using (https://answers.unity.com/questions/1414634/can-i-add-an-enum-value-in-the-inspector.html) as reference
 * 
 * Designed and altered with BBKStudent Personalities in mind
 * 
 * To implement additional Enumerations
 * 
 * */

public class AddEnumerator : Editor
{
    const string extension = ".cs";
    
    public static void WriteToEnum<T>(string path, string name, ICollection<T> data)
    {
        
        //Will Overwrite Existing File
        using (StreamWriter file = File.CreateText(path + name + extension))
        {
            file.WriteLine("public enum " + name + " \n{");

            int i = 0;
            foreach (var line in data)
            {
                string lineRep = line.ToString().Replace(" ", string.Empty);
                if(!string.IsNullOrEmpty(lineRep))
                {
                    file.WriteLine(string.Format("\t{0} = {1},", lineRep, i));
                    i++;
                }
            }

            file.WriteLine("\n}");
        }

        AssetDatabase.ImportAsset(path + name + extension);
    }
}
#endif