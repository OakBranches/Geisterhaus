using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighScoreAndNameStruct;

[System.Serializable] public class ScoreData
{
    public HighScoreAndName[] scoreAndName;

    public ScoreData (HighScoreDisplay highScores)
    {
        scoreAndName = new HighScoreAndName[10];

        HighScoreAndName[] aux = highScores.GetHighScoresAndNames();

        for (int i = 0; i < 10; i++)
        {
            scoreAndName[i] = aux[i];
        }
    }
}
