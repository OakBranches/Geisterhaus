using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolution : MonoBehaviour
{
    Vector2[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = new Vector2[]
        {
            new Vector2(1024, 576),
            new Vector2(1152, 648),
            new Vector2(1280, 720),
            new Vector2(1366, 768),
            new Vector2(1600, 900),
            new Vector2(1920, 1080),
            new Vector2(2560, 1440),
            new Vector2(3840, 2160)
        };

        Resolution highestRes = Screen
            .resolutions[Screen.resolutions.Length - 1];
        
        for (int i = resolutions.Length - 1; i > 0; i--)
        {
            if (((int) resolutions[i].x) <= highestRes.width &&
                ((int) resolutions[i].y) <= highestRes.height)
            {
                SetResolution(resolutions[i]);
                break;
            }
        }
    }

    // Update is called once per frame
    void SetResolution(Vector2 res)
    {
        Screen.SetResolution((int) res.x, (int) res.y, true);
    }
}
