using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCameraFrame : MonoBehaviour
{
    public static RectTransform frame;
    public RectTransform miniCamera;

    public static string currentRoom="Downstairs Corridor";
    public static string oldRoom;

    // Start is called before the first frame update
    void Start()
    {
        if (frame == null)
        {
            frame = GameObject.FindGameObjectWithTag("CameraFrame")
                .GetComponent<RectTransform>();
        }
    }
    public string getCurrentRoom(){return currentRoom;}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            currentRoom = transform.name;

            if (oldRoom != currentRoom)
            {
                frame.sizeDelta = miniCamera.sizeDelta;
                frame.transform.position = 
                    miniCamera.transform.position;
            }

            oldRoom = transform.name;
        }
    }
}
