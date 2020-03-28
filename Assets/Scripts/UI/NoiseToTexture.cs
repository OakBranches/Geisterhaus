using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseToTexture : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 20f;
    public int fps = 60;
    float fpsTimer = (float) 1f/12f;
    float timer = 0f;
    Renderer renderer;
    float red, green, blue;
    // Start is called before the first frame update
    void Start()
    {
        red = Random.Range(0f, 1f);
        green = Random.Range(0f, 1f);
        blue = Random.Range(0f, 1f);
        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= fpsTimer)
        {
            renderer.material.mainTexture = GenerateTexture();
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / height * scale;

        float sample = Perlin.Noise(xCoord, yCoord, Time.time);
        return new Color(sample * red, sample * green, sample * blue);
    }
}
