using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up : MonoBehaviour
{
    Collider2D collider;
    public float duracao;
    private float  inicio;
    private GameObject player;
    private Player playerScript;
    Fire fire;
    public bool isSpeedPowerUp = true;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        inicio = Time.time;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        fire = player.GetComponent<Fire>();
        playerScript = player.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            if (isSpeedPowerUp)
            {
                // +0.25 chegar em 12
                if (playerScript.moveSpeed < 12)
                {
                    playerScript.moveSpeed += 0.25f;
                }   
            }
            else
            {
                // -0.1 até chegar em 0.5
                if (fire.projectileTimer > 0.5f)
                {
                    fire.projectileTimer -= 0.1f;
                }   
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - inicio > duracao)
        {
            Destroy(gameObject);
        }
    }
}
