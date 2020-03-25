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

    private void OnTriggerStay2D(Collider2D collider)
    {
        UpstairCorridorLeftDoor(collider);
        UpstairCorridorRightDoor(collider);
        ChildBedroomRightDoor(collider);
        ParentBedroomLeftDoor(collider);
    }

    private void UpstairCorridorLeftDoor(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Left Door" && Input.GetKey(KeyCode.E))
        {
            gameObject.transform.position = new Vector2(-50f, -2f);
            mainCamera.transform.position = new Vector3(-50f, 0f, -10f);
        }
    }

    private void UpstairCorridorRightDoor(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Right Door" && Input.GetKey(KeyCode.E))
        {
            gameObject.transform.position = new Vector2(50f, -2f);
            mainCamera.transform.position = new Vector3(50f, 0f, -10f);
        }
    }

    private void ChildBedroomRightDoor(Collider2D collider)
    {
        if (collider.name == "Child's Bedroom Right Door" && Input.GetKey(KeyCode.E))
        {
            gameObject.transform.position = new Vector2(0f, -2f);
            mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        }
    }
    private void ParentBedroomLeftDoor(Collider2D collider)
    {
        if (collider.name == "Parent's Bedroom Left Door" && Input.GetKey(KeyCode.E))
        {
            gameObject.transform.position = new Vector2(0f, -2f);
            mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        }
    }
}
