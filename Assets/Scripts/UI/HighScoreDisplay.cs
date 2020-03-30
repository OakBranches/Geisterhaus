using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HighScoreAndNameStruct;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    ScoreManager score;
    public static HighScoreAndName[] highScores = null;
    Text[] scoreText = null;
    bool loaded = false;

    public HighScoreDisplay(HighScoreAndName[] scoreAndName)
    {
        highScores = new HighScoreAndName[10];
        for (int i = 0; i < 10; i++)
        {
            highScores[i] = scoreAndName[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        loaded = false;
        scoreText = gameObject.GetComponentsInChildren<Text>();
        score = FindObjectOfType<ScoreManager>();
        print(Application.persistentDataPath);
        SaveScores();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu" && !loaded)
        {
            loaded = true;
            UpdateDisplay();
        }    
    }

    public void UpdateDisplay()
    {
        ScoreData data = SaveSystem.LoadScore();
        highScores = new HighScoreAndName[10];

        for (int i = 0; i < 10; i++)
        {
            highScores[i] = data.scoreAndName[i];
            scoreText[i].text = i + 1 + ". " + highScores[i].name + " " + highScores[i].score.ToString();
        }
    }

    public void SaveScores()
    {
        int aux = score.GetScore();
        string auxName = score.GetName();
        HighScoreAndName[] auxArray = new HighScoreAndName[10];
        ScoreData data = SaveSystem.LoadScore();
        for (int i = 0; i < 10; i++)
        {
            auxArray[i] = data.scoreAndName[i];
            scoreText[i].text = i + 1 + ". " + auxArray[i].name + " " + auxArray[i].score.ToString();
        }
        List<HighScoreAndName> list = new List<HighScoreAndName>(auxArray);
        list.Add(new HighScoreAndName(aux, auxName));
        list.Sort((x,y) => x.score.CompareTo(y.score));
        list.Reverse();
        if (list.Count == 11)
        {
            print(11);
            list.Remove(list[10]);
        }
        highScores = list.ToArray();

        SaveSystem.SaveScore();
    }

    public HighScoreAndName[] GetHighScoresAndNames()
    {
        return highScores;
    }
}