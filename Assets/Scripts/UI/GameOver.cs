using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] string arrowName = "Arrow";
    GameObject arrow;

    [SerializeField] string menuUIName = "Menu UI";
    GameObject menuUI;

    string currentScreen;
    string oldScreen;
    string[] screens;

    public PairInt[] limits;

    int index;

    Dictionary<string, Transform> screenDictionary =
        new Dictionary<string, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        List<string> screensList = new List<string>();

        // Iterates through all children
        foreach (Transform child in transform)
        {
            if (child.name == arrowName)
            {
                arrow = child.gameObject;
            }
            else if (child.tag == "UIContainer")
            {
                screensList.Add(child.name);
                screenDictionary.Add(child.name, child);
                if (child.name == menuUIName)
                {
                    currentScreen = child.name;
                    menuUI = child.gameObject;
                }
            }
        }

        screens = screensList.ToArray();
        System.Array.Sort(screens);

        ResetUI(currentScreen);
    }

    // Update is called once per frame
    void Update()
    {
        SetArrowPosition();
        oldScreen = currentScreen;

        if (oldScreen != currentScreen)
        {
            ResetUI(currentScreen);
        }

        DetectEnterInput();
    }

    void SetArrowPosition()
    {
        Debug.Log(currentScreen + " " + oldScreen + " " + index);

        int limitsIndex = System.Array
            .BinarySearch(screens, currentScreen);
        PairInt limit;

        if (limitsIndex >= 0 && limits.Length > 0)
        {
            limit = limits[limitsIndex];
        }
        else
        {
            limit = new PairInt(0, 0);
        }

        if (currentScreen != oldScreen)
        {
            index = limit.x;
        }

        if (Input.GetButtonDown("Up")) index--;
        if (Input.GetButtonDown("Down")) index++;

        if (index < limit.x)
        {
            index = limit.y;
        }

        if (index > limit.y)
        {
            index = limit.x;
        }

        arrow.transform.position =
            new Vector3(arrow.transform.position.x,
            screenDictionary[currentScreen].GetChild(index)
            .transform.position.y,
            transform.position.z);
    }

    void ResetUI(string dontReset)
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "UIContainer")
            {
                child.gameObject.SetActive(false);
            }
            if (child.name == dontReset)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    [System.Serializable]
    public struct PairInt
    {
        public int x;
        public int y;

        public PairInt(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    private void DetectEnterInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameOverOptions(index);
        }
    }
    private void GameOverOptions(int index)
    {
        switch (index)
        {
            case 1:
                SceneManager.LoadScene("Main Menu");
                break;
            case 2:
                Debug.Log("Implementa os high scores ae");
                break;
            case 3:
                Debug.Log("Exit");
                Application.Quit();
                break;
            default:
                Debug.LogError("Please check MainMenu.cs for missing case in MenuOptions");
                break;
        }
    }
}
