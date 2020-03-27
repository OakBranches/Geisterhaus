using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDriver : MonoBehaviour
{
	
	public bool canEnter;

    Camera mainCamera;
    [SerializeField] float cameraSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
		if(gameObject.tag == "Player"){
            if(Input.GetKeyDown(KeyCode.E))
                canEnter = true;
            else
                canEnter = false;
		}
    }
    public void OnTriggerStay2D(Collider2D collider)
    {
        UpstairsCorridorLeftDoor(collider);
        UpstairsCorridorRightDoor(collider);
        ChildBedroomRightDoor(collider);
        ParentBedroomLeftDoor(collider);

        UpstairsCorridorLeftElevator(collider);
        DownstairsCorridorLeftElevator(collider);
        UpstairsCorridorRightElevator(collider);
        DownstairsCorridorRightElevator(collider);

        DownstairsCorridorLeftDoor(collider);
        DownstairsCorridorRightDoor(collider);
        LivingRoomRightDoor(collider);
        KitchenLeftDoor(collider);
  
    }

    private void UpstairsCorridorLeftDoor(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Left Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(-41.5f, -4f);
            StartCoroutine(SmoothCameraPosition(new Vector3(-50f, 0f, -10f), toWhere));
        }
    }

    private void UpstairsCorridorRightDoor(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Right Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(41.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(50f, 0f, -10f), toWhere));
        }
    }

    private void ChildBedroomRightDoor(Collider2D collider)
    {
        if (collider.name == "Child's Bedroom Right Door" && canEnter)
        {   
            Vector2 toWhere = new Vector2(-8.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, 0f, -10f), toWhere));
        }
    }

    private void ParentBedroomLeftDoor(Collider2D collider)
    {
        if (collider.name == "Parent's Bedroom Left Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(8.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, 0f, -10f), toWhere));
        }
    }

    private void UpstairsCorridorLeftElevator(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Left Elevator" && canEnter)
        {
            Vector2 toWhere = new Vector2(-11f, -33f);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, -29f, -10f), toWhere));
        }
    }

    private void DownstairsCorridorLeftElevator(Collider2D collider)
    {
        if (collider.name == "Downstairs Corridor Left Elevator" && canEnter)
        {
            Vector2 toWhere = new Vector2(-11f, -4f);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, 0f, -10f), toWhere));
        }
    }

    private void UpstairsCorridorRightElevator(Collider2D collider)
    {
        if (collider.name == "Upstairs Corridor Right Elevator" && canEnter)
        {
            Vector2 toWhere = new Vector2(11f, -33f);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, -29f, -10f), toWhere));
        }
    }

    private void DownstairsCorridorRightElevator(Collider2D collider)
    {
        if (collider.name == "Downstairs Corridor Right Elevator" && canEnter)
        {
            Vector2 toWhere = new Vector2(11f, -4f);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, 0f, -10f), toWhere));
        }
    }

    private void DownstairsCorridorLeftDoor(Collider2D collider)
    {
        if (collider.name == "Downstairs Corridor Left Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(-41.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(-50f, -29f, -10f), toWhere));
        }
    }

    private void DownstairsCorridorRightDoor(Collider2D collider)
    {
        if (collider.name == "Downstairs Corridor Right Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(41.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(50f, -29f, -10f), toWhere));
        }
    }

    private void LivingRoomRightDoor(Collider2D collider)
    {
        if (collider.name == "Living Room Right Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(-8.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, -29f, -10f), toWhere));
        }
    }

    private void KitchenLeftDoor(Collider2D collider)
    {
        if (collider.name == "Kitchen Left Door" && canEnter)
        {
            Vector2 toWhere = new Vector2(8.5f, transform.position.y);
            StartCoroutine(SmoothCameraPosition(new Vector3(0f, -29f, -10f), toWhere));
        }
    }

    IEnumerator SmoothCameraPosition(Vector3 toWhere, Vector3 playerPosition)
    {
        if (gameObject.tag == "Player")
        {
            Time.timeScale = 0f;
            Vector3 startPosition = mainCamera.transform.position;

            float distance = Vector3.Distance(mainCamera.transform.position, toWhere);
            float startTime = Time.unscaledTime;
            float distanceTraveledPercentage = 0f;

            while (distanceTraveledPercentage < 1)
            {
                float distanceMoved = (Time.unscaledTime - startTime) * cameraSpeed;

                distanceTraveledPercentage = distanceMoved / distance;

                mainCamera.transform.position = Vector3.Lerp(startPosition, toWhere,
                    distanceTraveledPercentage);
                yield return null;
            }
            Time.timeScale = 1f;
        }

        gameObject.transform.position = playerPosition; 
        canEnter = false;
    }
}
