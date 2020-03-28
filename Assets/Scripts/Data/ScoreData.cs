using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class ScoreData
{
    public int[] score;
    //public string[] names;

    public ScoreData (HighScoreDisplay highScores)
    {
        score = new int[10];
        //names = new string[10];

        int[] aux = highScores.GetHighScores();
        //string[] nameAux = highScores.GetNames();

        for (int i = 0; i < 10; i++)
        {
            score[i] = aux[i];
            //names[i] = nameAux[i];
        }
    }
}
