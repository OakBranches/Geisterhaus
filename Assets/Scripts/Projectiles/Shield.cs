using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static Player player;
    Collider2D projectileCollider;
    Rigidbody2D rb;
    public float dano = 10f;

    void Start()
    {   
        projectileCollider = GetComponent<Collider2D>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "PlayerProjectile")
        {
            Projectiles.Projectile shield = new Projectiles.Projectile(rb, gameObject);
            Monk.shieldPool.Enqueue(shield);
            gameObject.SetActive(false);
            Projectiles.Projectile playerShoot = 
                new Projectiles.Projectile(other.GetComponent<Rigidbody2D>(), other.gameObject);
            Fire.projectilePool.Enqueue(playerShoot);
            playerShoot.instance.SetActive(false);
        }
        else if (other.transform.tag == "Player")
        {
            if(player.lifeManager.subLife(dano * Time.fixedDeltaTime))
            {
                // Fim de jogo
                GameController.GameOver();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);    
    }
}