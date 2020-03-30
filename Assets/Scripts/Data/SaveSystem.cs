using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using HighScoreAndNameStruct;

public class SaveSystem
{
    public static void SaveScore()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "SaveData.bin");
        FileStream stream = new FileStream(path, FileMode.Create);

        List<HighScoreAndName> highScoresList = new List<HighScoreAndName>();

        for (int i = 0; i < HighScoreDisplay.highScores.Length; i++)
        {
            highScoresList.Add(HighScoreDisplay.highScores[i]);
        }

        while (highScoresList.Count < 10)
        {
            highScoresList.Add(new HighScoreAndName(0, "N/A"));
        }

        ScoreData data = new ScoreData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScore()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveData.bin");

        if (!File.Exists(path))
        {
            SaveScore();
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        ScoreData data = formatter.Deserialize(stream) as ScoreData;
        data.ToString();
        stream.Close();

        return data;
    }
}
