using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGhost : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(0f, 0f, 0f);
    [SerializeField] float period = 10f;

    float movementFactor;
    SpriteRenderer spriteRenderer;


    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SinWave();
    }

    private void SinWave()
    {
        float oldP = transform.position.x;

        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 3f + 0.345f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;

        if (transform.position.x - oldP >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
