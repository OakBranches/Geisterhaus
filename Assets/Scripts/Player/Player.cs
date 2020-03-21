using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = .0625f;
    Vector2 input;
    bool fire;
    bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(input.x * moveSpeed,
            input.y * moveSpeed);

        if ((velocity.x > 0 && !facingRight) ||
            (velocity.x < 0 && facingRight))
        {
            Flip();
            facingRight = !facingRight;
        }

        transform.Translate(velocity);
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -newScale.x;
        transform.localScale = newScale;
    }
}
