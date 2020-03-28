using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LifeManager lifeManager;
    public float moveSpeed = .0625f;
    Vector2 input;
    Rigidbody2D rb;
    bool fire;
    bool facingRight = true;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {	
		lifeManager = GetComponent<LifeManager>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
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

        rb.velocity = velocity;
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -newScale.x;
        transform.localScale = newScale;
    }
}
