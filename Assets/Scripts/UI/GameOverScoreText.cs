using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreText : MonoBehaviour
{
    Text scoreText;
    ScoreManager score;

    void Start()
    {
        score = FindObjectOfType<ScoreManager>();
        scoreText = GetComponent<Text>();
        Debug.Log(ScoreManager.self);
        scoreText.text = score.GetScore().ToString();
    }
}
