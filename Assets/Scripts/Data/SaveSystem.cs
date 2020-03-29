using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using HighScoreAndNameStruct;

public class SaveSystem
{
    public static void SaveScore(HighScoreDisplay highScores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "SaveData.bin");
        FileStream stream = new FileStream(path, FileMode.Create);

        List<HighScoreAndName> highScoresList = new List<HighScoreAndName>();

        for (int i = 0; i < highScores.highScores.Length; i++)
        {
            highScoresList.Add(highScores.highScores[i]);
        }

        while (highScoresList.Count < 10)
        {
            highScoresList.Add(new HighScoreAndName(0, "N/A"));
        }

        highScores = new HighScoreDisplay(highScoresList.ToArray());

        ScoreData data = new ScoreData(highScores);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScore(HighScoreDisplay highScores)
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveData.bin");

        if (!File.Exists(path))
        {
            SaveScore(highScores);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        ScoreData data = formatter.Deserialize(stream) as ScoreData;
        stream.Close();

        return data;
    }
}
