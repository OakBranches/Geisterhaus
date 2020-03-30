using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighScoreAndNameStruct;

[System.Serializable] public class ScoreData
{
    public HighScoreAndName[] scoreAndName;

    public ScoreData()
    {
        scoreAndName = new HighScoreAndName[10];

        HighScoreAndName[] aux = HighScoreDisplay.highScores;

        for (int i = 0; i < 10; i++)
        {
            scoreAndName[i] = aux[i];
        }
    }

    public void ToString()
    {
        foreach (HighScoreAndName aux in scoreAndName)
        {
            Debug.Log(aux.name + ": " + aux.score);
        }
    }
}
