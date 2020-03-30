using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public static Player player;
    Collider2D projectileCollider;
    Rigidbody2D rb;
    public float dano = 10f;
    bool doDamage = false;

    void Start()
    {   
        projectileCollider = GetComponent<Collider2D>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (doDamage)
        {
            if(player.lifeManager.subLife(dano * Time.fixedDeltaTime))
            {
                // Fim de jogo
                GameController.GameOver();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            doDamage = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            doDamage = false;
        }
    }
}