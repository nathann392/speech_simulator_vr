using System;
using System.IO;
using UnityEngine;

namespace ResultsNS
{
    [Serializable]
    public class ResultHistory
    {
        public ResultRecord[] Items;
        public ResultHistory()
        {
        }

        public static ResultHistory getFromSavedFile()
        {
            string filepath = Path.Combine(Application.persistentDataPath, ExportSpeechResults.fileName + ".json");

            if (File.Exists(filepath))
            {
                using (StreamReader fileRead = new StreamReader(filepath))
                {
                    string content = fileRead.ReadToEnd();
                    return JsonUtility.FromJson<ResultHistory>(content);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
