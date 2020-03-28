using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveScore(HighScoreDisplay highScores)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData(highScores);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreData LoadScore(HighScoreDisplay highScores)
    {
        string path = Application.persistentDataPath + "/SaveData.bin";

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
