﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}