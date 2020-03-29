using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager self;
    private static int score;
    private string name = "N/A";
    public static bool startedPlaying = false;
    private float startTime = 0f;

    void Awake()
    {
        if (self == null)
        {
            self = this;
            score = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game" && startedPlaying)
        {
            score = 0;
            startedPlaying = false;
            startTime = Time.time;
            StartCoroutine(AddScoreByTime());
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public string GetName()
    {
        return name;
    }

    IEnumerator AddScoreByTime()
    {
        while (SceneManager.GetActiveScene().name == "Game")
        {
            if ((int) Time.time > (int) startTime)
            {
                AddScore(1);
                startTime = Time.time;
                Debug.Log(score);
            }
            yield return null;
        }
    }
}
