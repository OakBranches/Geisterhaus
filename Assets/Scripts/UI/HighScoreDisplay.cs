using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    ScoreManager score;
    public int[] highScores = null;
    public string[] names = null;
    Text[] scoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        highScores = new int[10];
        scoreText = gameObject.GetComponentsInChildren<Text>();
        score = FindObjectOfType<ScoreManager>();

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            UpdateDisplay();
        }
        DontDestroyOnLoad(this);
    }

    public void UpdateDisplay()
    {
        ScoreData data = SaveSystem.LoadScore(this);

        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Indice " + i + ": " + "Valor: " + data.score[i]);
            highScores[i] = data.score[i];
            scoreText[i].text = i + 1 + ". " + highScores[i].ToString();
        }
    }

    public void SaveScores()
    {
        int aux = score.GetScore();
        int[] auxArray = GetHighScores();
        List<int> list = new List<int>(auxArray);
        list.Add(aux);
        list.Sort();
        list.Reverse();
        highScores = list.ToArray();

        SaveSystem.SaveScore(this);
    }
    public int[] GetHighScores()
    {
        return highScores;
    }
    public string[] GetNames()
    {
        return names;
    }
}